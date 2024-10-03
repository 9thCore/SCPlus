using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCPlus.patch.variable
{
    [HarmonyPatch]
    internal class VariableUnrestrict
    {
        private static readonly HashSet<Disease.EDiseaseType> ALL_TYPES = 
            new(Enum.GetValues(typeof(Disease.EDiseaseType)).Cast<Disease.EDiseaseType>());

        // Trick editor into considering all disease types as valid for scope
        [HarmonyPatch(typeof(VariableSelectOverlay))]
        [HarmonyPatch(nameof(VariableSelectOverlay.UpdateTree))]
        [HarmonyPostfix]
        internal static void UpdateTreePostfix(VariableSelectOverlay __instance)
        {
            __instance.types = ALL_TYPES;
        }

        // Not a patch, but a method called from Awake()
        // Plugin runs after main game code,
        // when variables are already registered,
        // so this can't really be patching anything relevant
        internal static void RemoveDiseaseTypes()
        {
            foreach (EventVariable variable in ScenarioCreatorAPI.Instance.sortedEventVariables)
            {
                variable.diseaseType = ""; // Just clear it out
            }
        }
    }
}
