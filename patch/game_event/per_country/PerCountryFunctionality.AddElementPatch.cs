using HarmonyLib;
using SCPlus.plugin;

namespace SCPlus.patch.game_event.per_country
{
    internal partial class PerCountryFunctionality
    {
        [HarmonyPatch(typeof(GameItemList), nameof(GameItemList.AddElement))]
        private static class AddElementPatch
        {
            private static void Postfix(GameItemList __instance, GameItemElement element)
            {
                if (!Config.perCountryEventFunctionality.Value
                    || __instance is not EventList
                    || element is not EventListElement eventElement)
                {
                    return;
                }

                if (creatingPerCountryEvent || perCountryEvents.Contains(eventElement.Event))
                {
                    EmphasizeEvent(eventElement);
                }

                if (creatingPerCountryEvent)
                {
                    perCountryEvents.Add(eventElement.Event);
                    creatingPerCountryEvent = false;
                }
            }
        }
    }
}
