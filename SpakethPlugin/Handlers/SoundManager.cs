using System;
using System.Runtime.InteropServices;
using System.Text;
using Dalamud.Game.Config;
using Dalamud.Utility;

namespace Spaketh.Handlers;

public class SoundManager
{
    // Attributed to VFXEditor: https://github.com/0ceal0t/Dalamud-VFXEditor/blob/main/VFXEditor/Interop/ResourceLoader.Sound.cs

    private const string PlaySoundSig = "E8 ?? ?? ?? ?? E9 ?? ?? ?? ?? FE C2";

    private delegate IntPtr PlaySoundDelegate(IntPtr path, byte play);

    private readonly PlaySoundDelegate _playSoundPath;

    private bool _muted;

    private Configuration config;

    public SoundManager(Plugin plugin)
    {
        config = plugin.Configuration;
        InterfaceManager.GameInteropProvider.InitializeFromAttributes(this);
        _playSoundPath =
            Marshal.GetDelegateForFunctionPointer<PlaySoundDelegate>(InterfaceManager.SigScanner.ScanText(PlaySoundSig));
        InterfaceManager.PluginLog.Verbose("Initializing Sound Manager");
    }

    public void PlaySound(String path)
    {
        if (!config.IsEnabled)
        {
            return;
        }

        Plugin.Log.Info($"Playing sound {path}");

        var localPath = path.Replace("xlangx", Language.GetClientLanguage());

        var bytes = Encoding.ASCII.GetBytes(localPath);
        var ptr = Marshal.AllocHGlobal(bytes.Length + 1);

        try
        {
            Marshal.Copy(bytes, 0, ptr, bytes.Length);
            Marshal.WriteByte(ptr + bytes.Length, 0);
            _playSoundPath(ptr, 1);
        }
        catch (Exception e)
        {
            InterfaceManager.PluginLog.Error(e, "Issue In Sound Manager");
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }


    
}


