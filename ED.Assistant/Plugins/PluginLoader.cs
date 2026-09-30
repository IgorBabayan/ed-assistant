using System.Diagnostics;
using System.IO;

namespace ED.Assistant.Plugins;

public sealed record LoadedPlugin(IPlugin Plugin, string Directory);

public static class PluginLoader
{
    public static IReadOnlyList<LoadedPlugin> LoadAll(string root)
    {
        var result = new List<LoadedPlugin>();
        if (!Directory.Exists(root)) return result;

        foreach (var dir in Directory.GetDirectories(root))
        {
            var dll = IOPath.Combine(dir, IOPath.GetFileName(dir) + ".dll");
            if (!File.Exists(dll)) continue;

            try
            {
                var asm = new PluginLoadContext(dll).LoadFromAssemblyPath(dll);
                foreach (var t in asm.GetExportedTypes()
                             .Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract))
                    result.Add(new LoadedPlugin((IPlugin)Activator.CreateInstance(t)!, dir));
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Failed to load plugin from '{dir}': {ex}");
            }
        }
        return result;
    }
}