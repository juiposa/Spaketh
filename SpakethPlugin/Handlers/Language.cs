using System.Collections.Generic;

namespace SpakethPlugin.Handlers;

public static class Language
{
    private static readonly Dictionary<uint, string> AvailableLangauges = new Dictionary<uint, string>()
    {
        { 0, "ja" },
        { 1, "en" },
        { 2, "de" },
        { 3, "fr" }
    };

    private static readonly Dictionary<string, Lumina.Data.Language> LuminaLanguages = new Dictionary<string, Lumina.Data.Language>()
    {
        { "ja", Lumina.Data.Language.Japanese },
        { "en", Lumina.Data.Language.English },
        { "de", Lumina.Data.Language.German },
        { "fr", Lumina.Data.Language.French }
    };
    
    public static string GetVoiceoverLanguage()
    {
        uint lang = InterfaceManager.GameConfig.System.GetUInt("CutsceneMovieVoice");
        return AvailableLangauges[lang];
    }

    public static string GetTextLanguage()
    {
        return AvailableLangauges[(uint)Plugin.ClientState.ClientLanguage];
    }
    
    public static Lumina.Data.Language GetVoiceoverLanguageLumina()
    {
        
        return GetLuminaLanguage(GetVoiceoverLanguage());
    }

    public static Lumina.Data.Language GetTextLanguageLumina()
    {
        return GetLuminaLanguage(GetTextLanguage());
    }
    
    public static Lumina.Data.Language GetLuminaLanguage(string lang)
    {
        return LuminaLanguages[lang];
    }

    public static string GetXivLanguage(Lumina.Data.Language lang)
    {
        return AvailableLangauges[(uint)lang - 1];
    }
}
