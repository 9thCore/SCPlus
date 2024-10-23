using HarmonyLib;
using SCPlus.plugin;

namespace SCPlus.patch.game_event.per_country
{
    internal partial class PerCountryFunctionality
    {
        [HarmonyPatch(typeof(ScenarioCreatorAPI), nameof(ScenarioCreatorAPI.ClearScenarioInformation))]
        private static class ClearScenarioInformationPatch
        {
            private static void Postfix()
            {
                if (!Config.perCountryEventFunctionality.Value)
                {
                    return;
                }

                perCountryEvents.Clear();
                subEvents.Clear();
                creatingPerCountryEvent = false;
            }
        }
    }
}
