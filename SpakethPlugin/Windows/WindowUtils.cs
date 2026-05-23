using System;
using System.Net.Mime;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using SpakethPlugin.Handlers;
using ImGui = Dalamud.Bindings.ImGui.ImGui;

namespace SpakethPlugin.Windows;

public static class WindowUtils
{

    public const int ButtonSizeX = 85;
    public const int ButtonSizeY = 35;
    public static Vector2 ButtonSize = new Vector2(ButtonSizeX, ButtonSizeY);
    
    public static void DrawColoredWord(string word, Vector4 color)
    {
        ImGui.PushStyleColor(ImGuiCol.Text, color);
        ImGui.Text(word);
        ImGui.PopStyleColor();
    }

    public static void Separator()
    {
        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();
    }

    public static void SameLineWithSpace()
    {
        ImGui.SameLine();
        ImGui.Spacing();
        ImGui.SameLine();
    }
    
    public static void TextCentered(string text) {
        var windowWidth = ImGui.GetWindowSize().X;
        var textWidth   = ImGui.CalcTextSize(text).X;

        ImGui.SetCursorPosX((windowWidth - textWidth) * 0.5f);
        ImGui.Text(text);
    }

    public static Action ClosePopupAction()
    {
        return () => ImGui.CloseCurrentPopup();
    }

    public static string ConfirmationDialogue(
        string label, string message, string affirm, string neg, Action affirmAction, Action negAction)
    {
        var id = $"##PopUp{label}";
        if (ImGui.BeginPopup(id, ImGuiWindowFlags.NoResize | ImGuiWindowFlags.Popup))
        {
            TextCentered(message);
            ImGui.Spacing();
            
            
            var windowWidth = ImGui.GetWindowSize().X;
            ImGui.SetCursorPosX(windowWidth * 0.5f - ButtonSizeX - 5);
            if (ImGui.Button(affirm, ButtonSize))
            {
                affirmAction.Invoke();
                ClosePopupAction().Invoke();
            }
            ImGui.SameLine();
            ImGui.SetCursorPosX(windowWidth * 0.5f + 5);
            if (ImGui.Button(neg, ButtonSize))
            {
                negAction.Invoke();
            }
            ImGui.EndPopup();
        }

        return id;
    }
}
