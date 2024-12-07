using HarmonyLib;
using SCPlus.patch.lang;
using SCPlus.plugin;

namespace SCPlus.patch.game_event.patch_name
{
    internal static class VariableNamePatcher
    {
        internal static void Register()
        {
            SetEventVariableTranslation("wealthy", "COUNTRY");
            SetEventVariableTranslation("poverty", "COUNTRY");
            SetEventVariableTranslation("urban", "COUNTRY");
            SetEventVariableTranslation("rural", "COUNTRY");
            SetEventVariableTranslation("hot", "COUNTRY");
            SetEventVariableTranslation("cold", "COUNTRY");
            SetEventVariableTranslation("humid", "COUNTRY");
            SetEventVariableTranslation("arid", "COUNTRY");
        }

        private static void SetEventVariableTranslation(string name, string scope)
        {
            SetTranslations(name, scope, $"UI_Event_Variable_{name}_country", $"Help_Event_Variable_{name}_country");
        }

        private static void SetTranslations(string name, string scope, string title, string tooltip)
        {
            EventVariable variable = ScenarioCreatorAPI.Instance.GetEventVariable(name, scope);
            if (variable == null)
            {
                return;
            }

            variable.variableString = title;
            variable.tooltip = tooltip;
        }

        [HarmonyPatch(typeof(CEventScreen), nameof(CEventScreen.Initialise))]
        private static class Patch
        {
            private static void Postfix()
            {
                if (!Config.patchVariableName.Value)
                {
                    return;
                }

                Register();
                LanguageRegister.InitSuffixes();
            }
        }
    }
}
