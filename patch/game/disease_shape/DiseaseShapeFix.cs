using HarmonyLib;
using SCPlus.patch.lang;
using SCPlus.plugin;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SCPlus.patch.game.disease_shape
{
    internal class DiseaseShapeFix
    {
        private static void Register(CDiseaseScreen screen)
        {
            if (screen.DiseaseModelTypeList == null
                || screen.DiseaseModelTypeList.elements == null)
            {
                Plugin.Logger.LogError($"Could not run patch: {nameof(Config.diseaseShapeNameFix)}");
                return;
            }

            UIDropdownPopupElement tutorialElement = screen.DiseaseModelTypeList.elements.Find(element => Enum.TryParse(element.Value, true, out Disease.EDiseaseType type) && type == Disease.EDiseaseType.TUTORIAL);
            if (screen.DiseaseModelTypeList.elements.Contains(tutorialElement))
            {
                screen.DiseaseModelTypeList.elements.Remove(tutorialElement);
                GameObject.Destroy(tutorialElement.gameObject);
            }

            foreach (UIDropdownPopupElement element in screen.DiseaseModelTypeList.elements)
            {
                if (!Enum.TryParse(element.Value, true, out Disease.EDiseaseType type))
                {
                    Plugin.Logger.LogWarning($"{nameof(Config.diseaseShapeNameFix)}: element {element} [{element.Value}] is not part of the enum");
                    continue;
                }

                int ordinal = (int)type;
                element.gameObject.name = $"{ordinal:D4}_{element.gameObject.name}_{LanguageRegister.SCPLUS_TRANSLATION_KEY}";
            }
        }

        [HarmonyPatch(typeof(CDiseaseScreen), nameof(CDiseaseScreen.Initialise))]
        private static class Patch
        {
            private static void Postfix(CDiseaseScreen __instance)
            {
                if (!Config.diseaseShapeNameFix.Value)
                {
                    return;
                }

                Register(__instance);
            }
        }
    }
}
