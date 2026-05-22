using System;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;

// #pragma warning disable PendingExcelSchema
// // TODO use this sheet when it is no longer experimental
// using ContentDirectorBattleTalk = Lumina.Excel.Sheets.Experimental.ContentDirectorBattleTalk;
// #pragma warning restore PendingExcelSchema

namespace SpakethPlugin.Handlers;

public static class BattleTalk
{
    // Style 0 is for dialogue, Style 6 is the default linkshell/announcement box
    // Style 7 is black and rounded, and Style 11 is blue and sleek
    public static uint[] styles = new uint[] {0, 6, 7, 11};
    
    public static void ShowBattleTalk(string name, string text, uint duration, uint style = 0)
    {
        unsafe
        {
            try
            {
                // get the voiceline text from game filess
                Plugin.Log.Debug($"Battle Text {name} {text}");
                
                UIModule.Instance()->ShowBattleTalk(name, text, duration, (byte)style);
            }
            catch (Exception e)
            {
                Plugin.Log.Error(e, "Issue sending Battle Talk");
            }
        }
    }
    
    public static void ShowBattleTalkImage(string name, string text, uint duration, uint style = 0)
    {
        unsafe
        {
            try
            {
                // get the voiceline text from game files
                Plugin.Log.Debug($"Battle Text w/ Image {name} {text}");
                
                UIModule.Instance()->ShowBattleTalkImage(name, text, (float)duration, 0, (byte)style);
            }
            catch (Exception e)
            {
                Plugin.Log.Error(e, "Issue sending Battle Talk w/ Image");
            }
        }
    }
}
