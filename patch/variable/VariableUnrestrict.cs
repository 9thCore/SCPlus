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

        // Return a set of all available disease types whenever code asks for available diseases
        [HarmonyPatch(typeof(ScenarioInformation))]
        [HarmonyPatch(nameof(ScenarioInformation.GetDiseaseTypes))]
        [HarmonyPrefix]
        internal static bool GetDiseaseTypesPatch(ref HashSet<Disease.EDiseaseType> __result)
        {
            __result = ALL_TYPES;
            return false;
        }
    }
}
