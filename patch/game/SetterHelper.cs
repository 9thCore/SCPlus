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

        internal static GameObject CreateButton(UITable uiTable, ListSetter setterTemplate, TranslationKey key, string internalName, EventDelegate.Callback callback)
        {
            GameObject gameObject = CreateSetter(uiTable, setterTemplate, key, internalName, "", out ListSetter setter);
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
            label.text = GetTranslation(key, $"{internalName}_Button");
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

            tabAdvancedComponent.variableSetters.Remove(setter);
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

        internal static GameObject CreateCustomBoolSetter(UITable uiTable, BoolSetterToggle setterTemplate, TranslationKey key, string internalName, string variable, Func<object, object> redirectorFunc)
        {
            return Redirect(CreateBoolSetter(uiTable, setterTemplate, key, internalName, variable), redirectorFunc);
        }

        internal static GameObject CreateBoolSetter(UITable uiTable, BoolSetterToggle setterTemplate, TranslationKey key, string internalName, string variable)
        {
            CreateSetter(uiTable, setterTemplate, key, internalName, variable, out BoolSetterToggle setter);
            return setter.gameObject;
        }

        internal static GameObject CreateCustomListSetter<T>(UITable uITable, T setterTemplate, TranslationKey key, string internalName, string variable, ListSetterElement[] elements, Func<object, object> redirectorFunc) where T : ListSetter
        {
            return Redirect(CreateListSetter(uITable, setterTemplate, key, internalName, variable, elements, out T setter), redirectorFunc);
        }

        internal static GameObject CreateListSetter<T>(UITable uiTable, T setterTemplate, TranslationKey key, string internalName, string variable, ListSetterElement[] elements, out T setter) where T : ListSetter
        {
            GameObject gameObject = CreateSetter(uiTable, setterTemplate, key, internalName, variable, out setter);
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

        internal static GameObject CreateSetter<T>(UITable uiTable, T setterTemplate, TranslationKey key, string internalName, string variable, out T setter) where T : VariableSetter
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

            label.text = GetSetterTranslation(key, internalName);
            translator.originalLabelText = label.text;
            translator.UseOriginalLabel();

            tooltip.localisationTag = GetSetterHelpTranslation(key, internalName);

            setter.variable = variable;
            tabAdvancedComponent.variableSetters.Add(setter);

            currentIndex++;
            return setterClone.gameObject;
        }

        internal static string GetSetterHelpTranslation(TranslationKey key, string suffix)
        {
            return GetSetterTranslation(key, $"{suffix}_Help");
        }

        internal static string GetSetterTranslation(TranslationKey key, string suffix)
        {
            return GetTranslation(key, $"{suffix}_Setter");
        }

        internal static string GetTranslation(TranslationKey key, string suffix)
        {
            return $"{LanguageRegister.SCPLUS_TRANSLATION_KEY}_{key}_{suffix}";
        }

        internal enum TranslationKey
        {
            ExtraFunc,
            PerCountry
        }

        // lol
        private static readonly Vector3 LIST_SETTER_POSITION = new(591f, 0f, 0f);
        private static readonly Vector3 BUTTON_POSITION = new(784f, 0f, 0f);
        private static readonly Vector2Int BUTTON_SIZE = new(384, 64);
        private static readonly Vector3 BUTTON_COLLIDER_SIZE = new(BUTTON_SIZE.x, BUTTON_SIZE.y, 0f);
    }
}
