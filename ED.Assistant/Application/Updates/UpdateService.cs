using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json.Serialization;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using ED.Assistant.Application.Notifications;
using Material.Icons;

namespace ED.Assistant.Application.Updates;

internal sealed class UpdateService(IUpdateInstaller installer, AlertService alerts) : IUpdateService
{
	// Written by CI: dotnet publish -p:UpdateRepository=owner/repo (see ED.Assistant.csproj)
	private const string RepositoryMetadataKey = "UpdateRepository";

	private static readonly HttpClient Http = CreateHttpClient();

	public Version CurrentVersion { get; } = ReadCurrentVersion();

	private string? Repository { get; } = ReadRepository();

	public bool IsSupported => Repository is not null;

	public async Task<UpdateRelease?> FindNewerReleaseAsync(CancellationToken cancellationToken = default)
	{
		if (Repository is null)
			return null;

		// /releases/latest only returns the newest published, non-prerelease release
		using var request = new HttpRequestMessage(HttpMethod.Get,
			$"https://api.github.com/repos/{Repository}/releases/latest");
		request.Headers.Accept.ParseAdd("application/vnd.github+json");

		using var response = await Http.SendAsync(request, cancellationToken);
		if (response.StatusCode == HttpStatusCode.NotFound)
			return null; // nothing published yet

		response.EnsureSuccessStatusCode();

		var release = await response.Content.ReadFromJsonAsync<GitHubRelease>(cancellationToken);
		if (release is null || !TryParseVersion(release.TagName, out var version) || version <= CurrentVersion)
			return null;

		var assets = (release.Assets ?? [])
			.Select(a => new UpdateAsset(a.Name, new Uri(a.BrowserDownloadUrl), a.Size))
			.ToList();

		return new UpdateRelease(version, new Uri(release.HtmlUrl), assets);
	}

	public async Task CheckAndInstallAsync(CancellationToken cancellationToken = default)
	{
		try
		{
			var release = await FindNewerReleaseAsync(cancellationToken);
			if (release is null)
				return;

			var asset = installer.SelectAsset(release);
			if (asset is null)
			{
				// Portable zip, dev run, read-only location...: can't replace ourselves, just tell the user
				alerts.Notify("Update available",
					$"ED Assistant v{release.Version} is available on the GitHub Releases page.",
					MaterialIconKind.Update);
				return;
			}

			alerts.Notify("Downloading update", $"ED Assistant v{release.Version}…", MaterialIconKind.Download);
			var file = await DownloadAsync(asset, cancellationToken);

			installer.StartInstall(file);

			alerts.Notify("Installing update", $"ED Assistant will restart as v{release.Version}.",
				MaterialIconKind.Update);

			// Let the toast show for a moment; the installer/script waits for this process to exit
			await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
			await Dispatcher.UIThread.InvokeAsync(() =>
			{
				if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
					desktop.Shutdown();
			});
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex)
		{
			// Updating is best effort: a failure must never break the app
			Trace.WriteLine($"Auto-update failed: {ex}");
			alerts.Notify("Update failed", ex.Message, MaterialIconKind.AlertCircleOutline);
		}
	}

	private static async Task<string> DownloadAsync(UpdateAsset asset, CancellationToken cancellationToken)
	{
		var folder = global::System.IO.Path.Combine(global::System.IO.Path.GetTempPath(), "ed-assistant-update");
		try
		{
			// Leftovers from a previous update
			if (Directory.Exists(folder))
				Directory.Delete(folder, recursive: true);
		}
		catch (IOException)
		{
		}

		Directory.CreateDirectory(folder);
		var target = global::System.IO.Path.Combine(folder, asset.Name);
		var partial = target + ".part";

		using (var response = await Http.GetAsync(asset.DownloadUrl, HttpCompletionOption.ResponseHeadersRead,
			       cancellationToken))
		{
			response.EnsureSuccessStatusCode();
			await using var source = await response.Content.ReadAsStreamAsync(cancellationToken);
			await using var destination = File.Create(partial);
			await source.CopyToAsync(destination, cancellationToken);
		}

		if (asset.Size > 0 && new FileInfo(partial).Length != asset.Size)
			throw new IOException($"Download of {asset.Name} is incomplete.");

		File.Move(partial, target, overwrite: true);
		return target;
	}

	private static HttpClient CreateHttpClient()
	{
		var client = new HttpClient { Timeout = TimeSpan.FromMinutes(10) };
		// GitHub rejects API requests without a User-Agent
		client.DefaultRequestHeaders.UserAgent.ParseAdd("ED-Assistant-Updater");
		return client;
	}

	private static Version ReadCurrentVersion()
		=> Normalize(Assembly.GetEntryAssembly()?.GetName().Version ?? new Version(0, 0));

	private static string? ReadRepository()
		=> Assembly.GetEntryAssembly()?
			.GetCustomAttributes<AssemblyMetadataAttribute>()
			.FirstOrDefault(a => a.Key == RepositoryMetadataKey)?.Value is { Length: > 0 } repository
			? repository
			: null;

	private static bool TryParseVersion(string? tag, out Version version)
	{
		version = new Version(0, 0);
		if (string.IsNullOrWhiteSpace(tag) || !Version.TryParse(tag.TrimStart('v', 'V'), out var parsed))
			return false;

		version = Normalize(parsed);
		return true;
	}

	// The assembly says 1.0.42.0, the tag says 1.0.42: compare major.minor.build only
	private static Version Normalize(Version v) => new(v.Major, v.Minor, Math.Max(v.Build, 0));

	private sealed record GitHubRelease(
		[property: JsonPropertyName("tag_name")] string TagName,
		[property: JsonPropertyName("html_url")] string HtmlUrl,
		[property: JsonPropertyName("assets")] IReadOnlyList<GitHubAsset>? Assets);

	private sealed record GitHubAsset(
		[property: JsonPropertyName("name")] string Name,
		[property: JsonPropertyName("browser_download_url")] string BrowserDownloadUrl,
		[property: JsonPropertyName("size")] long Size);
}