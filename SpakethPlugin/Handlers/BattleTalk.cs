using System;
using System.Collections.Generic;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using Lumina.Data.Structs.Excel;
using Lumina.Excel.Sheets;
// #pragma warning disable PendingExcelSchema
// // TODO use this sheet when it is no longer experimental
// using ContentDirectorBattleTalk = Lumina.Excel.Sheets.Experimental.ContentDirectorBattleTalk;
// #pragma warning restore PendingExcelSchema

namespace Spaketh.Handlers;

public static class BattleTalk
{
    // Style 0 is for dialogue, Style 6 is the default linkshell/announcement box
    // Style 7 is black and rounded, and Style 11 is blue and sleek
    public static uint[] styles = new uint[] {0, 6, 7, 11};

    private static Dictionary<string, uint> replacementMap;
    
    public static void ShowBattleTalk(string name, uint textId, uint style = 0)
    {
        unsafe
        {
            try
            {
                // get the voiceline text from game filess
                var (text, duration) = Voicelines.GetVoicelineText(textId);
                Plugin.Log.Debug($"Battle Text {textId} {text}");
                
                UIModule.Instance()->ShowBattleTalk(name, text, duration, (byte)style);
            }
            catch (Exception e)
            {
                Plugin.Log.Error(e, "Issue sending Battle Talk!");
            }
        }
    }
}
