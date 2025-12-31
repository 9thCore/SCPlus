
#if (!MELON_LOADER)
using BepInEx;
using BepInEx.Logging;
#else
using MelonLoader;
#endif

using HarmonyLib;
using SCPlus.patch.lang;
using SCPlus.patch.variable;
using System;
using UnityEngine;

#if (MELON_LOADER)
[assembly: MelonInfo(typeof(SCPlus.Plugin), SCPlus.MyPluginInfo.PLUGIN_NAME, SCPlus.MyPluginInfo.PLUGIN_VERSION, "9thCore")]
#endif

namespace SCPlus;

#if (!MELON_LOADER)
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("PlagueIncSC.exe")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    internal static readonly Harmony harmony = new(MyPluginInfo.PLUGIN_GUID);

    public static void LogInfo(object message)
    {
        Logger.LogInfo(message);
    }

    public static void LogError(object message)
    {
        Logger.LogError(message);
    }

    public static void LogWarning(object message)
    {
        Logger.LogWarning(message);
    }

    private void Awake()
    {
        Logger = base.Logger;

        bool x86Version = IntPtr.Size == 4;

#if (!USE_32_COMPAT)
        if (x86Version)
        {
            Logger.LogError($"Cannot start plugin - must use the 32-bit compatibility build on the win32beta branch!");
            return;
        }
#else
        if (!x86Version)
        {
            Logger.LogWarning($"Using the 32-bit compatibility build on the stable branch! Some features will be unavailable, use the 64-bit version of the mod to unlock them.");
        }
#endif

        // Plugin startup logic
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is starting loading...");
        plugin.Config.Awake();

        harmony.PatchAll();
        VariableRegister.Awake();
        VariableUnrestrict.RemoveDiseaseTypes();
        VariableUnrestrict.WidenAccessibility();
        LanguageRegister.Awake();
        LanguageRegister.Init("English");

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} has loaded.");
    }
}
#else
public class Plugin : MelonMod
{
    private static MelonLogger.Instance Logger;
    internal static bool canLoad = true;

    public static void LogInfo(object message)
    {
        Logger.Msg(message);
    }

    public static void LogError(object message)
    {
        Logger.Error(message);
    }

    public static void LogWarning(object message)
    {
        Logger.Warning(message);
    }

    public override void OnEarlyInitializeMelon()
    {
        Logger = this.LoggerInstance;

        bool x86Version = IntPtr.Size == 4;

#if (!USE_32_COMPAT)
        if (x86Version)
        {
            Plugin.LogError($"Cannot start plugin - must use the 32-bit compatibility build on the win32beta branch!");
            canLoad = false;
            return;
        }
#else
        if (!x86Version)
        {
            Plugin.LogWarning($"Using the 32-bit compatibility build on the stable branch! Some features will be unavailable, use the 64-bit version of the mod to unlock them.");
        }
#endif

        LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} - config init");
        plugin.Config.Awake();
        LanguageRegister.Awake();
        LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} - config init done");
    }
}

[HarmonyPatch(typeof(ScenarioCreatorAPI), MethodType.Constructor)]
public static class MelonPatch
{
    public static void Postfix(ScenarioCreatorAPI __instance)
    {
        if (!Plugin.canLoad)
        {
            return;
        }

        ScenarioCreatorAPI.m_instance = __instance;

        Plugin.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} - mod init");

        VariableRegister.Awake();
        VariableUnrestrict.RemoveDiseaseTypes();
        VariableUnrestrict.WidenAccessibility();
        LanguageRegister.Init("English");

        Plugin.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} - mod init done");

        ScenarioCreatorAPI.m_instance = null;
    }
}

#endif