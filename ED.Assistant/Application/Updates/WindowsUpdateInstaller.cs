using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace ED.Assistant.Application.Updates;

// ───────────────────────────── Windows: silent Inno Setup upgrade ─────────────────────────────
[SupportedOSPlatform("windows")]
internal sealed class WindowsUpdateInstaller : IUpdateInstaller
{
	// Inno Setup puts its uninstaller next to the app; a copy unpacked from the portable zip has none
	private static bool IsInstalled =>
		File.Exists(global::System.IO.Path.Combine(AppContext.BaseDirectory, "unins000.exe"));

	public UpdateAsset? SelectAsset(UpdateRelease release)
		=> IsInstalled
			? release.Assets.FirstOrDefault(a => a.Name.EndsWith("-win-x64-setup.exe", StringComparison.OrdinalIgnoreCase))
			: null;

	public void StartInstall(string downloadedFile)
	{
		// Same AppId => Inno upgrades the existing install in place (same folder, same shortcuts).
		// /CLOSEAPPLICATIONS closes us if we're still running; the [Run] entry with
		// Check: WizardSilent in ED.Assistant.iss starts the new version afterwards.
		Process.Start(new ProcessStartInfo(downloadedFile)
		{
			UseShellExecute = true, // lets Windows show UAC if it was installed for all users
			Arguments = "/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /CLOSEAPPLICATIONS /NOCANCEL"
		})?.Dispose();
	}
}

// ───────────────────────────── Linux: replace the AppImage ─────────────────────────────
[SupportedOSPlatform("linux")]
internal sealed class LinuxUpdateInstaller : IUpdateInstaller
{
	private const string RelaunchScript = """
		trap '' HUP
		while kill -0 "$1" 2>/dev/null; do sleep 0.5; done
		exec "$2"
		""";

	// Set by the AppImage runtime; null when running from a publish folder or the IDE
	private static string? CurrentAppImage =>
		Environment.GetEnvironmentVariable("APPIMAGE") is { Length: > 0 } path && File.Exists(path) ? path : null;

	public UpdateAsset? SelectAsset(UpdateRelease release)
		=> CurrentAppImage is not null && RuntimeInformation.OSArchitecture == Architecture.X64
			? release.Assets.FirstOrDefault(a => a.Name.EndsWith("-x86_64.AppImage", StringComparison.OrdinalIgnoreCase))
			: null;

	public void StartInstall(string downloadedFile)
	{
		var target = CurrentAppImage ?? throw new InvalidOperationException("Not running from an AppImage.");

		// Stage next to the target so the final rename stays on one filesystem (atomic).
		// The file keeps its old name on purpose: .desktop entries and shortcuts point at it.
		var staged = target + ".update";
		File.Copy(downloadedFile, staged, overwrite: true);
		File.SetUnixFileMode(staged, File.GetUnixFileMode(target)
			| UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute);

		// Safe while running: the mounted old image keeps its inode until this process exits
		File.Move(staged, target, overwrite: true);
		File.Delete(downloadedFile);

		UpdateScript.RunAfterExit(RelaunchScript, target);
	}
}

// ───────────────────────────── macOS: swap the .app bundle from the DMG ─────────────────────────────
[SupportedOSPlatform("macos")]
internal sealed class MacUpdateInstaller : IUpdateInstaller
{
	// $1 = our PID, $2 = dmg, $3 = current .app. Rolls back to the old bundle if the copy fails.
	private const string InstallScript = """
		trap '' HUP
		while kill -0 "$1" 2>/dev/null; do sleep 0.5; done
		DMG="$2"; APP="$3"
		MNT="$(mktemp -d /tmp/ed-assistant-update.XXXXXX)"
		if hdiutil attach -nobrowse -noautoopen -quiet -mountpoint "$MNT" "$DMG"; then
			SRC="$(find "$MNT" -maxdepth 1 -name '*.app' | head -n 1)"
			if [ -n "$SRC" ] && mv "$APP" "$APP.old"; then
				if ditto "$SRC" "$APP"; then rm -rf "$APP.old"; else rm -rf "$APP"; mv "$APP.old" "$APP"; fi
				xattr -dr com.apple.quarantine "$APP" 2>/dev/null
			fi
			hdiutil detach -quiet "$MNT"
		fi
		rmdir "$MNT" 2>/dev/null
		rm -f "$DMG"
		open "$APP"
		""";

	// .../ED Assistant.app/Contents/MacOS/  ->  .../ED Assistant.app
	private static string? CurrentBundle
	{
		get
		{
			var macOs = new DirectoryInfo(AppContext.BaseDirectory.TrimEnd('/'));
			var bundle = macOs.Parent?.Parent;
			return macOs.Name == "MacOS"
			       && bundle?.Extension == ".app"
			       && !bundle.FullName.StartsWith("/Volumes/", StringComparison.Ordinal) // still running from the DMG
				? bundle.FullName
				: null;
		}
	}

	public UpdateAsset? SelectAsset(UpdateRelease release)
	{
		if (CurrentBundle is null)
			return null;

		var rid = RuntimeInformation.OSArchitecture == Architecture.Arm64 ? "osx-arm64" : "osx-x64";
		return release.Assets.FirstOrDefault(a => a.Name.EndsWith($"-{rid}.dmg", StringComparison.OrdinalIgnoreCase));
	}

	public void StartInstall(string downloadedFile)
	{
		var bundle = CurrentBundle ?? throw new InvalidOperationException("Not running from an .app bundle.");
		UpdateScript.RunAfterExit(InstallScript, downloadedFile, bundle);
	}
}

// ───────────────────────────── Fallback ─────────────────────────────
internal sealed class NullUpdateInstaller : IUpdateInstaller
{
	public UpdateAsset? SelectAsset(UpdateRelease release) => null;

	public void StartInstall(string downloadedFile) => throw new PlatformNotSupportedException();
}

internal static class UpdateScript
{
	/// <summary>Runs a /bin/sh script that outlives this process. $1 is our PID, then <paramref name="args"/>.</summary>
	public static void RunAfterExit(string script, params string[] args)
	{
		var info = new ProcessStartInfo("/bin/sh") { UseShellExecute = false };
		info.ArgumentList.Add("-c");
		info.ArgumentList.Add(script);
		info.ArgumentList.Add("ed-assistant-updater"); // $0
		info.ArgumentList.Add(Environment.ProcessId.ToString(CultureInfo.InvariantCulture));
		foreach (var arg in args)
			info.ArgumentList.Add(arg);

		Process.Start(info)?.Dispose();
	}
}