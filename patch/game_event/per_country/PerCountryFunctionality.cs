using HarmonyLib;
using SCPlus.patch.game;
using SCPlus.patch.hierarchy;
using SCPlus.patch.lang;
using SCPlus.plugin;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SCPlus.patch.game_event.per_country
{
    internal partial class PerCountryFunctionality
    {
        internal static void Register()
        {
            if (!SetterHelper.EventScreenExists()
                || !HierarchyHelper.TryFindWithLogging(SetterHelper.eventScreen.eventList.createNewOverlay.transform, "Anchor - Center", out Transform anchor)
                || !HierarchyHelper.TryFindWithLogging(anchor, "Buttons", out Transform buttonsList)
                || !HierarchyHelper.TryFindWithLogging(buttonsList, "Button_Create", out Transform buttonCreate)
                || !HierarchyHelper.TryFindWithLogging(buttonsList, "Button_Cancel", out Transform buttonCancel)
                || !HierarchyHelper.TryFindComponentWithLogging(buttonCancel, out UIButton buttonCancelButton))
            {
                Plugin.Logger.LogError($"Could not create per-country functionality button");
                return;
            }

            Transform perCountryCreate = Transform.Instantiate(buttonCreate);
            perCountryCreate.name = $"Button_PerCountry";

            HierarchyHelper.Parent(perCountryCreate.transform, buttonsList);
            perCountryCreate.transform.localPosition = Vector3.zero;

            if (!HierarchyHelper.TryFindComponentWithLogging(perCountryCreate, out BoxCollider boxCollider)
                || !HierarchyHelper.TryFindComponentWithLogging(perCountryCreate, out UIButton button)
                || !HierarchyHelper.TryFindWithLogging(perCountryCreate, "Text", out Transform text)
                || !HierarchyHelper.TryFindComponentWithLogging(text, out UILabel label)
                || !HierarchyHelper.TryFindComponentWithLogging(text, out UILabelAutotranslate translator))
            {
                GameObject.DestroyImmediate(perCountryCreate.gameObject);
                Plugin.Logger.LogError($"Invalid tree on per-country functionality button");
                return;
            }

            label.text = LanguageRegister.GetLocalizationTag(LanguageRegister.LocalizationKey.PerCountry, "Button");
            translator.SetInitialText(label.text);

            EventDelegate.Add(buttonCancelButton.onClick, () =>
            {
                creatingPerCountryEvent = false;
            });

            EventDelegate.Add(button.onClick, () =>
            {
                creatingPerCountryEvent = true;
            });
            
            // ensure custom logic runs before base logic
            button.onClick.Reverse();
        }

        internal static string GetSubEventName(GameEvent gameEvent, Country country)
        {
            return $"{gameEvent.name}:{country.id}:{SUFFIX}";
        }

        internal static bool IsSubEvent(GameEvent gameEvent)
        {
            return gameEvent.name.EndsWith(SUFFIX);
        }

        internal static bool TryGetSubEventCountry(GameEvent gameEvent, out Country country)
        {
            if (gameEvent.localCondition == null
                || gameEvent.localCondition.parameterConditions == null
                || gameEvent.localCondition.parameterConditions.Length == 0)
            {
                country = null;
                return false;
            }

            ParameterCondition condition = gameEvent.localCondition.parameterConditions.Last();
            if (condition.target == EReflectionTarget.COUNTRY
                && condition.fieldName == nameof(Country.id))
            {
                bool match(Country country) => country.id == condition.val;

                if (!ScenarioCreatorAPI.Instance.Countries.Exists(match))
                {
                    country = null;
                    return false;
                }

                country = ScenarioCreatorAPI.Instance.Countries.Find(match);
                return true;
            }

            country = null;
            return false;
        }

        internal static Country GetSubEventCountry(GameEvent gameEvent)
        {
            if (gameEvent.localCondition == null
                || gameEvent.localCondition.parameterConditions == null
                || gameEvent.localCondition.parameterConditions.Length == 0)
            {
                return null;
            }

            ParameterCondition condition = gameEvent.localCondition.parameterConditions.Last();
            if (condition.target == EReflectionTarget.COUNTRY
                && condition.fieldName == nameof(Country.id))
            {
                return ScenarioCreatorAPI.Instance.Countries.Find(country => country.id == condition.val);
            }

            return null;
        }

        internal static string GetSubEventRealName(GameEvent gameEvent)
        {
            if (IsSubEvent(gameEvent))
            {
                Country country = GetSubEventCountry(gameEvent);
                if (country == default)
                {
                    return gameEvent.name.Substring(0, gameEvent.name.IndexOf(SUFFIX) - 1);
                }

                return gameEvent.name.Substring(0, gameEvent.name.IndexOf(country.id) - 1);
            }

            return gameEvent.name;
        }

        internal static void EmphasizeEvent(EventListElement element)
        {
            if (HierarchyHelper.TryFindWithLogging(element.transform, "Text", out Transform text)
                    && HierarchyHelper.TryFindComponentWithLogging(text, out UILabel label))
            {
                label.color = PER_COUNTRY_EVENT_COLOR;
            }
        }

        internal static HashSet<GameEvent> perCountryEvents = [];
        internal static HashSet<GameEvent> subEvents = [];
        internal static bool creatingPerCountryEvent = false;

        // lol
        internal static readonly string SUFFIX = $"InternalSubEvent ({LanguageRegister.SCPLUS_TRANSLATION_KEY})";
        internal static readonly Color PER_COUNTRY_EVENT_COLOR = new(0f, 0.9f, 0.9f);

        [HarmonyPatch(typeof(CEventScreen), nameof(CEventScreen.Initialise))]
        private static class Patch
        {
            private static void Postfix()
            {
                if (!Config.perCountryEventFunctionality.Value)
                {
                    return;
                }

                Register();
            }
        }
    }
}
