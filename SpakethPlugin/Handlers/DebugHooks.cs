using System;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI;

namespace SpakethPlugin.Handlers;

public class DebugHooks : IDisposable
{
    private readonly Hook<UIModule.Delegates.ShowImage>? _showImage;

    internal unsafe DebugHooks(IGameInteropProvider interop)
    {
        if (Plugin.ClientState.TerritoryType != 641)
            return;
        try
        {
            _showImage =
                interop.HookFromAddress<UIModule.Delegates.ShowImage>(
                    UIModule.StaticVirtualTablePointer->ShowImage, HookShowImage);

        }
        catch (Exception ex)
        {
            Plugin.Log.Error($"Failed to start up hook {ex.Message}");
        }
        _showImage.Enable();
    }

    private unsafe void HookShowImage(UIModule* self, uint imageId, bool useLocalePath, int displayType, bool playSound)
    {
        Plugin.Log.Debug($"SHOW IMAGE {imageId} {useLocalePath} {displayType} {playSound}");
        _showImage!.Original(self, imageId, useLocalePath, displayType, playSound);
    }

    public void Dispose()
    {
        _showImage.Dispose();
    }
}
