using SCPlus.patch.lang.localization;
using SCPlus.plugin;
using System;

namespace SCPlus.patch.lang
{
    internal class LanguageRegister
    {
        internal static readonly string SCPLUS_PREFIX = "[+]";

        internal static void Awake()
        {
            if (Config.exposeMoreVariables.Value)
            {
                LocalizationEnglish.MoreVariableTranslations();
            }
        }

        internal static void RegisterSCPlusVariableFromExisting(string language, string commonSuffix, string existingVariable = null, string namePrefix = "", string nameSuffix = "", string tooltipPrefix = "", string tooltipSuffix = "")
        {
            RegisterSCPlusVariable(
                language,
                commonSuffix,
                $"{namePrefix}{CLocalisationManager.GetText($"UI_Event_Variable_{existingVariable ?? commonSuffix}")}{nameSuffix}",
                $"{tooltipPrefix}{CLocalisationManager.GetText($"Help_Event_Variable_{existingVariable ?? commonSuffix}")}{tooltipSuffix}");
        }

        internal static void RegisterSCPlusVariable(string language, string commonSuffix, string name, string tooltip)
        {
            RegisterVariable(language, $"SCPlus_Event_Variable_{commonSuffix}", name, tooltip);
        }

        internal static void RegisterSCPlusLine(string language, string suffix, string line)
        {
            RegisterLine(language, $"SCPlus_{suffix}", line);
        }

        internal static void RegisterVariable(string language, string commonSuffix, string name, string tooltip)
        {
            RegisterLine(language, $"UI_{commonSuffix}", $"{SCPLUS_PREFIX} {name}");
            RegisterLine(language, $"Help_{commonSuffix}", tooltip);
        }

        internal static void RegisterLine(string language, string tag, string line)
        {
            if (!CLocalisationManager.LanguageExists(language) || !CLocalisationManager.mpLocalisedTexts.ContainsKey(language))
            {
                throw new Exception($"Invalid language {language}");
            }

            CLocalisationManager.mpLocalisedTexts[language].Add(tag, line);
            CLocalisationManager.mpLocalisedTexts[language].Add(tag.ToLower(), line);
        }
    }
}
