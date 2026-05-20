using System;
using System.Collections.Generic;
using System.Text;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI;
using InteropGenerator.Runtime;
using static FFXIVClientStructs.FFXIV.Client.UI.UIModule.Delegates;

namespace Spaketh.Handlers;

public sealed class GameHook : IDisposable
{
    private readonly Hook<ShowBattleTalk>? showBattleTalkHook;
    private readonly Hook<ShowBattleTalkImage>? showBattleTalkImageHook;
    private readonly Hook<ShowBattleTalkSound>? showBattleTalkSoundHook;
    

    public static readonly List<uint> ActiveInstances = new List<uint> { 887, 583, 587 }; //TEA, A12, A12S

    internal unsafe GameHook(IGameInteropProvider interop)
    {
        try
        {
            showBattleTalkHook =
                interop.HookFromAddress<ShowBattleTalk>(
                    UIModule.StaticVirtualTablePointer->ShowBattleTalk, HookShowBattleTalk);

            showBattleTalkImageHook = interop.HookFromAddress<ShowBattleTalkImage>(
                UIModule.StaticVirtualTablePointer->ShowBattleTalkImage, HookShowBattleTalkImage);

            showBattleTalkSoundHook = interop.HookFromAddress<ShowBattleTalkSound>(
                UIModule.StaticVirtualTablePointer->ShowBattleTalkSound, HookShowBattleTalkSound);
            StartHooks();
        }
        catch (Exception ex)
        {
            Plugin.Log.Error($"Failed to start up hook {ex.Message}");
        }
    }
    
    public void StartHooks()
    {
        Plugin.Log.Debug("Starting game hook");
        showBattleTalkHook?.Enable();
        showBattleTalkImageHook?.Enable();
        showBattleTalkSoundHook?.Enable();
    }

    public void StopHooks()
    {
        Plugin.Log.Debug("Stopping game hook");
        showBattleTalkHook?.Disable();
        showBattleTalkImageHook?.Disable();
        showBattleTalkSoundHook?.Disable();
    }

    private unsafe void HookShowBattleTalk(UIModule* self, CStringPointer sender, CStringPointer talk, float duration, byte style)
    {
        Plugin.Log.Debug($"HOOK {sender} {talk}");
        if (!ActiveInstances.Contains(Plugin.ClientState.TerritoryType))
        {
            Plugin.Log.Debug($"SKIPPING Not in Supported Instance");
            showBattleTalkHook!.Original(self, sender, talk, duration, style);
            return;
        }
        
        var (replacement, newDuration) = Voicelines.ReplaceVoiceline(talk.ToString());
        if (replacement != null)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(replacement);
            fixed (byte* pbyte = bytes)
            {
                showBattleTalkHook!.Original(self, sender, new CStringPointer(pbyte), newDuration, style);
            }
            return;
        }
        showBattleTalkHook!.Original(self, sender, talk, duration, style);
    }
    
    private unsafe void HookShowBattleTalkImage(UIModule* self, CStringPointer sender, CStringPointer talk, float duration, uint image, byte style, int sound, uint entityId)
    {
        Plugin.Log.Debug($"HOOK IMAGE {sender} {talk}");
        if (!ActiveInstances.Contains(Plugin.ClientState.TerritoryType))
        {
            Plugin.Log.Debug($"SKIPPING Not in Supported Instance");
            showBattleTalkImageHook!.Original(self, sender, talk, duration, image, style, sound, entityId);
            return;
        }
        
        var (replacement, newDuration) = Voicelines.ReplaceVoiceline(talk.ToString());
        if (replacement != null)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(replacement);
            fixed (byte* pbyte = bytes)
            {
                showBattleTalkImageHook!.Original(self, sender, new CStringPointer(pbyte), newDuration, image, style, sound, entityId);
            }
            return;
        }
        showBattleTalkImageHook!.Original(self, sender, talk, duration, image, style, sound, entityId);
    }
    
    private unsafe void HookShowBattleTalkSound(UIModule* self, CStringPointer sender, CStringPointer talk, float duration, int sound, byte style)
    {
        Plugin.Log.Debug($"HOOK SOUND {sender} {talk}");
        if (!ActiveInstances.Contains(Plugin.ClientState.TerritoryType))
        {
            Plugin.Log.Debug($"SKIPPING Not in Supported Instance");
            showBattleTalkSoundHook!.Original(self, sender, talk, duration, sound, style);
            return;
        }
        
        var (replacement, newDuration) = Voicelines.ReplaceVoiceline(talk.ToString());
        if (replacement != null)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(replacement);
            fixed (byte* pbyte = bytes)
            {
                showBattleTalkSoundHook!.Original(self, sender, new CStringPointer(pbyte), newDuration, sound, style);
            }
            return;
        }
        showBattleTalkSoundHook!.Original(self, sender, talk, duration, sound, style);
    }

    public void Dispose()
    {
        showBattleTalkHook?.Dispose();
        showBattleTalkImageHook?.Dispose();
        showBattleTalkSoundHook?.Dispose();
    }
}
