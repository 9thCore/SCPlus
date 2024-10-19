using HarmonyLib;
using SCPlus.patch.game_event;
using SCPlus.plugin;

namespace SCPlus.patch.game
{
    [HarmonyPatch(typeof(ScenarioCreatorAPI), nameof(ScenarioCreatorAPI.ClearScenarioInformation))]
    class ScenarioCreatorAPIPatch
    {
        private static void Postfix()
        {
            if (!Config.expandEventFunctionality.Value)
            {
                return;
            }

            ExtraFunctionality.eventData.Clear();
        }
    }
}
