﻿using HarmonyLib;
using SCPlus.plugin;
using System;
using System.Linq;

namespace SCPlus.patch.game_event
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

            ExtraFunctionality.Data data = ExtraFunctionality.GetDataOrDefault(event_);

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

            if (!ExtraFunctionality.eventData.TryGetValue(event_, out ExtraFunctionality.Data data))
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