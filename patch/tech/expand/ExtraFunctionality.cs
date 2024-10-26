using HarmonyLib;
using SCPlus.patch.game;
using SCPlus.patch.game_event;
using SCPlus.patch.hierarchy;
using SCPlus.patch.lang;
using SCPlus.patch.tech.expand.tech_overlay;
using SCPlus.plugin;
using System;
using UnityEngine;

namespace SCPlus.patch.tech.expand
{
    internal partial class ExtraFunctionality
    {
        private static void Register(TechManagementScreen screen)
        {
            if (!HierarchyHelper.TryFindWithLogging(screen.transform, "Anchor Right", out Transform anchor)
                || !HierarchyHelper.TryFindWithLogging(anchor, "Bottom", out Transform bottom)
                || !HierarchyHelper.TryFindComponentWithLogging(bottom, out UIPanel bottomPanel)
                || !HierarchyHelper.TryFindWithLogging(bottomPanel.transform, "Table", out Transform bottomTable)
                || !HierarchyHelper.TryFindComponentWithLogging(bottomTable, out UITable bottomTableComponent)
                || !HierarchyHelper.TryFindWithLogging(bottomTable, "45_Header_Tech_Tree", out Transform techTreeHeader)
                || !HierarchyHelper.TryFindComponentWithLogging(techTreeHeader, out UIToggledObjects techTreeHeaderToggle)
                || !HierarchyHelper.TryFindWithLogging(bottomTable, "40_Advanced_Settings_Header", out Transform advancedSettingsHeader)
                || !HierarchyHelper.TryFindComponentWithLogging(advancedSettingsHeader, out UIToggledObjects advancedSettingsHeaderToggle)
                || !HierarchyHelper.TryFindComponentWithLogging(bottomTable, out BoolToIntSetter boolToIntSetter)
                || !HierarchyHelper.TryFindComponentWithLogging(bottomTable, out ListSetter listSetter))
            {
                return;
            }

            try
            {
                CreateBoolSetter(
                    bottomTableComponent,
                    boolToIntSetter,
                    "EventLocked",
                    "eventLocked",
                    $"40D_{LanguageRegister.SCPLUS_TRANSLATION_KEY}_EventLocked",
                    advancedSettingsHeaderToggle);                

                if (Config.replaceTraitRequirement.Value)
                {
                    ReplaceTechRequirements(screen, techTreeHeaderToggle, bottomTableComponent, listSetter);
                }
            } catch (Exception ex)
            {
                Plugin.Logger.LogError($"Caught error while creating {nameof(tech)}.{nameof(ExtraFunctionality)}: {ex}");
                return;
            }
        }

        private static void ReplaceTechRequirements(TechManagementScreen screen, UIToggledObjects techTreeHeader, UITable table, ListSetter listSetter)
        {
            if (!HierarchyHelper.TryFindWithLogging(table.transform, "45A_Tech_Tree_Dependency", out Transform dependencies)
                || !TryCreateRequirementScreen(screen, "", out CTechRequirementOverlay requirementOverlay)
                || !TryCreateRequirementScreen(screen, "Not", out CTechRequirementNotOverlay requirementNotOverlay))
            {
                Plugin.Logger.LogError($"Could not replace tech requirements");
                return;
            }

            GameObject additionalLayer = new()
            {
                name = $"{LanguageRegister.SCPLUS_TRANSLATION_KEY}_{nameof(Config.replaceTraitRequirement)}_ADDITIONAL_LAYER"
            };
            additionalLayer.transform.SetParent(table.transform);
            dependencies.transform.SetParent(additionalLayer.transform);
            additionalLayer.SetActive(false);

            CreateButton(
                table,
                listSetter,
                "Requirement",
                () =>
                {
                    CUIManager.instance.ShowOverlay(requirementOverlay);
                },
                "45A_Tech_Requirements",
                techTreeHeader);

            CreateButton(
                table,
                listSetter,
                "RequirementNot",
                () =>
                {
                    CUIManager.instance.ShowOverlay(requirementNotOverlay);
                },
                "45B_Tech_Requirements_Not",
                techTreeHeader);
        }

        private static GameObject CreateButton(UITable uiTable, ListSetter listSetter, string internalName, EventDelegate.Callback callback, string name, UIToggledObjects uiToggle)
        {
            GameObject result = SetterHelper.CreateButton(uiTable, listSetter, LanguageRegister.LocalizationKey.TechExtraFunc, internalName, callback);
            result.name = name;

            if (!HierarchyHelper.TryFindWithLogging(result.transform, "Remove_Button", out Transform removeButton))
            {
                return result;
            }

            uiToggle.activate.Add(result);
            removeButton.localPosition = BUTTON_POSITION;

            return result;
        }

        private static GameObject CreateBoolSetter(UITable uiTable, BoolToIntSetter boolToIntSetter, string internalName, string variable, string name, UIToggledObjects uiToggle)
        {
            GameObject setter = SetterHelper.CreateBoolSetter(uiTable, boolToIntSetter, LanguageRegister.LocalizationKey.TechExtraFunc, internalName, variable, false);
            setter.name = name;

            if (!HierarchyHelper.TryFindComponentWithLogging(setter.transform, out ToggleSelector selector)
                || !HierarchyHelper.TryFindWithLogging(selector.transform, "Toggle_No", out Transform toggleNo)
                || !HierarchyHelper.TryFindComponentWithLogging(toggleNo, out UIToggle uiToggleNo)
                || !HierarchyHelper.TryFindWithLogging(selector.transform, "Toggle_Yes", out Transform toggleYes)
                || !HierarchyHelper.TryFindComponentWithLogging(toggleYes, out UIToggle uiToggleYes))
            {
                return setter;
            }

            uiToggle.activate.Add(setter);

            HierarchyHelper.SwitchComponentWithSuper(true, false, setter.GetComponent<BoolToIntSetter>(), out BoolSetter _);

            uiToggleNo.group = toggleGroup;
            uiToggleYes.group = toggleGroup;
            toggleGroup++;

            return setter;
        }

        private static bool TryCreateRequirementScreen<T>(TechManagementScreen screen, string suffix, out T overlay) where T : CTechRequirementOverlay
        {
            if (!TryCreateScreen($"Requirement{suffix}", out overlay))
            {
                return false;
            }

            overlay.screen = screen;
            return true;
        }

        private static bool TryCreateScreen<T>(string name, out T overlay) where T : CTechSelectionOverlay
        {
            if (!SetterHelper.TryCreateTechScreen(LanguageRegister.LocalizationKey.TechExtraFunc, name, out overlay))
            {
                return false;
            }

            return true;
        }

        // Presumably, this should be high enough to where there arent issues LOL
        private static int toggleGroup = 10000000;
        private static readonly Vector3 BUTTON_POSITION = new(1600f, 0f, 0f);

        [HarmonyPatch(typeof(TechManagementScreen), nameof(TechManagementScreen.Initialise))]
        private static class Patch
        {
            private static void Prefix(TechManagementScreen __instance)
            {
                if (!Config.expandTraitFunctionality.Value)
                {
                    return;
                }

                Register(__instance);
            }
        }
    }
}
