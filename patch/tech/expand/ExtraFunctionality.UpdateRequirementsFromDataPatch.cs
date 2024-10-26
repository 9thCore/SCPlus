using HarmonyLib;
using SCPlus.plugin;

namespace SCPlus.patch.tech.expand
{
    internal partial class ExtraFunctionality
    {
        // Disallow base code if we're replacing it anyway
        [HarmonyPatch(typeof(TechManagementScreen), nameof(TechManagementScreen.UpdateRequirementsFromData))]
        private static class UpdateRequirementsFromDataPatch
        {
            private static bool Prefix()
            {
                return !Config.replaceTraitRequirement.Value;
            }
        }
    }
}
