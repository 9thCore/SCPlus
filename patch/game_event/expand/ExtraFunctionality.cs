using UnityEngine;
using HarmonyLib;
using SCPlus.patch.hierarchy;
using SCPlus.patch.lang;
using System;
using SCPlus.plugin;
using System.Collections.Generic;
using SCPlus.patch.game_event.tech_overlay;
using SCPlus.patch.game;

namespace SCPlus.patch.game_event
{
    internal static partial class ExtraFunctionality
    {
        private static void Register()
        {
            Transform conditionsOutcomes = null;
            Transform conditionsOutcomesTab = null;
            Transform advancedTabButton = null;
            UISprite advancedTabButtonSprite = null;
            UIPlaySound advancedTabButtonSound = null;
            UIButton conditionsOutcomesTabButton = null;
            Transform tabAdvancedDragPanel = null;

            if (!SetterHelper.EventScreenExists()
                || !HierarchyHelper.TryFindWithLogging(SetterHelper.eventDetails, "Conditions&Outcomes", out conditionsOutcomes)
                || !HierarchyHelper.TryFindWithLogging(SetterHelper.eventDetails, "Tab Buttons", out Transform tabButtons)
                || !HierarchyHelper.TryFindWithLogging(tabButtons, "Tab_01_Conditions_Outcomes", out conditionsOutcomesTab)
                || !HierarchyHelper.TryFindComponentWithLogging(conditionsOutcomesTab, out conditionsOutcomesTabButton)
                || !HierarchyHelper.TryFindWithLogging(tabButtons, "Tab_04_Advanced", out advancedTabButton)
                || !HierarchyHelper.TryFindWithLogging(SetterHelper.tabAdvancedScrollViews, "AdvancedDragPanel", out tabAdvancedDragPanel))
            {
                return;
            }

            advancedTabButtonSprite = advancedTabButton.GetComponent<UISprite>();
            if (advancedTabButtonSprite == null)
            {
                Plugin.Logger.LogError($"Could not create extra functionality toggle");
                return;
            }

            advancedTabButtonSound = advancedTabButton.GetComponent<UIPlaySound>();
            if (advancedTabButtonSound == null)
            {
                Plugin.Logger.LogError($"Could not create extra functionality toggle");
                return;
            }

            if (advancedTabButtonSprite.atlas == null)
            {
                Plugin.Logger.LogError($"Could not create extra functionality toggle");
                return;
            }

            GameObject modeToggle = new($"{LanguageRegister.SCPLUS_TRANSLATION_KEY}_Extra_Toggle");
            modeToggle.SetActive(false);
            HierarchyHelper.Parent(modeToggle.transform, SetterHelper.tabAdvancedScrollViews);
            modeToggle.transform.localPosition = TOGGLE_BUTTON_POSITION;

            // what lack of unity editor inspector does to a mf
            UISprite backgroundSprite = NGUITools.AddSprite(modeToggle, advancedTabButtonSprite.atlas, TOGGLE_SPRITE_BACKGROUND);
            UISprite foregroundSprite = NGUITools.AddSprite(modeToggle, advancedTabButtonSprite.atlas, TOGGLE_SPRITE_FOREGROUND);
            UISprite onSprite = NGUITools.AddSprite(modeToggle, advancedTabButtonSprite.atlas, TOGGLE_SPRITE_ON);
            UISprite offSprite = NGUITools.AddSprite(modeToggle, advancedTabButtonSprite.atlas, TOGGLE_SPRITE_OFF);
            UIButtonCustom button = modeToggle.AddComponent<UIButtonCustom>();
            TooltipObject tooltip = modeToggle.AddComponent<TooltipObject>();
            UIToggle toggle = modeToggle.AddComponent<UIToggle>();
            UIEventToggledObjects toggled = modeToggle.AddComponent<UIEventToggledObjects>();
            UIPlaySound sound = modeToggle.AddComponent<UIPlaySound>();
            BoxCollider boxCollider = modeToggle.AddComponent<BoxCollider>();

            SetDimensions(backgroundSprite, TOGGLE_SIZE);
            backgroundSprite.depth = TOGGLE_DEPTH;

            SetDimensions(foregroundSprite, TOGGLE_SIZE);
            foregroundSprite.depth = TOGGLE_DEPTH + 1;

            onSprite.transform.localPosition += TOGGLE_SPRITE_ONOFF_OFFSET;
            onSprite.width = 20;
            onSprite.height = 19;
            onSprite.depth = TOGGLE_DEPTH + 2;

            offSprite.transform.localPosition += TOGGLE_SPRITE_ONOFF_OFFSET;
            offSprite.width = 20;
            offSprite.height = 6;
            offSprite.depth = TOGGLE_DEPTH + 2;

            button.tweenTarget = foregroundSprite.gameObject;

            tooltip.localisationTag = LanguageRegister.GetLocalizationTag(LanguageRegister.LocalizationKey.ExtraFunc, "GlobalToggleTooltip");

#if (USE_32_COMPAT)
            Plugin.Logger.LogWarning($"Could not create tech lock screen, because the mod is on the 32-bit compatibility patch. Switch to the 64-bit Plague Inc build and 64-bit version of the mod to use it.");

            if (!TryCreateTechRandomScreen(out CTechRandomOverlay randomOverlay)
                || !TryCreateDragPanel(randomOverlay, null, out GameObject dragPanel))
            {
                Plugin.Logger.LogError($"Could not create entire {nameof(UITable)} tree");
                return;
            }
#else
            if (!TryCreateTechLockScreen(out CTechLockOverlay lockOverlay)
                || !TryCreateTechRandomScreen(out CTechRandomOverlay randomOverlay)
                || !TryCreateDragPanel(randomOverlay, lockOverlay, out GameObject dragPanel))
            {
                Plugin.Logger.LogError($"Could not create entire {nameof(UITable)} tree");
                return;
            }
#endif

            UITable table = dragPanel.GetComponentInChildren<UITable>();

            tabToggle = toggle;
            EventDelegate.Add(conditionsOutcomesTabButton.onClick, ExitExtraFunctionalityTab);

            EventDelegate.Add(toggle.onChange, () =>
            {
                dragPanel.SetActive(toggle.value);
                if (toggle.value)
                {
                    table.Reposition();
                }
            });

            toggled.activate = [offSprite.gameObject];
            toggled.deactivate = [onSprite.gameObject, SetterHelper.tabAdvancedDragPanel.gameObject];

            sound.audioClip = advancedTabButtonSound.audioClip;

            boxCollider.size = new Vector3(TOGGLE_SIZE.x, TOGGLE_SIZE.y, 0f);

            modeToggle.SetActive(true);
        }

        private static bool TryCreateTechRandomScreen(out CTechRandomOverlay overlay)
        {
            if (!SetterHelper.TryCreateTechScreen(LanguageRegister.LocalizationKey.ExtraFunc, "Random", out overlay)
                || !HierarchyHelper.TryFindComponentWithLogging(overlay.transform, out UITable table)
                || !HierarchyHelper.TryFindWithLogging(table.transform, "03_Header_Event_Not_Triggered", out Transform header)
                || !HierarchyHelper.TryFindWithLogging(table.transform, "04_Table_Events_Not_Triggered", out Transform techList))
            {
                return false;
            }

            header.gameObject.SetActive(false);
            techList.gameObject.SetActive(false);
            return true;
        }

        private static bool TryCreateTechLockScreen(out CTechLockOverlay lockOverlay)
        {
            return SetterHelper.TryCreateTechScreen(LanguageRegister.LocalizationKey.ExtraFunc, "Lock", out lockOverlay);
        }

        private static bool TryCreateDragPanel(CTechRandomOverlay randomTechOverlay, CTechLockOverlay lockTechOverlay, out GameObject dragPanel)
        {
            dragPanel = GameObject.Instantiate(SetterHelper.tabAdvancedDragPanel.gameObject);
            dragPanel.SetActive(false);
            dragPanel.name = $"{LanguageRegister.SCPLUS_TRANSLATION_KEY}_{LanguageRegister.LocalizationKey.ExtraFunc}_DragPanel";
            HierarchyHelper.Parent(dragPanel.transform, SetterHelper.eventDetails);
            dragPanel.transform.localPosition = SetterHelper.tabAdvancedDragPanel.localPosition;
            dragPanel.transform.localRotation = SetterHelper.tabAdvancedDragPanel.localRotation;
            dragPanel.transform.localScale = SetterHelper.tabAdvancedDragPanel.localScale;

            if (!HierarchyHelper.TryFindComponentWithLogging(dragPanel.transform, out UITable thisTable)
                || !HierarchyHelper.TryFindComponentWithLogging(SetterHelper.tabAdvancedDragPanel, out UITable originalTable)
                || !HierarchyHelper.TryFindWithLogging(originalTable.transform, "00_GENERAL_Header", out Transform headerTemplate)
                || !HierarchyHelper.TryFindComponentWithLogging(originalTable.transform, out BoolSetterToggle boolSetter)
                || !HierarchyHelper.TryFindComponentWithLogging(originalTable.transform, out IntSetter intSetter)
                || !HierarchyHelper.TryFindComponentWithLogging(originalTable.transform, out StringSetter stringSetter)
                || !HierarchyHelper.TryFindComponentWithLogging(HierarchyHelper.Root.transform, out StringListSetter stringListSetter))
            {
                return false;
            }

            foreach (Transform child in thisTable.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            try
            {
                CreateHeader(
                    thisTable,
                    headerTemplate,
                    "RandomTech");

                SetterHelper.CreateCustomBoolSetter(
                    thisTable,
                    boolSetter,
                    LanguageRegister.LocalizationKey.ExtraFunc,
                    "EvolveRandomTech",
                    "evolveRandomTech",
                    GenericRedirector);

                SetterHelper.CreateButton(
                    thisTable,
                    stringListSetter,
                    LanguageRegister.LocalizationKey.ExtraFunc,
                    "RandomTech",
                    () =>
                    {
                        CUIManager.instance.ShowOverlay(randomTechOverlay);
                    });

                CreateHeader(
                    thisTable,
                    headerTemplate,
                    "General");

#if (!USE_32_COMPAT)
                SetterHelper.CreateButton(
                    thisTable,
                    stringListSetter,
                    LanguageRegister.LocalizationKey.ExtraFunc,
                    "LockTech",
                    () =>
                    {
                        CUIManager.instance.ShowOverlay(lockTechOverlay);
                    });
#endif
            }
            catch (Exception ex)
            {
                ex.StackTrace.Trim(); // remove the "declared but never used" annoying warning
                return false;
            }

            return true;
        }

        private static object GenericRedirector(object to)
        {
            if (to is not GameEvent @event)
            {
                throw new InvalidOperationException($"Invalid type {to.GetType()}, expected {nameof(GameEvent)}");
            }

            return GetDataOrDefault(@event);
        }

        private static void CreateHeader(UITable uiTable, Transform headerTemplate, string internalName)
        {
            Transform headerClone = Transform.Instantiate(headerTemplate);
            headerClone.name = $"{SetterHelper.currentIndex:D4}_{internalName}";
            HierarchyHelper.Parent(headerClone, uiTable.transform);

            if (!HierarchyHelper.TryFindComponentWithLogging(headerClone, out UILabel label)
                || !HierarchyHelper.TryFindComponentWithLogging(headerClone, out UILabelAutotranslate translator))
            {
                Plugin.Logger.LogError($"Could not clone header {headerTemplate}");
                GameObject.Destroy(headerClone.gameObject);
                throw new InvalidOperationException();
            }

            label.text = LanguageRegister.GetLocalizationTag(LanguageRegister.LocalizationKey.ExtraFunc, internalName);
            translator.originalLabelText = label.text;
            translator.UseOriginalLabel();

            SetterHelper.currentIndex++;
        }

        internal static void SetDimensions(UISprite sprite, Vector2 size)
        {
            sprite.width = (int)size.x;
            sprite.height = (int)size.y;
        }

        internal static Data GetDataOrDefault(GameEvent gameEvent)
        {
            if (!eventData.TryGetValue(gameEvent, out Data data))
            {
                data = new();
                eventData.Add(gameEvent, data);
            }

            return data;
        }

        internal static void ExitExtraFunctionalityTab()
        {
            if (tabToggle == null)
            {
                Plugin.Logger.LogError($"{nameof(ExitExtraFunctionalityTab)} called without {tabToggle} being set");
                return;
            }

            tabToggle.Set(false);
        }

        internal static UIToggle tabToggle = null;
        internal static readonly Dictionary<GameEvent, Data> eventData = [];

        // lol
        private static readonly Vector3 TOGGLE_BUTTON_POSITION = new(1345f, -185f, 0f);
        private const int TOGGLE_DEPTH = 5;
        private static readonly Vector2 TOGGLE_SIZE = new(60, 60);
        private static readonly Vector2 TOGGLE_POSITION = new(28, 28);
        private static readonly Vector3 TOGGLE_SPRITE_ONOFF_OFFSET = new(
            (TOGGLE_SIZE.x - TOGGLE_POSITION.x) * 0.5f,
            -(TOGGLE_SIZE.y - TOGGLE_POSITION.y) * 0.5f,
            0f);

        private static readonly string TOGGLE_SPRITE_BACKGROUND = "Standard";
        private static readonly string TOGGLE_SPRITE_FOREGROUND = "spanner";
        private static readonly string TOGGLE_SPRITE_ON = "Plus_Symbol";
        private static readonly string TOGGLE_SPRITE_OFF = "Minus_Symbol";

        internal class Data
        {
            public bool evolveRandomTech = false;
            public bool deEvolveRandomTech = false;
            public string[] randomTech = null;
            public string function = null;
#if (!USE_32_COMPAT)
            public EventLockTech[] eventLockTech = null;
#endif

            internal bool IsDefault()
            {
                return evolveRandomTech == false
                    && deEvolveRandomTech == false
                    && randomTech == null
                    && function == null
#if (USE_32_COMPAT)
;
#else
                    && eventLockTech == null;
#endif
            }
        }

        [HarmonyPatch(typeof(CEventScreen), nameof(CEventScreen.Initialise))]
        private static class Patch
        {
            private static void Postfix()
            {
                if (!Config.expandEventFunctionality.Value)
                {
                    return;
                }

                Register();
            }
        }
    }
}
