using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Client.UI;
using Lumina.Excel.Sheets;
using Penumbra.Api.Api;
using SpakethPlugin.Handlers;

namespace SpakethPlugin.Windows;

public class MainWindow : Window, IDisposable
{
    private readonly Plugin plugin;
    
    public MainWindow(Plugin plugin)
        : base("Spaketh##Main", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        this.plugin = plugin;
    }

    public void Dispose() { }

    public override void Draw()
    {
        var enabled = plugin.Configuration.IsEnabled;
        if (ImGui.Checkbox("Spaketh Enabled", ref enabled))
        {
            plugin.Configuration.IsEnabled = enabled;
            plugin.SetGameHooks();
        }
        
        ImGui.Text($"Current client language is {Plugin.ClientState.ClientLanguage}");
        ImGui.Text($"Current voice language is {Language.GetVoiceoverLanguageLumina()}");
        var penumbraState = ModManager.IsPenumbraAvailable() ? "Available" : "Unavailable";
        ImGui.Text($"Penumbra is {penumbraState}");
        
        WindowUtils.Separator();
        
        if (!plugin.Configuration.IsEnabled)
            ImGui.BeginDisabled();
                
        if (ImGui.Button("Test Existing Voiceover"))
        {
            Plugin.TestMode = true;
            BattleTalk.ShowBattleTalkImage("Not Alex", GameDialogue.GetBattletext(28), 5);
        }

        WindowUtils.SameLineWithSpace();


        var penumbraIsAvailable = ModManager.IsPenumbraAvailable();
        if (!penumbraIsAvailable)
            ImGui.BeginDisabled();
            
        if (!TestingExample.CheckExampleMod())
        {
            if (!TestingExample.Installing)
            {
                DrawExampleInstall();
            }
            else
            {
                ImGui.BeginDisabled();
                ImGui.Button("Installing...");
                ImGui.EndDisabled();
            }
        }
        else
        {
            if (ImGui.Button("Test Example Voiceover"))
            {
                Plugin.TestMode = true;
                BattleTalk.ShowBattleTalkImage("Not Frank", GameDialogue.GetBattletext(6), 5);
            }
        }
        
        if (!penumbraIsAvailable)
            ImGui.EndDisabled();
        
        
        if (!plugin.Configuration.IsEnabled)
            ImGui.EndDisabled();
        
        WindowUtils.Separator();
        
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

    public void DrawExampleInstall()
    {
        var installModAction = TestingExample.EnsureExampleMod;
        var installMessage = "Spaketh needs to install a Penumbra mod\nto enable the test custom voiceover. Install?";
        var popupId = WindowUtils.ConfirmationDialogue("TestingExampleInstall", installMessage, "Yes", "No", installModAction, WindowUtils.ClosePopupAction());
        if (ImGui.Button("Install Example Voiceover?"))
        {
            ImGui.OpenPopup(popupId);
        }
    }
}
