using HarmonyLib;
using SCPlus.plugin;

namespace SCPlus.patch.game_event.per_country
{
    internal partial class PerCountryFunctionality
    {
        [HarmonyPatch(typeof(CEventScreen), nameof(CEventScreen.CurrentEventIsLocal))]
        private static class CurrentEventIsLocalPatch
        {
            private static void Postfix(CEventScreen __instance, ref bool __result)
            {
                if (!Config.perCountryEventFunctionality.Value)
                {
                    return;
                }

                __result = __result || perCountryEvents.Contains(__instance.currentEvent);
            }
        }
    }
}
