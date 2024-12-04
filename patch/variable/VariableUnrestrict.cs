using HarmonyLib;
using SCPlus.plugin;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCPlus.patch.variable
{
    [HarmonyPatch]
    internal static class VariableUnrestrict
    {
        private static readonly HashSet<Disease.EDiseaseType> ALL_TYPES = 
            new(Enum.GetValues(typeof(Disease.EDiseaseType)).Cast<Disease.EDiseaseType>());

        // Trick editor into considering all disease types as valid for scope
        [HarmonyPatch(typeof(VariableSelectOverlay))]
        [HarmonyPatch(nameof(VariableSelectOverlay.UpdateTree))]
        [HarmonyPostfix]
        internal static void UpdateTreePostfix(VariableSelectOverlay __instance)
        {
            if (!Config.liftVariableTypeCheck.Value)
            {
                return;
            }

            __instance.types = ALL_TYPES;
        }

        // Not a patch, but a method called from Awake()
        // Plugin runs after main game code,
        // when variables are already registered,
        // so this can't really be patching anything relevant
        internal static void RemoveDiseaseTypes()
        {
            if (!Config.liftVariableTypeCheck.Value)
            {
                return;
            }

            foreach (EventVariable variable in ScenarioCreatorAPI.Instance.sortedEventVariables)
            {
                variable.diseaseType = ""; // Just clear it out
            }
        }

        internal static void WidenAccessibility()
        {
            if (!Config.widenVariableAccessibility.Value)
            {
                return;
            }

            VariableHelpers.WidenAccess(
                "localZCombatStrength",
                VariableHelpers.File.LOCALDISEASE,
                VariableHelpers.AccessModifier.CONDITION);

            VariableHelpers.WidenAccess(
                "localHCombatStrength",
                VariableHelpers.File.COUNTRY,
                VariableHelpers.AccessModifier.CONDITION);

            VariableHelpers.WidenAccess(
                "cureRequirements",
                VariableHelpers.File.DISEASE,
                VariableHelpers.AccessModifier.OUTCOME | VariableHelpers.AccessModifier.EXPRESSION);

            VariableHelpers.WidenAccess(
                "cureFlag",
                VariableHelpers.File.DISEASE,
                VariableHelpers.AccessModifier.OUTCOME);
        }
    }
}
