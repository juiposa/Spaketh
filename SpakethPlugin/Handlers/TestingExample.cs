using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace SpakethPlugin.Handlers;

public static class TestingExample
{
    private const string ExampleModName = "Spaketh Example & Test";
    private const string ExampleModVersion = "v1.0";
    private static bool ExampleModState = false;
    public static bool Installing = false;

    public static Dictionary<string, string> ModList;
    private static TimeSpan RefershInterval = TimeSpan.FromSeconds(10); // in seconds
    private static DateTime ModListLastFetched = DateTime.UnixEpoch;
    private static DateTime ExampleModLastCheck = DateTime.UnixEpoch;

    static TestingExample()
    {
        FetchModList();
    }


    public static async void EnsureExampleMod()
    {
        Installing = true;
        if (CheckExampleMod()) // already good, nothing to do
            return;

        if (await InstallExampleMod())
        {
            Plugin.Log.Debug("InstallDone");
            CheckExampleMod(true);
            Installing = false;
        }
    }
    
    private static async Task<bool> InstallExampleMod()
    {
        string pluginDirectory = Plugin.PluginInterface.AssemblyLocation.DirectoryName ?? "";
        var exampleModPath = Path.Combine(pluginDirectory, "Data", "penumbra", "spaketh_example.pmp");
        if (!await ModManager.InstallMod(exampleModPath))
        {
            Plugin.Log.Error("Failed to install example mod");
            return false;
        }
        return true;
    }

    
    public static bool CheckExampleMod(bool forceCheck = false)
    {
        if (!forceCheck && !HasLapsed(ExampleModLastCheck))
            return ExampleModState;
        //Plugin.Log.Debug($"Checking example mod state");
        ExampleModLastCheck = DateTime.Now;
        FetchModList(forceCheck);
        foreach (var mod in ModList)
        {
            if (mod.Value.Equals(GetCurrentModVersion())) //check if the mod is installed
            {
                //Plugin.Log.Debug("Example mod is installed and at the correct version");
                ExampleModState = true;
                return ExampleModState;
            } 
            else if (mod.Value.StartsWith(ExampleModName)) // an old version is installed
            {
                //Plugin.Log.Debug("Example mod is installed but needs to be updated");
                //InterfaceManager.Penumbra.DeleteMod.Invoke(mod.ToString(), mod.Value); //delete older version
                ExampleModState = false; //return false to prompt a reinstall
                return ExampleModState;
            }
            else
            {
                ExampleModState = false;
            }
        }
        //if (!ExampleModState)
                //Plugin.Log.Debug("Example mod is not installed");
        return ExampleModState;
    }

    private static string GetCurrentModVersion()
    {
        return $"{ExampleModName} - {ExampleModVersion}";
    }

    private static void FetchModList(bool forced = false)
    {
        if (forced || HasLapsed(ModListLastFetched))
        {
            //Plugin.Log.Debug("Fetching mod list");
            ModList = InterfaceManager.Penumbra.GetModList.Invoke();
            ModListLastFetched = DateTime.Now;
        }
    }

    private static bool HasLapsed(DateTime timestamp)
    {
        return timestamp.Add(RefershInterval).CompareTo(DateTime.Now) == -1;
    }
}
