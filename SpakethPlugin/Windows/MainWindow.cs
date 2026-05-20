using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text.Json.Nodes;
using Dalamud.Bindings.ImGui;
using Dalamud.Game.Chat;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Client.UI;
using Lumina.Excel.Sheets;
using Newtonsoft.Json;
using SamplePlugin;
using Spaketh.Handlers;
using Spaketh.Model;

namespace Spaketh.Windows;

public class MainWindow : Window, IDisposable
{
    private readonly Plugin plugin;
    private GameHook gameHook;

    private uint CurrentTerritory = 0;

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
        
        
        
        ImGui.Text($"Current language is {Language.GetClientLanguage()}");
        
        if (ImGui.Button("Test"))
        {
            BattleTalk.ShowBattleTalk("Bug With a HUUUUUUGE Ass", 27, 5);
        }
        ImGui.Spacing();
        
        var territoryId = Plugin.ClientState.TerritoryType;
        if (Plugin.DataManager.GetExcelSheet<TerritoryType>().TryGetRow(territoryId, out var territoryRow))
        {
            ImGui.Text($"Current location:");
            ImGui.SameLine();
            ImGui.Text(territoryRow.PlaceName.Value.Name.ToString() + " --- " + territoryId);
            ImGui.Text("Plugin is");
            ImGui.SameLine();
            if (GameHook.ActiveInstances.Contains(territoryId))
            {
                ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(0.3f, 0.9f, 0.4f, 1.0f));
                ImGui.Text("ACTIVE");
                ImGui.PopStyleColor();
            }
            else
            {
                ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(1.0f, 0.35f, 0.35f, 1.0f));
                ImGui.Text("INACTIVE");
                ImGui.PopStyleColor();
            }
            
        }
        
    }
}
