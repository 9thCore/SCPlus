using SCPlus.patch.game_event;
using SCPlus.patch.hierarchy;
using SCPlus.patch.lang;
using System;
using UnityEngine;

namespace SCPlus.patch.game
{
    internal static class SetterHelper
    {
        internal static bool exists = false;
        internal static CEventScreen eventScreen;
        internal static Transform anchor;
        internal static Transform eventList;
        internal static Transform eventDetails;
        internal static Transform tabAdvanced;
        internal static Transform tabAdvancedScrollViews;
        internal static Transform tabAdvancedDragPanel;
        internal static CEventAdvancedSubscreen tabAdvancedComponent;
        internal static OutcomePanel outcomePanel;
        internal static CTechTriggerOverlay techTriggerOverlay;
        internal static ulong currentIndex;

        internal static bool EventScreenExists()
        {
            if (exists)
            {
                return true;
            }

            if (!HierarchyHelper.TryFindComponentWithLogging(HierarchyHelper.Root.transform, out eventScreen)
                || !HierarchyHelper.TryFindComponentWithLogging(HierarchyHelper.Root.transform, out techTriggerOverlay)
                || !HierarchyHelper.TryFindWithLogging(eventScreen.transform, "Anchor Right", out anchor)
                || !HierarchyHelper.TryFindWithLogging(anchor, "Event_List", out eventList)
                || !HierarchyHelper.TryFindWithLogging(anchor, "Event_Details", out eventDetails)
                || !HierarchyHelper.TryFindComponentWithLogging(eventDetails, out outcomePanel)
                || !HierarchyHelper.TryFindWithLogging(eventDetails, "Advanced", out tabAdvanced)
                || !HierarchyHelper.TryFindComponentWithLogging(tabAdvanced, out tabAdvancedComponent)
                || !HierarchyHelper.TryFindWithLogging(tabAdvanced, "Scroll Views", out tabAdvancedScrollViews)
                || !HierarchyHelper.TryFindWithLogging(tabAdvancedScrollViews, "AdvancedDragPanel", out tabAdvancedDragPanel))
            {
                Plugin.Logger.LogError($"Could not find required GameObjects");
                return false;
            }

            exists = true;
            return true;
        }

        internal static GameObject CreateButton(UITable uiTable, ListSetter setterTemplate, LanguageRegister.LocalizationKey key, string internalName, EventDelegate.Callback callback)
        {
            GameObject gameObject = CreateSetter(uiTable, setterTemplate, key, internalName, "", out ListSetter setter, false);
            gameObject.SetActive(false);

            if (!HierarchyHelper.TryFindComponentWithLogging(gameObject.transform, out UIDropdownPopupList dropdownPopupList)
                || !HierarchyHelper.TryFindWithLogging(dropdownPopupList.transform, "Remove_Button", out Transform remove)
                || !HierarchyHelper.TryFindComponentWithLogging(remove, out UISprite removeSprite)
                || !HierarchyHelper.TryFindComponentWithLogging(remove, out UIButton removeButton)
                || !HierarchyHelper.TryFindComponentWithLogging(remove, out BoxCollider removeCollider)
                || !HierarchyHelper.TryFindComponentWithLogging(gameObject.transform, out UILabel labelTemplate))
            {
                Plugin.Logger.LogError($"Could not setup button");
                return gameObject;
            }

            GameObject labelClone = UnityEngine.Object.Instantiate(labelTemplate.gameObject);
            HierarchyHelper.Parent(labelClone.transform, remove.transform);

            if (!HierarchyHelper.TryFindComponentWithLogging(labelClone.transform, out UILabel label)
                || !HierarchyHelper.TryFindComponentWithLogging(labelClone.transform, out UILabelAutotranslate translator))
            {
                Plugin.Logger.LogError($"Could not setup button");
                return gameObject;
            }

            label.width = BUTTON_SIZE.x;
            label.text = LanguageRegister.GetLocalizationTag(key, $"{internalName}_Button");
            translator.originalLabelText = label.text;
            translator.UseOriginalLabel();

            remove.transform.SetParent(gameObject.transform);
            remove.localPosition = BUTTON_POSITION;

            removeButton.onClick.Clear();
            EventDelegate.Add(removeButton.onClick, callback);
            removeButton.normalSprite = "Standard";

            removeSprite.type = UIBasicSprite.Type.Sliced;
            removeSprite.rawPivot = UIWidget.Pivot.Center;
            removeSprite.SetDimensions(BUTTON_SIZE.x, BUTTON_SIZE.y);

            removeCollider.center = Vector3.zero;
            removeCollider.size = BUTTON_COLLIDER_SIZE;

            UnityEngine.Object.DestroyImmediate(setter);

            UIDropdownPopupList.popups.Remove(dropdownPopupList);

            UnityEngine.Object.Destroy(dropdownPopupList.gameObject);

            gameObject.SetActive(true);
            return gameObject;
        }

        private static GameObject Redirect(GameObject gameObject, Func<object, object> redirectorFunc)
        {
            VariableSetterRedirector redirector = gameObject.AddComponent<VariableSetterRedirector>();
            redirector.apply = redirectorFunc;
            return gameObject;
        }

        internal static GameObject CreateCustomBoolSetter<T>(UITable uiTable, T setterTemplate, LanguageRegister.LocalizationKey key, string internalName, string variable, Func<object, object> redirectorFunc, bool eventVariable = true) where T : BoolSetter
        {
            return Redirect(CreateBoolSetter(uiTable, setterTemplate, key, internalName, variable, eventVariable), redirectorFunc);
        }

        internal static GameObject CreateBoolSetter<T>(UITable uiTable, T setterTemplate, LanguageRegister.LocalizationKey key, string internalName, string variable, bool eventVariable = true) where T : BoolSetter
        {
            CreateSetter(uiTable, setterTemplate, key, internalName, variable, out T setter, eventVariable);
            return setter.gameObject;
        }

        internal static GameObject CreateCustomListSetter<T>(UITable uITable, T setterTemplate, LanguageRegister.LocalizationKey key, string internalName, string variable, ListSetterElement[] elements, Func<object, object> redirectorFunc, bool eventVariable = true) where T : ListSetter
        {
            return Redirect(CreateListSetter(uITable, setterTemplate, key, internalName, variable, elements, out T setter, eventVariable), redirectorFunc);
        }

        internal static GameObject CreateListSetter<T>(UITable uiTable, T setterTemplate, LanguageRegister.LocalizationKey key, string internalName, string variable, ListSetterElement[] elements, out T setter, bool eventVariable = true) where T : ListSetter
        {
            GameObject gameObject = CreateSetter(uiTable, setterTemplate, key, internalName, variable, out setter, eventVariable);
            gameObject.SetActive(false);

            if (!HierarchyHelper.TryFindComponentWithLogging(gameObject.transform, out UIDropdownPopupList dropdownList)
                || !HierarchyHelper.TryFindWithLogging(dropdownList.transform, "Remove_Button", out Transform removeButton)
                || !HierarchyHelper.TryFindComponentWithLogging(removeButton, out UIButton button))
            {
                Plugin.Logger.LogError($"Could not setup {nameof(ListSetter)}");
                throw new InvalidOperationException();
            }

            setter.listElementData = elements;
            dropdownList.SetByValue(elements[0].value);
            dropdownList.Locked = false;

            EventDelegate.Add(button.onClick, () =>
            {
                dropdownList.SetByValue(elements[0].value);
            });

            dropdownList.transform.localPosition = LIST_SETTER_POSITION;

            gameObject.SetActive(true);
            return setter.gameObject;
        }

        internal static GameObject CreateSetter<T>(UITable uiTable, T setterTemplate, LanguageRegister.LocalizationKey key, string internalName, string variable, out T setter, bool eventVariable) where T : VariableSetter
        {
            Transform setterClone = UnityEngine.Object.Instantiate(setterTemplate.transform);
            setterClone.name = $"{currentIndex:D4}_{internalName}";
            HierarchyHelper.Parent(setterClone, uiTable.transform);

            if (!HierarchyHelper.TryFindComponentWithLogging(setterClone, out setter)
                || !HierarchyHelper.TryFindComponentWithLogging(setterClone, out UILabel label)
                || !HierarchyHelper.TryFindComponentWithLogging(setterClone, out UILabelAutotranslate translator)
                || !HierarchyHelper.TryFindComponentWithLogging(setterClone, out TooltipObject tooltip))
            {
                Plugin.Logger.LogError($"Could not clone setter {setterTemplate}");
                UnityEngine.Object.Destroy(setterClone.gameObject);
                throw new InvalidOperationException();
            }

            label.text = LanguageRegister.GetSetterLocalizationTag(key, internalName);
            translator.originalLabelText = label.text;
            translator.UseOriginalLabel();

            tooltip.localisationTag = LanguageRegister.GetSetterHelpLocalizationTag(key, internalName);

            setter.variable = variable;

            if (eventVariable)
            {
                tabAdvancedComponent.variableSetters.Add(setter);
            }

            currentIndex++;
            return setterClone.gameObject;
        }

        internal static bool TryCreateTechScreen<T>(LanguageRegister.LocalizationKey key, string suffix, out T overlay) where T : CTechSelectionOverlay
        {
            if (!EventScreenExists())
            {
                overlay = null;
                return false;
            }

            GameObject techScreen = GameObject.Instantiate(SetterHelper.techTriggerOverlay.gameObject);
            techScreen.SetActive(false);
            techScreen.name = $"{LanguageRegister.SCPLUS_TRANSLATION_KEY}_{key}_TechScreen_{suffix}";
            HierarchyHelper.Parent(techScreen.transform, HierarchyHelper.Camera.transform);
            techScreen.transform.position = Vector3.zero;

            if (!HierarchyHelper.TryFindComponentWithLogging(techScreen.transform, out CTechTriggerOverlay triggerOverlay)
                || !HierarchyHelper.TryFindComponentWithLogging(triggerOverlay.transform, out UITable table)
                || !HierarchyHelper.TryFindWithLogging(table.transform, "03_Header_Event_Not_Triggered", out Transform headerUntriggered)
                || !HierarchyHelper.TryFindWithLogging(table.transform, "00_Header_Event_Triggered", out Transform headerTriggered)
                || !HierarchyHelper.TryFindWithLogging(headerUntriggered, "Title_Events_Not_Triggered", out Transform titleUntriggered)
                || !HierarchyHelper.TryFindWithLogging(headerTriggered, "Title_Event_Triggered", out Transform titleTriggered)
                || !HierarchyHelper.TryFindWithLogging(headerUntriggered, "Instructions", out Transform instructionUntriggered)
                || !HierarchyHelper.TryFindWithLogging(headerTriggered, "Instruction", out Transform instructionTriggered)
                || !HierarchyHelper.TryFindComponentWithLogging(titleUntriggered, out UILabel titleUntriggeredLabel)
                || !HierarchyHelper.TryFindComponentWithLogging(titleTriggered, out UILabel titleTriggeredLabel)
                || !HierarchyHelper.TryFindComponentWithLogging(instructionUntriggered, out UILabel instructionUntriggeredLabel)
                || !HierarchyHelper.TryFindComponentWithLogging(instructionTriggered, out UILabel instructionTriggeredLabel))
            {
                overlay = null;
                return false;
            }

            HierarchyHelper.SwitchComponentWithSub(true, false, triggerOverlay, out overlay);

            overlay.tooltipPositiveText = LanguageRegister.GetLocalizationTag(key, $"TechScreen_Help_{suffix}");
            overlay.tooltipNegativeText = LanguageRegister.GetLocalizationTag(key, $"TechScreen_Help_Not_{suffix}");
            overlay.title.text = LanguageRegister.GetLocalizationTag(key, $"TechScreen_{suffix}");
            HierarchyHelper.EnsureComponent<UILabelAutotranslate>(overlay.title.gameObject);

            overlay.paramRoot = new()
            {
                parameterConditions = []
            };

            titleTriggeredLabel.text = LanguageRegister.GetLocalizationTag(key, $"TechScreen_Title_{suffix}");
            titleUntriggeredLabel.text = LanguageRegister.GetLocalizationTag(key, $"TechScreen_Title_Not_{suffix}");
            instructionTriggeredLabel.text = LanguageRegister.GetLocalizationTag(key, $"TechScreen_Instruction_{suffix}");
            instructionUntriggeredLabel.text = LanguageRegister.GetLocalizationTag(key, $"TechScreen_Instruction_Not_{suffix}");


            return true;
        }

        // lol
        private static readonly Vector3 LIST_SETTER_POSITION = new(591f, 0f, 0f);
        private static readonly Vector3 BUTTON_POSITION = new(784f, 0f, 0f);
        private static readonly Vector2Int BUTTON_SIZE = new(384, 64);
        private static readonly Vector3 BUTTON_COLLIDER_SIZE = new(BUTTON_SIZE.x, BUTTON_SIZE.y, 0f);
    }
}
