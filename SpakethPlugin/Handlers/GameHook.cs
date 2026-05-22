using System;
using System.Collections.Generic;
using System.Text;
using Dalamud.Hooking;
using Dalamud.Interface;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI;
using InteropGenerator.Runtime;
using SpakethPlugin.Model;
using static FFXIVClientStructs.FFXIV.Client.UI.UIModule.Delegates;

namespace SpakethPlugin.Handlers;

public sealed class GameHook : IDisposable
{
    private readonly Hook<ShowBattleTalk>? _showBattleTalkHook;
    private readonly Hook<ShowBattleTalkImage>? _showBattleTalkImageHook;
    private readonly Hook<ShowBattleTalkSound>? _showBattleTalkSoundHook;
    
    internal unsafe GameHook(IGameInteropProvider interop)
    {
        try
        {
            _showBattleTalkHook =
                interop.HookFromAddress<ShowBattleTalk>(
                    UIModule.StaticVirtualTablePointer->ShowBattleTalk, HookShowBattleTalk);

            _showBattleTalkImageHook = interop.HookFromAddress<ShowBattleTalkImage>(
                UIModule.StaticVirtualTablePointer->ShowBattleTalkImage, HookShowBattleTalkImage);

            _showBattleTalkSoundHook = interop.HookFromAddress<ShowBattleTalkSound>(
                UIModule.StaticVirtualTablePointer->ShowBattleTalkSound, HookShowBattleTalkSound);
        }
        catch (Exception ex)
        {
            Plugin.Log.Error($"Failed to start up hook {ex.Message}");
        }
    }
    
    public void StartHooks()
    {
        Plugin.Log.Debug("Starting game hooks");
        _showBattleTalkHook?.Enable();
        _showBattleTalkImageHook?.Enable();
        _showBattleTalkSoundHook?.Enable();
    }

    public void StopHooks()
    {
        Plugin.Log.Debug("Stopping game hooks");
        _showBattleTalkHook?.Disable();
        _showBattleTalkImageHook?.Disable();
        _showBattleTalkSoundHook?.Disable();
    }

    private unsafe void HookShowBattleTalk(UIModule* self, CStringPointer sender, CStringPointer talk, float duration, byte style)
    {
        Plugin.Log.Debug($"HOOK {sender} {talk}");
        var proceed = () => _showBattleTalkHook!.Original(self, sender, talk, duration, style);
        var intercept = (Playback p) => _showBattleTalkHook!.Original(self, Stocs(p.GetName(sender.ToString())), Stocs(p.Text), p.Duration, style);
        ProcessHook(talk.ToString(), proceed, intercept);
    }
    
    private unsafe void HookShowBattleTalkImage(UIModule* self, CStringPointer sender, CStringPointer talk, float duration, uint image, byte style, int sound, uint entityId)
    {
        Plugin.Log.Debug($"HOOK IMAGE {image} {sender} {talk}");
        var proceed = () => _showBattleTalkImageHook!.Original(self, sender, talk, duration, image, style, sound, entityId);
        var intercept = (Playback p) => _showBattleTalkImageHook!.Original(self, Stocs(p.GetName(sender.ToString())), Stocs(p.Text), p.Duration, p.GetImage(image), style, sound, entityId);
        ProcessHook(talk.ToString(), proceed, intercept);
    }
    
    private unsafe void HookShowBattleTalkSound(UIModule* self, CStringPointer sender, CStringPointer talk, float duration, int sound, byte style)
    {
        Plugin.Log.Debug($"HOOK SOUND {sender} {talk}");
        var proceed = () => _showBattleTalkSoundHook!.Original(self, sender, talk, duration, sound, style);
        var intercept = (Playback p) => _showBattleTalkSoundHook!.Original(self, Stocs(p.GetName(sender.ToString())), Stocs(p.Text), p.Duration, sound, style);
        ProcessHook(talk.ToString(), proceed, intercept);
    }

    private void ProcessHook(string originalLine, Action proceed, Delegate intercept)
    {
        
        if (!Plugin.TestMode && !Voicelines.IsInSupportedInstance())
        {
            Plugin.Log.Debug("SKIP Not in Supported Instance");
            proceed.Invoke();
            return;
        }

        if (Plugin.TestMode)
            Plugin.TestMode = false;
        
        var playback  = Voicelines.ReplaceVoiceline(originalLine);
        if (playback != null)
        {
            Plugin.Log.Debug($"INTERCEPT {playback.Name} {playback.Duration} {playback.GetImage(0)} {playback.Text}");
            intercept.DynamicInvoke(playback);
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
        _showBattleTalkHook?.Dispose();
        _showBattleTalkImageHook?.Dispose();
        _showBattleTalkSoundHook?.Dispose();
    }
}
