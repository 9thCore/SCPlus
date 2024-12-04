using HarmonyLib;
using SCPlus.patch.lang.localization;
using SCPlus.plugin;
using System;
using System.Collections.Generic;

namespace SCPlus.patch.lang
{
    internal static class LanguageRegister
    {
        internal static readonly string SCPLUS_PREFIX = "[+]";
        internal static readonly string SCPLUS_TRANSLATION_KEY = "SCPlus";

        internal static HashSet<string> TYPE_MODIFIED_LANGUAGES = [];

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

            if (Config.expandTraitFunctionality.Value)
            {
                LocalizationEnglish.TechExtraFunctionalityTranslations();
            }
            
            if (Config.patchVariableName.Value)
            {
                LocalizationEnglish.PatchedVariableNamesTranslations();
            }

            if (Config.widenVariableAccessibility.Value)
            {
                LocalizationEnglish.WidenedAccessVariableTranslations();
            }

            if (Config.describeVariableType.Value)
            {
                // this actually kinda sucks but whatever
                // avoid all those pesky warnings and such by accessing .Type
                UnityEngine.Debug.unityLogger.logEnabled = false;
                LocalizationEnglish.DescribeVariableTypeTranslations();
                UnityEngine.Debug.unityLogger.logEnabled = true;
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

        internal static void ModifyVariable(string language, string commonSuffix, string namePrefix = "", string nameSuffix = "", string tooltipPrefix = "", string tooltipSuffix = "")
        {
            ModifyLine(language, $"UI_Event_Variable_{commonSuffix}", namePrefix, nameSuffix);
            ModifyLine(language, $"Help_Event_Variable_{commonSuffix}", tooltipPrefix, tooltipSuffix);
        }

        internal static void RegisterVariable(string language, string commonSuffix, string name, string tooltip)
        {
            RegisterLine(language, $"UI_{commonSuffix}", $"{SCPLUS_PREFIX} {name}");
            RegisterLine(language, $"Help_{commonSuffix}", tooltip);
        }

        internal static void RegisterSetter(string language, LocalizationKey key, string internalName, string name, string help)
        {
            RegisterLine(language, GetSetterLocalizationTag(key, internalName), name);
            RegisterLine(language, GetSetterHelpLocalizationTag(key, internalName), help);
        }

        internal static void RegisterTechScreen(string language, LocalizationKey key, string suffix, string overlayTitle, TechScreenListTranslations positive = null, TechScreenListTranslations negative = null)
        {
            RegisterLine(language, GetLocalizationTag(key, $"TechScreen_{suffix}"), overlayTitle);
            if (positive != null)
            {
                RegisterLine(language, GetLocalizationTag(key, $"TechScreen_Title_{suffix}"), positive.title);
                RegisterLine(language, GetLocalizationTag(key, $"TechScreen_Instruction_{suffix}"), positive.instruction);
                RegisterLine(language, GetLocalizationTag(key, $"TechScreen_Help_{suffix}"), positive.help);
            }

            if (negative != null)
            {
                RegisterLine(language, GetLocalizationTag(key, $"TechScreen_Title_Not_{suffix}"), negative.title);
                RegisterLine(language, GetLocalizationTag(key, $"TechScreen_Instruction_Not_{suffix}"), negative.instruction);
                RegisterLine(language, GetLocalizationTag(key, $"TechScreen_Help_Not_{suffix}"), negative.help);
            }
        }

        internal static void ModifyLine(string language, string tag, string prefix = "", string suffix = "", bool silentError = false)
        {
            if (!CLocalisationManager.LanguageExists(language) || !CLocalisationManager.mpLocalisedTexts.ContainsKey(language))
            {
                if (!silentError)
                {
                    Plugin.Logger.LogError($"Invalid language {language}");
                }
                return;
            }

            tag = tag.ToLower();
            if (!CLocalisationManager.mpLocalisedTexts[language].ContainsKey(tag))
            {
                if (!silentError)
                {
                    Plugin.Logger.LogError($"Invalid tag {tag} - cannot modify something that does not exist");
                }
                return;
            }

            CLocalisationManager.mpLocalisedTexts[language][tag] = $"{prefix}{CLocalisationManager.mpLocalisedTexts[language][tag]}{suffix}";
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

        internal static void DefaultDescribeVariableTypes(string language, string suffixFormat = "Type: {0}")
        {
            TYPE_MODIFIED_LANGUAGES.Add(language);

            foreach (EventVariable variable in ScenarioCreatorAPI.Instance.sortedEventVariables)
            {
                if (variable.Type == null)
                {
                    continue;
                }

                string rep = !string.IsNullOrWhiteSpace(variable.outcomeListData) ? variable.outcomeListData :
                    Type.GetTypeCode(variable.Type).ToString();

                ModifyLine(
                    language,
                    variable.tooltip,
                    suffix: $"\n{SCPLUS_PREFIX} {String.Format(suffixFormat, rep)}",
                    silentError: true);
            }
        }

        internal static void FillDescribeVariableTypes(string language)
        {
            if (!Config.describeVariableType.Value)
            {
                return;
            }

            if (TYPE_MODIFIED_LANGUAGES.Contains(language))
            {
                return;
            }

            DefaultDescribeVariableTypes(language);
        }

        [HarmonyPatch(typeof(CLocalisationManager), nameof(CLocalisationManager.InitialiseLanguage))]
        private static class Patch
        {
            private static void Postfix(string languageName)
            {
                UnityEngine.Debug.unityLogger.logEnabled = false;
                FillDescribeVariableTypes(languageName);
                UnityEngine.Debug.unityLogger.logEnabled = true;
            }
        }

        internal enum LocalizationKey
        {
            ExtraFunc,
            PerCountry,
            TechExtraFunc
        }

        internal class TechScreenListTranslations
        {
            internal string title = "";
            internal string instruction = "";
            internal string help = "";
        }
    }
}
