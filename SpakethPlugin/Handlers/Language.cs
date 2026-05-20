using System.Collections.Generic;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace Spaketh.Handlers;

public static class Language
{
    private static readonly Dictionary<uint, string> AvailableLangauges = new Dictionary<uint, string>()
    {
        { 0, "ja" },
        { 1, "en" },
        { 2, "de" },
        { 3, "fr" }
    };

    private static readonly Dictionary<uint, Lumina.Data.Language> LuminaLanguages = new Dictionary<uint, Lumina.Data.Language>()
    {
        { 0, Lumina.Data.Language.Japanese },
        { 1, Lumina.Data.Language.English },
        { 2, Lumina.Data.Language.German },
        { 3, Lumina.Data.Language.French }
    };
    
    public static string GetClientLanguage()
    {
        uint lang = InterfaceManager.GameConfig.System.GetUInt("CutsceneMovieVoice");
        return AvailableLangauges[lang];
    }

    public static Lumina.Data.Language GetLuminaLanguage()
    {
        
        uint lang = InterfaceManager.GameConfig.System.GetUInt("CutsceneMovieVoice");
        return LuminaLanguages[lang];
    }
}
