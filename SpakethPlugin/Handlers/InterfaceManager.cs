using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace SpakethPlugin.Handlers;

#pragma warning disable 8618
public class InterfaceManager
{
    [PluginService] public static IDataManager DataManager { get; private set; }

    [PluginService] public static ISigScanner SigScanner { get; private set; }

    [PluginService] public static IGameInteropProvider GameInteropProvider { get; private set; }

    [PluginService] public static IPluginLog PluginLog { get; private set; }
    
    [PluginService] public static IGameConfig GameConfig { get; private set; }
    
    public static SoundManager SoundManager { get; private set; }

    internal static void Init(Plugin plugin, IDalamudPluginInterface pluginInterface)
    {
        pluginInterface.Create<InterfaceManager>();
        SoundManager = new SoundManager(plugin);
    }
}
