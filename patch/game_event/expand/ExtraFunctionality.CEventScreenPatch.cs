using HarmonyLib;
using System;
using SCPlus.plugin;
using System.Linq;

namespace SCPlus.patch.game_event
{
    internal static partial class ExtraFunctionality
    {
        [HarmonyPatch(typeof(CEventScreen))]
        internal static class CEventScreenPatch
        {
            [HarmonyPatch(nameof(CEventScreen.TreeFromXML))]
            [HarmonyPrefix]
            private static void LoadPatch(GameEvent event_)
            {
                if (!Config.expandEventFunctionality.Value)
                {
                    return;
                }

                if (event_.eventOutcomes.Length == 0)
                {
                    return;
                }

                EventOutcome last = event_.eventOutcomes.Last();
                DiseaseEffect effect = last.diseaseEffect;

                if (effect == null)
                {
                    return;
                }

                Data data = GetDataOrDefault(event_);

                data.evolveRandomTech = effect.evolveRandomTech;
                data.deEvolveRandomTech = effect.deEvolveRandomTech;
                data.randomTech = effect.randomTech;
                data.function = effect.function;
                data.eventLockTech = effect.eventLockTech;

                EventOutcome[] array = event_.eventOutcomes;
                Array.Resize(ref array, event_.eventOutcomes.Length - 1);
                event_.eventOutcomes = array;
            }

            [HarmonyPatch(nameof(CEventScreen.XMLFromTree))]
            [HarmonyPostfix]
            private static void SavePatch(GameEvent event_)
            {
                if (!Config.expandEventFunctionality.Value)
                {
                    return;
                }

                if (!eventData.TryGetValue(event_, out Data data)
                    || data.IsDefault())
                {
                    return;
                }

                DiseaseEffect effect = new()
                {
                    parameterEffects = [],
                    evolveRandomTech = data.evolveRandomTech,
                    deEvolveRandomTech = data.deEvolveRandomTech,
                    randomTech = data.randomTech,
                    function = data.function,
                    eventLockTech = data.eventLockTech
                };

                EventOutcome outcome = new()
                {
                    logicalOp = EventOutcome.ELogicalOp.ALWAYS,
                    diseaseEffect = effect
                };

                event_.eventOutcomes = event_.eventOutcomes.AddToArray(outcome);
            }
        }
    }
}
