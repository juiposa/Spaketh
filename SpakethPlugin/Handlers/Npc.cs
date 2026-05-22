using Lumina.Excel.Sheets;

namespace SpakethPlugin.Handlers;

public class Npc
{
    public static string GetNpcName(uint npcId)
    {
        var lang = Language.GetTextLanguageLumina();
        var bnpcNames = InterfaceManager.DataManager.Excel.GetSheet<BNpcName>(
            lang, "BNpcName");
        return bnpcNames[npcId].Singular.ExtractText();
    }
}
