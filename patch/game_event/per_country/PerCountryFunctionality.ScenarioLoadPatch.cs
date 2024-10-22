using HarmonyLib;
using SCPlus.plugin;
using System.Collections.Generic;
using System.Linq;

namespace SCPlus.patch.game_event.per_country
{
    internal partial class PerCountryFunctionality
    {
        [HarmonyPatch(typeof(ScenarioCreatorAPI), nameof(ScenarioCreatorAPI.LocalImport))]
        private static class ScenarioLoadPatch
        {
            private static void Postfix(ScenarioCreatorAPI __instance)
            {
                if (!Config.perCountryEventFunctionality.Value)
                {
                    return;
                }

                List<GameEvent> subEventList = [];
                Dictionary<string, GameEvent> perCountryEventsWithTemplate = [];

                foreach (GameEvent gameEvent in __instance.GameEvents)
                {
                    if (IsSubEvent(gameEvent))
                    {
                        subEventList.Add(gameEvent);

                        if (!perCountryEventsWithTemplate.ContainsKey(GetSubEventRealName(gameEvent))) 
                        {
                            perCountryEventsWithTemplate.Add(GetSubEventRealName(gameEvent), gameEvent);
                        }
                    }
                }

                foreach (GameEvent gameEvent in subEventList)
                {
                    __instance.GameEvents.Remove(gameEvent);
                }

                foreach (KeyValuePair<string, GameEvent> pair in perCountryEventsWithTemplate)
                {
                    string name = pair.Key;
                    GameEvent template = pair.Value;

                    GameEvent superEvent = template.Clone();
                    superEvent.name = GetSubEventRealName(template);

                    if (superEvent.localCondition != null
                        && superEvent.localCondition.parameterConditions != null
                        && superEvent.localCondition.parameterConditions.Length > 0)
                    {
                        ParameterCondition condition = superEvent.localCondition.parameterConditions.Last();
                        if (GetSubEventCountry(superEvent) != null)
                        {
                            superEvent.localCondition.parameterConditions = superEvent.localCondition.parameterConditions.Except([condition]).ToArray();
                        }
                    }

                    perCountryEvents.Add(superEvent);
                    __instance.GameEvents.Add(superEvent);
                }
            }
        }
    }
}
