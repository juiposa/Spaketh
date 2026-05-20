using Lumina.Excel.Sheets;

namespace Spaketh.Handlers;

public class Npc
{
    public static string GetNPCName(uint npcId)
    {
        var lang = Language.GetLuminaLanguage();
        var bnpcNames = InterfaceManager.DataManager.Excel.GetSheet<BNpcName>(
            lang, "BNpcName");
        return bnpcNames[npcId].Singular.ExtractText();
    }
}
