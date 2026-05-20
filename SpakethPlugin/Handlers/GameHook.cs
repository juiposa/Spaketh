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
    

    public static readonly List<uint> ActiveInstances = new List<uint> { 1205, 887, 583, 587 }; //Tuli Inn, TEA, A12, A12S

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
        var proceed = () => showBattleTalkHook!.Original(self, sender, talk, duration, style);
        var intercept = (string n, string t, uint d) => showBattleTalkHook!.Original(self, Stocs(n), Stocs(t), d, style);
        ProcessHook(sender.ToString(), talk.ToString(), proceed, intercept);
    }
    
    private unsafe void HookShowBattleTalkImage(UIModule* self, CStringPointer sender, CStringPointer talk, float duration, uint image, byte style, int sound, uint entityId)
    {
        Plugin.Log.Debug($"HOOK IMAGE {sender} {talk}");
        var proceed = () => showBattleTalkImageHook!.Original(self, sender, talk, duration, image, style, sound, entityId);
        var intercept = (string n, string t, uint d) => showBattleTalkImageHook!.Original(self, Stocs(n), Stocs(t), d, image, style, sound, entityId);
        ProcessHook(sender.ToString(), talk.ToString(), proceed, intercept);
    }
    
    private unsafe void HookShowBattleTalkSound(UIModule* self, CStringPointer sender, CStringPointer talk, float duration, int sound, byte style)
    {
        Plugin.Log.Debug($"HOOK SOUND {sender} {talk}");
        var proceed = () => showBattleTalkSoundHook!.Original(self, sender, talk, duration, sound, style);
        var intercept = (string n, string t, uint d) => showBattleTalkSoundHook!.Original(self, Stocs(n), Stocs(t), d, sound, style);
        ProcessHook(sender.ToString(), talk.ToString(), proceed, intercept);
    }

    private void ProcessHook(string sender, string originalLine, Action proceed,  Delegate intercept)
    {
        
        if (!ActiveInstances.Contains(Plugin.ClientState.TerritoryType))
        {
            Plugin.Log.Debug("SKIP Not in Supported Instance");
            proceed.Invoke();
            return;
        }
        
        var (replacement, newDuration, newName) = Voicelines.ReplaceVoiceline(originalLine);
        if (replacement != null)
        {
            var callName = newName ?? sender;
            Plugin.Log.Debug($"INTERCEPT {newName} {replacement}");
            intercept.DynamicInvoke(callName, replacement, newDuration);
            return;
        }
        proceed.Invoke();
    }

    private unsafe byte* Stocs(string str)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        fixed (byte* pbyte = bytes)
        {
            return pbyte;
        }
    }

    public void Dispose()
    {
        showBattleTalkHook?.Dispose();
        showBattleTalkImageHook?.Dispose();
        showBattleTalkSoundHook?.Dispose();
    }
}
