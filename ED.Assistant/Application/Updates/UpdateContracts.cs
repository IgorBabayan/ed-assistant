namespace ED.Assistant.Application.Updates;

public sealed record UpdateAsset(string Name, Uri DownloadUrl, long Size);

public sealed record UpdateRelease(Version Version, Uri PageUrl, IReadOnlyList<UpdateAsset> Assets);

public interface IUpdateService
{
    Version CurrentVersion { get; }

    /// <summary>False for local/dev builds: CI stamps the GitHub repository into release builds only.</summary>
    bool IsSupported { get; }

    /// <summary>The latest GitHub release if it's newer than this build, otherwise null.</summary>
    Task<UpdateRelease?> FindNewerReleaseAsync(CancellationToken cancellationToken = default);

    /// <summary>Checks, downloads and installs the update, then restarts the app. Never throws.</summary>
    Task CheckAndInstallAsync(CancellationToken cancellationToken = default);
}

/// <summary>Platform-specific part of the update: which asset to take and how to install it.</summary>
public interface IUpdateInstaller
{
    /// <summary>The asset this install can update itself from, or null (portable copy, dev run, unknown arch).</summary>
    UpdateAsset? SelectAsset(UpdateRelease release);

    /// <summary>Starts installing <paramref name="downloadedFile"/>. The app shuts down right after this returns.</summary>
    void StartInstall(string downloadedFile);
}