using System.Collections.Generic;
using System.IO;
using Dalamud.Bindings.ImGui;
using Lumina.Excel.Sheets;
using Newtonsoft.Json;
using Spaketh.Model;

namespace Spaketh.Handlers;

public static class Voicelines
{
    private static Dictionary<string, Voiceline> voicelines;
    private static Dictionary<string, (uint, string?)>? mappings;
    static Voicelines()
    {   
        // TODO regenerate whenever the language changes
        // get declared voice lines from files
        string pluginDirectory = Plugin.PluginInterface.AssemblyLocation.DirectoryName;
        string filePath = Path.Combine(pluginDirectory, "Data", "source_npc.json");
        using StreamReader reader = new(filePath);
        string text = reader.ReadToEnd();
        
        // get the voiceline text from the game tables
        var lang = Language.GetLuminaLanguage();
        var textDataSheet = InterfaceManager.DataManager.Excel.GetSheet<InstanceContentTextData>(
            lang, "InstanceContentTextData");

        // map them into a Dict for searching
        voicelines = new Dictionary<string, Voiceline>();
        var json = JsonConvert.DeserializeObject<Voiceline[]>(text);
        foreach (var value in json )
        {
            value.Text = textDataSheet[value.Transcript].Text.ExtractText();
            voicelines.Add(value.Transcript.ToString(), value);
        }
        
        // get the replacement mappings from the plugin file
        string filePath1 = Path.Combine(pluginDirectory, "Data", "mappings.json");
        using StreamReader reader2 = new(filePath1);
        text = reader2.ReadToEnd();

        var rawMappings = JsonConvert.DeserializeObject<Mapping[]>(text);
        var richMap = new Dictionary<string, (uint, string?)>();
        foreach (var value in rawMappings)
        {
            var originalText = textDataSheet[value.Original].Text.ExtractText();
            richMap[originalText] = (value.Replacement, value.Name);
        }

        mappings = richMap;
    }
    
    public static (string?, uint, string?) ReplaceVoiceline(string original)
    {
        if (mappings.TryGetValue(original, out var rep)) // if there is a mapping 
        {
            var voiceline = voicelines[rep.Item1.ToString()];
            PlayVoiceline(rep.Item1); // play the voiceline
            return (voiceline.Text, voiceline.Duration, rep.Item2); // return the new text
        }
        return (null, 0, ""); // otherwise just return back nothing
    }
    
    public static void PlayVoiceline(uint voicelineId)
    {
        if (voicelines.ContainsKey(voicelineId.ToString()))
        {
            var filename = voicelines[voicelineId.ToString()].File;
            Plugin.Log.Info($"Playing sound file {filename}");
            InterfaceManager.SoundManager.PlaySound(voicelines[voicelineId.ToString()].File);
        }
    }
    
    public static (string, uint) GetVoicelineText(uint voicelineId)
    {
        var vl = voicelines[voicelineId.ToString()];
        return (vl.Text, vl.Duration);
    }
}
