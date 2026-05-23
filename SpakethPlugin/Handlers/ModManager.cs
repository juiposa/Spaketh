using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Penumbra.Api.Enums;
using Penumbra.Api.IpcSubscribers;

namespace SpakethPlugin.Handlers;

public static class ModManager
{

    private const string ModPath = "Spaketh";
    
    private static bool PenumbraAvailable = false;
    private static TimeSpan RefershInterval = TimeSpan.FromSeconds(10); // in seconds
    private static DateTime PenumbraAvailableLastChecked = DateTime.UnixEpoch;

    public static bool IsPenumbraAvailable()
    {
        if (!HasLapsed(PenumbraAvailableLastChecked))
            return PenumbraAvailable;
        PenumbraAvailableLastChecked = DateTime.Now;
        
        try
        {
            InterfaceManager.Penumbra.ApiVersion.Invoke();
        }
        catch
        {
            PenumbraAvailable = false;
            return PenumbraAvailable;
        }
        
        PenumbraAvailable = true;
        return PenumbraAvailable;
    }
    
    public static async Task<bool> InstallMod(string filepath)
    {
        Plugin.Log.Debug($"Installing mod {filepath}");
        
        var interfaceCollection = InterfaceManager.Penumbra.GetCollection.Invoke(ApiCollectionType.Interface);
        if (interfaceCollection == null)
        {
            Plugin.Log.Error($"No Penumbra collections set on Interface");
            return false;
        }

        // var modAddedTask = InterfaceManager.Penumbra.GetLastAddedMod();
        // modAddedTask.Start();
        
        var result = InterfaceManager.Penumbra.InstallMod.Invoke(filepath);
        if (result == PenumbraApiEc.FileMissing)
        {
            Plugin.Log.Error($"Invalid mod path {filepath}");
            return false;
        }
        
        //var newModName = await modAddedTask; 

        await Task.Delay(1500);
        
        var newModName = "Spaketh Example & Test - v1.0";
        
        Plugin.Log.Debug($"MOD ADD {newModName}");
        
        result = InterfaceManager.Penumbra.SetModPath.Invoke("", ModPath, newModName);
        if (result != PenumbraApiEc.Success)
        {
            Plugin.Log.Error($"Error moving mod to subpath {result} {newModName}");
            return false;
        }
        
        result = InterfaceManager.Penumbra.TrySetMod.Invoke(interfaceCollection!.Value.Id, ModPath, true, newModName);
        if (result != PenumbraApiEc.Success && result != PenumbraApiEc.NothingChanged)
        {
            Plugin.Log.Error($"Error enabling mod {interfaceCollection.Value.Name} {result} {newModName}");
            return false;
        }
        
        result = InterfaceManager.Penumbra.OpenMainWindow.Invoke(TabType.Mods, ModPath, newModName);
        if (result != PenumbraApiEc.Success)
        {
            Plugin.Log.Error($"Error enabling opening Penumbra Window");
        }
        
        return true;
    }
    
    private static bool HasLapsed(DateTime timestamp)
    {
        return timestamp.Add(RefershInterval).CompareTo(DateTime.Now) == -1;
    }
}
