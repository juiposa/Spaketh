using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Dalamud.Storage.Assets;

namespace Spaketh.Handlers;

#pragma warning disable 8618
public class InterfaceManager
{
    [PluginService] public static IDataManager DataManager { get; private set; }

    [PluginService] public static ISigScanner SigScanner { get; private set; }

    [PluginService] public static ICommandManager CommandManager { get; private set; }

    [PluginService] public static IChatGui ChatGui { get; private set; }

    [PluginService] public static IObjectTable ObjectTable { get; private set; }

    [PluginService] public static IPartyList PartyList { get; private set; }

    [PluginService] public static IClientState ClientState { get; private set; }

    [PluginService] public static IPlayerState PlayerState { get; private set; }

    [PluginService] public static IDalamudPluginInterface DalamudPluginInterface { get; private set; }

    [PluginService] public static IFramework Framework { get; private set; }

    [PluginService] public static ICondition Condition { get; private set; }

    [PluginService] public static IGameInteropProvider GameInteropProvider { get; private set; }

    [PluginService] public static IPluginLog PluginLog { get; private set; }

    [PluginService] public static IDutyState DutyState { get; private set; }

    [PluginService] public static IGameConfig GameConfig { get; private set; }

    [PluginService] public static INotificationManager NotificationManager { get; private set; }

    [PluginService] public static IAddonLifecycle AddonLifecycle { get; private set; }

    [PluginService] public static ISeStringEvaluator SeStringEvaluator { get; private set; }

    [PluginService] public static ITextureProvider TextureProvider { get; private set; }
    
    [PluginService] public static IDalamudAssetManager AssetManager { get; private set; }
    
    public static SoundManager SoundManager { get; private set; }

    internal static void Init(Plugin plugin, IDalamudPluginInterface pluginInterface)
    {
        pluginInterface.Create<InterfaceManager>();
        SoundManager = new SoundManager(plugin);
    }
}
