using HarmonyLib;
using SCPlus.patch.variable;
using SCPlus.plugin;
using System;
using System.Collections.Generic;
using System.Reflection;
using SCPlus.patch.lang.localization;

#if (USE_32_COMPAT)
#else
using System.Linq;
#endif

namespace SCPlus.patch.lang
{
    internal static class LanguageRegister
    {
        internal static readonly string SCPLUS_PREFIX = "[+]";
        internal static readonly string SCPLUS_TRANSLATION_KEY = "SCPlus";

        internal static HashSet<string> WIDEN_ACCESS_MODIFIED_LANGUAGES = [];
        internal static HashSet<string> TYPE_MODIFIED_LANGUAGES = [];

        internal static Dictionary<string, Handler> LANGUAGE_HANDLERS = [];

        internal static void Init(string language)
        {
            if (!LANGUAGE_HANDLERS.ContainsKey(language))
            {
                Plugin.LogWarning($"Could not find localisations for {language}, skipping initialisation");
                return;
            }

            LocalizationHandlerAttribute attribute = LANGUAGE_HANDLERS[language].Attribute;
            Type localisator = LANGUAGE_HANDLERS[language].Type;

            if (Config.exposeMoreVariables.Value)
            {
                RunMethod(localisator, attribute.moreVariables);
            }

            if (Config.expandEventFunctionality.Value)
            {
                RunMethod(localisator, attribute.extraFunctionality);
            }

            if (Config.perCountryEventFunctionality.Value)
            {
                RunMethod(localisator, attribute.perCountry);
            }

            if (Config.expandTraitFunctionality.Value)
            {
                RunMethod(localisator, attribute.techExtraFunctionality);
            }

            if (Config.patchVariableName.Value)
            {
                RunMethod(localisator, attribute.patchedVariables);
            }
        }
        
        internal static void Awake()
        {
            var handlers = new Type[] { typeof(LocalizationEnglish) };

            foreach (Type type in handlers)
            {

                LocalizationHandlerAttribute attribute = Attribute.GetCustomAttribute(type, typeof(LocalizationHandlerAttribute))
                    as LocalizationHandlerAttribute;

                if (LANGUAGE_HANDLERS.ContainsKey(attribute.language))
                {
                    Plugin.LogError($"Handler with language {attribute.language} already exists, skipping {type.Name}");
                    continue;
                }

                LANGUAGE_HANDLERS[attribute.language] = new Handler(attribute, type);
            }
        }

        internal static void RunMethod(Type type, string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                Plugin.LogError($"Invalid method name in {type.Name}, must be both non-null and non-empty");
                return;
            }

            MethodInfo info = type.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static);
            if (info == null)
            {
                Plugin.LogError($"Could not find {name} in {type.Name}; did not apply translation");
                return;
            }

            info.Invoke(null, []);
        }

        internal static void InitSuffixes()
        {
            foreach (Handler pair in LANGUAGE_HANDLERS.Values)
            {
                LocalizationHandlerAttribute attribute = pair.Attribute;
                Type localisator = pair.Type;

                if (Config.widenVariableAccessibility.Value)
                {
                    RunMethod(localisator, attribute.widenedAccess);
                }

                if (Config.describeVariableType.Value)
                {
                    // this actually kinda sucks but whatever
                    // avoid all those pesky warnings and such by accessing .Type
#if (USE_32_COMPAT)
                    UnityEngine.Debug.logger.logEnabled = false;
#else
                    UnityEngine.Debug.unityLogger.logEnabled = false;
#endif
                    RunMethod(localisator, attribute.describeVariableType);
#if (USE_32_COMPAT)
                    UnityEngine.Debug.logger.logEnabled = true;
#else
                    UnityEngine.Debug.unityLogger.logEnabled = true;
#endif
                }
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
                    Plugin.LogError($"Invalid language {language}");
                }
                return;
            }

            tag = tag.ToLower();
            if (!CLocalisationManager.mpLocalisedTexts[language].ContainsKey(tag))
            {
                if (!silentError)
                {
                    Plugin.LogError($"Invalid tag {tag} - cannot modify something that does not exist");
                }
                return;
            }

            CLocalisationManager.mpLocalisedTexts[language][tag] = $"{prefix}{CLocalisationManager.mpLocalisedTexts[language][tag]}{suffix}";
        }

        internal static void RegisterLine(string language, string tag, string line)
        {
            if (!CLocalisationManager.LanguageExists(language) || !CLocalisationManager.mpLocalisedTexts.ContainsKey(language))
            {
                Plugin.LogError($"Invalid language {language}");
                return;
            }

            CLocalisationManager.mpLocalisedTexts[language][tag] = line;
            CLocalisationManager.mpLocalisedTexts[language][tag.ToLower()] = line;
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

        internal static void DefaultWidenAccess(string language, string suffix = "Widened access")
        {
            WIDEN_ACCESS_MODIFIED_LANGUAGES.Add(language);

            foreach (EventVariable variable in VariableHelpers.widenedAccessVariables)
            {
                LanguageRegister.ModifyLine(
                    language,
                    variable.tooltip,
                    suffix: $"\n{LanguageRegister.SCPLUS_PREFIX} {suffix}");
            }
        }

        internal static void FillWidenAccess(string language)
        {
            if (!Config.widenVariableAccessibility.Value)
            {
                return;
            }

            if (WIDEN_ACCESS_MODIFIED_LANGUAGES.Contains(language))
            {
                return;
            }

            DefaultWidenAccess(language);
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

                string rep = !string.IsNullOrEmpty(variable.outcomeListData) ? variable.outcomeListData :
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

#if (USE_32_COMPAT)
                    UnityEngine.Debug.logger.logEnabled = false;
#else
            UnityEngine.Debug.unityLogger.logEnabled = false;
#endif
            DefaultDescribeVariableTypes(language);
#if (USE_32_COMPAT)
                    UnityEngine.Debug.logger.logEnabled = true;
#else
            UnityEngine.Debug.unityLogger.logEnabled = true;
#endif
        }

        [HarmonyPatch(typeof(CLocalisationManager), nameof(CLocalisationManager.InitialiseLanguage))]
        private static class Patch
        {
            private static void Postfix(string languageName)
            {
                Init(languageName);
                // i dont remember what these do ??
                // FillWidenAccess(languageName);
                // FillDescribeVariableTypes(languageName);
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

        internal record struct Handler(LocalizationHandlerAttribute Attribute, Type Type);
    }
}
