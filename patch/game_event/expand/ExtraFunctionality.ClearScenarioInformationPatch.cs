using HarmonyLib;
using SCPlus.plugin;

namespace SCPlus.patch.game_event
{
    internal static partial class ExtraFunctionality
    {
        [HarmonyPatch(typeof(ScenarioCreatorAPI), nameof(ScenarioCreatorAPI.ClearScenarioInformation))]
        class ClearScenarioInformationPatch
        {
            private static void Postfix()
            {
                if (!Config.expandEventFunctionality.Value)
                {
                    return;
                }

                eventData.Clear();
            }
        }
    }
}
