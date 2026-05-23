using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Spaketh.Model;
using SpakethPlugin.Model;

namespace SpakethPlugin.Handlers;

public static class Voicelines
{
    private static Dictionary<string, Voiceline> _voicelines = new Dictionary<string, Voiceline>();
    private static List<Playscript> _playscripts = new List<Playscript>();
    private static List<uint> _supportedInstances = new List<uint>() {};
    public static void Init()
    { 
        // get playscripts
        Plugin.Log.Debug("Loading static voicelines");
        string pluginDirectory = Plugin.PluginInterface.AssemblyLocation.DirectoryName ?? "";
        var files = Directory.GetFiles(Path.Combine(pluginDirectory, "Data", "playscripts"), "*.json", SearchOption.AllDirectories);
        Plugin.Log.Debug($"Files found {files.Length}");
        foreach (var file in files)
        {   
            using StreamReader reader = new(file);
            string text = reader.ReadToEnd();
            
            // map them for searching
            var data = JsonConvert.DeserializeObject<Playscript>(text);
            _playscripts.Add(data);
            _supportedInstances.AddRange(data.InstanceIds);
            foreach (var value in data.Lines)
            {
                var key = GameDialogue.GetInterceptLine(value);
                _voicelines[key] = value;
            }
        }
        Plugin.Log.Debug($"Playscripts loaded: {_playscripts.Count} --- Voicelines loaded: {_voicelines.Count}");
    }
    
    public static Playback? ReplaceVoiceline(string original)
    {
        if (_voicelines.TryGetValue(original, out var vl)) // if there is a mapping 
        {
            PlayVoiceline(vl); // play the voiceline
            return new Playback
            {
                Text = vl.Text?.Get() ?? GameDialogue.GetReplacementLine(vl),
                Name = vl.Name?.Get(),
                Duration = vl.Duration,
                Image = vl.ImageId
            }; // return the new text
        }
        return null; // otherwise just return back nothing
    }
    
    public static void PlayVoiceline(Voiceline vl)
    {
        var filename = vl.SoundFile;
        InterfaceManager.SoundManager.PlaySound(filename);
    }

    public static bool IsInSupportedInstance()
    {
        return _supportedInstances.Contains(Plugin.ClientState.TerritoryType);
    }
}
