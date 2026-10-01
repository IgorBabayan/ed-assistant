using ED.Assistant.Plugins;

namespace ED.Assistant.Application.Plugins;

internal sealed record PluginContext(string PluginDirectory, string DataDirectory, string DatabasePath)
    : IPluginContext;
