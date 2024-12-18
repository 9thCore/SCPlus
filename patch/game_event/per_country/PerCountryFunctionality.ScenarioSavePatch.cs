﻿using HarmonyLib;
using SCPlus.plugin;
using System.Collections.Generic;

namespace SCPlus.patch.game_event.per_country
{
    internal partial class PerCountryFunctionality
    {
        [HarmonyPatch(typeof(ScenarioCreatorAPI), nameof(ScenarioCreatorAPI.LocalExport))]
        private static class ScenarioSavePatch
        {
            private static void Prefix(ScenarioCreatorAPI __instance, ref List<GameEvent> __state)
            {
                if (!Config.perCountryEventFunctionality.Value)
                {
                    return;
                }

                __state = [];

                foreach (GameEvent gameEvent in perCountryEvents)
                {
                    if (!__instance.GameEvents.Contains(gameEvent))
                    {
                        continue;
                    }

                    __instance.GameEvents.Remove(gameEvent);
                    __state.Add(gameEvent);

                    ExtraFunctionality.Data data = null;
                    if (Config.expandEventFunctionality.Value)
                    {
                        ExtraFunctionality.eventData.TryGetValue(gameEvent, out data);
                    }

                    foreach (Country country in __instance.Countries)
                    {
                        GameEvent currentEvent = gameEvent.Clone();

                        currentEvent.localCondition ??= new()
                        {
                            parameterConditions = []
                        };

                        currentEvent.localCondition.parameterConditions = currentEvent.localCondition.parameterConditions.AddToArray(new()
                        {
                            target = EReflectionTarget.COUNTRY,
                            fieldName = nameof(Country.id),
                            comparison = ParameterCondition.EComparison.EQUALS,
                            mapOperation = EMapOp.SELF,
                            sumOperation = EMapOp.SELF,
                            val = country.id
                        });

                        if (data != null)
                        {
                            ExtraFunctionality.eventData[currentEvent] = data;
                        }

                        currentEvent.name = GetSubEventName(gameEvent, country);
                        __instance.GameEvents.Add(currentEvent);
                        subEvents.Add(currentEvent);
                    }
                }
            }

            private static void Postfix(ScenarioCreatorAPI __instance, List<GameEvent> __state)
            {
                if (!Config.perCountryEventFunctionality.Value)
                {
                    return;
                }

                foreach (GameEvent gameEvent in subEvents)
                {
                    __instance.GameEvents.Remove(gameEvent);
                }

                foreach (GameEvent gameEvent in __state)
                {
                    __instance.GameEvents.Add(gameEvent);
                }

                subEvents.Clear();
            }
        }
    }
}
