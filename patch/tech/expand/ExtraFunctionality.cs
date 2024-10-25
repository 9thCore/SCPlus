using HarmonyLib;
using Newtonsoft.Json.Utilities;
using SCPlus.patch.game;
using SCPlus.patch.hierarchy;
using SCPlus.patch.lang;
using SCPlus.plugin;
using System;
using UnityEngine;

namespace SCPlus.patch.tech.expand
{
    internal class ExtraFunctionality
    {
        private static void Register(TechManagementScreen screen)
        {
            if (!HierarchyHelper.TryFindWithLogging(screen.transform, "Anchor Right", out Transform anchor)
                || !HierarchyHelper.TryFindWithLogging(anchor, "Bottom", out Transform bottom)
                || !HierarchyHelper.TryFindComponentWithLogging(bottom, out UIPanel bottomPanel)
                || !HierarchyHelper.TryFindWithLogging(bottomPanel.transform, "Table", out Transform bottomTable)
                || !HierarchyHelper.TryFindComponentWithLogging(bottomTable.transform, out UITable bottomTableComponent)
                || !HierarchyHelper.TryFindWithLogging(bottomTable, "40B_Tech_Cant_Devolve", out Transform cantDevolve)
                || !HierarchyHelper.TryFindComponentWithLogging(bottomTable, out BoolToIntSetter boolToIntSetter))
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
                    $"40D_{LanguageRegister.SCPLUS_TRANSLATION_KEY}_EventLocked");
            } catch (Exception ex)
            {
                Plugin.Logger.LogError($"Caught error while creating {nameof(tech)}.{nameof(ExtraFunctionality)}: {ex}");
            }
        }

        private static GameObject CreateBoolSetter(UITable uiTable, BoolToIntSetter boolToIntSetter, string internalName, string variable, string name)
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

            HierarchyHelper.SwitchComponentWithSuper(true, false, setter.GetComponent<BoolToIntSetter>(), out BoolSetter boolSetter);

            uiToggleNo.group = toggleGroup;
            uiToggleYes.group = toggleGroup;
            toggleGroup++;

            return setter;
        }

        // Presumably, this should be high enough to where there arent issues LOL
        private static int toggleGroup = 10000000;

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
