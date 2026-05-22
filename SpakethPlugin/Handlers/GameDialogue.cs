using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Lumina.Excel;
using Lumina.Excel.Sheets;
using Spaketh.Model;

namespace SpakethPlugin.Handlers;

public static class GameDialogue
{
    
    private static Dictionary<string, ExcelSheet<InstanceContentTextData>> _battletext = new Dictionary<string, ExcelSheet<InstanceContentTextData>>();
    // TODO textbox dialogue

    public static string GetInterceptLine(Voiceline vl)
    {
        var lang = Language.GetTextLanguage();
        if (vl.Intercept.BattletextId != null)
        {
            return GetBattletext(vl.Intercept.BattletextId.Value, lang);
        }

         
        return "";
    }

    public static string GetReplacementLine(Voiceline vl)
    {
        var lang = Language.GetTextLanguage();
        if (vl.GameText?.BattletextId != null)
        {
            return GetBattletext(vl.GameText.BattletextId.Value, lang);
        }

        return "";
    }

    public static string GetBattletext(uint key)
    {
        return GetBattletext(key, Language.GetTextLanguage());
    }
    
    private static string GetBattletext(uint key, string lang)
    {
        if (!_battletext.ContainsKey(lang))
            InitBattletext(lang);
        return _battletext[lang][key].Text.ExtractText();
    }

    private static void InitBattletext(string lang)
    {
        _battletext[lang] = InterfaceManager.DataManager.Excel.GetSheet<InstanceContentTextData>(
            Language.GetLuminaLanguage(lang), "InstanceContentTextData");
    }
}
