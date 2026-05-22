using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Client.UI;
using Lumina.Excel.Sheets;
using SpakethPlugin.Handlers;

namespace SpakethPlugin.Windows;

public class MainWindow : Window, IDisposable
{
    private readonly Plugin plugin;
    private GameHook gameHook;

    private uint CurrentTerritory = 0;

    private int imageOutMode = 0;

    // We give this window a hidden ID using ##.
    // The user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(Plugin plugin, GameHook gameHook)
        : base("Spaketh##Main", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        this.plugin = plugin;
        this.gameHook = gameHook;
    }

    public void Dispose() { }

    public override void Draw()
    {
        // if (CurrentTerritory != Plugin.ClientState.TerritoryType) 
        // {
        //     if (GameHook.ActiveInstances.Contains(CurrentTerritory)) // if we are switching out of a supported instance
        //     {
        //         Plugin.Log.Info($"Leaving supported instance {CurrentTerritory}");
        //         gameHook.StopHooks();
        //
        //     }
        //     CurrentTerritory = Plugin.ClientState.TerritoryType;
        //     if (GameHook.ActiveInstances.Contains(CurrentTerritory)) // if we are entering a supported instance
        //     {
        //         Plugin.Log.Info($"Entering supported instance {CurrentTerritory}");
        //         gameHook.StartHooks();
        //     }
        //     
        // }
        var enabled = plugin.Configuration.IsEnabled;
        if (ImGui.Checkbox("Spaketh Enabled", ref enabled))
        {
            plugin.Configuration.IsEnabled = enabled;
            plugin.SetGameHooks();
        }
        
        ImGui.Text($"Current client language is {Plugin.ClientState.ClientLanguage}");
        ImGui.Text($"Current voice language is {Language.GetVoiceoverLanguageLumina()}");
        
        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();
        
        if (!plugin.Configuration.IsEnabled)
            ImGui.BeginDisabled();
                
        if (ImGui.Button("Test Existing Voiceover"))
        {
            Plugin.TestMode = true;
            BattleTalk.ShowBattleTalkImage("Not Alex", GameDialogue.GetBattletext(28), 5);
        }
        ImGui.SameLine();
        ImGui.Spacing();
        ImGui.SameLine();
        if (ImGui.Button("Test Custom Voiceover"))
        {
            Plugin.TestMode = true;
            BattleTalk.ShowBattleTalkImage("Not Frank", GameDialogue.GetBattletext(6), 5);
        }
        
        if (!plugin.Configuration.IsEnabled)
            ImGui.EndDisabled();
        
        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();
        
        var territoryId = Plugin.ClientState.TerritoryType;
        if (Plugin.DataManager.GetExcelSheet<TerritoryType>().TryGetRow(territoryId, out var territoryRow))
        {
            ImGui.Text($"Current location:");
            ImGui.SameLine();
            ImGui.Text(territoryRow.PlaceName.Value.Name.ToString() + " --- " + territoryId);
            ImGui.Text("Plugin is");
            ImGui.SameLine();
            if (!plugin.Configuration.IsEnabled)
            {
                WindowUtils.DrawColoredWord("DISABLED", Colors.Purple);
            } 
            else if (Voicelines.IsInSupportedInstance())
            {
                WindowUtils.DrawColoredWord("ACTIVE", Colors.Green);
            }
            else
            {
                WindowUtils.DrawColoredWord("INACTIVE", Colors.Red);
            }
        }
        
    }
}
