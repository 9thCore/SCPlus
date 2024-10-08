﻿using BepInEx.Configuration;
using BepInEx;
using System.IO;

namespace SCPlus.plugin
{
    internal class Config
    {
        internal static readonly ConfigFile config = new(Path.Combine(Paths.ConfigPath, "SCPlus.cfg"), true);
        internal static ConfigEntry<bool> patchTranslationLowercasing;
        internal static ConfigEntry<bool> liftVariableTypeCheck;
        internal static ConfigEntry<bool> exposeMoreVariables;

        internal static void Awake()
        {
            patchTranslationLowercasing = config.Bind(
                "QOL",
                nameof(patchTranslationLowercasing),
                true,
                "Patch translation code to return un-lowercased text, if the string is not localised.");

            liftVariableTypeCheck = config.Bind(
                "Variable",
                nameof(liftVariableTypeCheck),
                true,
                "Removes the checks for current disease type related to variables.");

            exposeMoreVariables = config.Bind(
                "Variable",
                nameof(exposeMoreVariables),
                true,
                "Expose variables to the Scenario Creator that are not usually accessible. Not all may be settable.");
        }
    }
}
