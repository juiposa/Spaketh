using System.IO;
using System.Threading.Tasks;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Penumbra.Api.Helpers;
using Penumbra.Api.IpcSubscribers;

namespace SpakethPlugin.Integration;

public class Penumbra
{
    public GetCollection GetCollection;
    public InstallMod InstallMod;
    public DeleteMod DeleteMod;
    public GetModList GetModList;
    public SetModPath SetModPath;
    public OpenMainWindow OpenMainWindow;
    public ApiVersion ApiVersion;
    public TrySetMod TrySetMod;
    public EventSubscriber<string> ModAddedSub;
    
    public Penumbra(IDalamudPluginInterface pluginInterface)
    {
        GetCollection = new GetCollection(pluginInterface);
        InstallMod = new InstallMod(pluginInterface);
        DeleteMod = new DeleteMod(pluginInterface);
        GetModList = new GetModList(pluginInterface);
        SetModPath = new SetModPath(pluginInterface);
        OpenMainWindow = new OpenMainWindow(pluginInterface);
        ApiVersion = new ApiVersion(pluginInterface);
        TrySetMod = new TrySetMod(pluginInterface);

        ModAddedSub = ModAdded.Subscriber(pluginInterface, GetModAdded);
    }

    private string? lastModAdded;

    private void GetModAdded(string modName)
    {
        lastModAdded = modName;
    }

    public async Task<string> GetLastAddedMod()
    {
        ModAddedSub.Enable();
        while (lastModAdded == null)
        {
            Task.Delay(100);
        }
        ModAddedSub.Disable();
        var returnVal = lastModAdded;
        lastModAdded = null;
        return returnVal;
    }
}
