using HarmonyLib;
using SCPlus.plugin;

namespace SCPlus.patch.game_event
{
    [HarmonyPatch(typeof(VariableSetter))]
    internal class VariableSetterPatch
    {
        // There are too many subclasses I should extend to redirect this "normally",
        // so patching imma do
        [HarmonyPatch(nameof(VariableSetter.Initialise))]
        [HarmonyPrefix]
        private static void AccessValuePatch(VariableSetter __instance, ref object to)
        {
            if (!Config.expandEventFunctionality.Value)
            {
                return;
            }

            VariableSetterRedirector redirector = __instance.GetComponent<VariableSetterRedirector>();
            if (redirector != null)
            {
                to = redirector.Apply(to);
            }
        }
    }
}
