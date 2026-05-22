using System.Numerics;
using Dalamud.Bindings.ImGui;

namespace SpakethPlugin.Windows;

public static class WindowUtils
{
    public static void DrawColoredWord(string word, Vector4 color)
    {
        ImGui.PushStyleColor(ImGuiCol.Text, color);
        ImGui.Text(word);
        ImGui.PopStyleColor();
    }
}
