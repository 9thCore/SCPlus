﻿using SCPlus.patch.game;
using SCPlus.patch.lang.localization;
using SCPlus.plugin;
using System;

namespace SCPlus.patch.lang
{
    internal static class LanguageRegister
    {
        internal static readonly string SCPLUS_PREFIX = "[+]";
        internal static readonly string SCPLUS_TRANSLATION_KEY = "SCPlus";

        internal static void Awake()
        {
            if (Config.exposeMoreVariables.Value)
            {
                LocalizationEnglish.MoreVariableTranslations();
            }

            if (Config.expandEventFunctionality.Value)
            {
                LocalizationEnglish.ExtraFunctionalityTranslations();
            }

            if (Config.perCountryEventFunctionality.Value)
            {
                LocalizationEnglish.PerCountryTranslations();
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
            RegisterVariable(language, $"{SCPLUS_TRANSLATION_KEY}_Event_Variable_{commonSuffix}", name, tooltip);
        }

        internal static void RegisterSCPlusLine(string language, string suffix, string line)
        {
            RegisterLine(language, $"{SCPLUS_TRANSLATION_KEY}_{suffix}", line);
        }

        internal static void RegisterVariable(string language, string commonSuffix, string name, string tooltip)
        {
            RegisterLine(language, $"UI_{commonSuffix}", $"{SCPLUS_PREFIX} {name}");
            RegisterLine(language, $"Help_{commonSuffix}", tooltip);
        }

        internal static void RegisterEventSetter(string language, LanguageRegister.LocalizationKey key, string internalName, string name, string help)
        {
            RegisterLine(language, GetSetterLocalizationTag(key, internalName), name);
            RegisterLine(language, GetSetterHelpLocalizationTag(key, internalName), help);
        }

        internal static void RegisterLine(string language, string tag, string line)
        {
            if (!CLocalisationManager.LanguageExists(language) || !CLocalisationManager.mpLocalisedTexts.ContainsKey(language))
            {
                Plugin.Logger.LogError($"Invalid language {language}");
                return;
            }

            CLocalisationManager.mpLocalisedTexts[language].Add(tag, line);
            CLocalisationManager.mpLocalisedTexts[language].Add(tag.ToLower(), line);
        }

        internal static string GetSetterHelpLocalizationTag(LocalizationKey key, string suffix)
        {
            return GetSetterLocalizationTag(key, $"{suffix}_Help");
        }

        internal static string GetSetterLocalizationTag(LocalizationKey key, string suffix)
        {
            return GetLocalizationTag(key, $"{suffix}_Setter");
        }

        internal static string GetLocalizationTag(LocalizationKey key, string suffix)
        {
            return $"{SCPLUS_TRANSLATION_KEY}_{key}_{suffix}";
        }

        internal enum LocalizationKey
        {
            ExtraFunc,
            PerCountry
        }
    }
}
