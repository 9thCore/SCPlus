using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SCPlus.patch.lang;
using SCPlus.patch.variable;
using System;

namespace SCPlus;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("PlagueIncSC.exe")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    internal static readonly Harmony harmony = new(MyPluginInfo.PLUGIN_GUID);
        
    private void Awake()
    {
        Logger = base.Logger;

        bool x86Version = IntPtr.Size == 4;

#if(!USE_32_COMPAT)
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
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        plugin.Config.Awake();

        harmony.PatchAll();
        VariableRegister.Awake();
        VariableUnrestrict.RemoveDiseaseTypes();
        VariableUnrestrict.WidenAccessibility();
        LanguageRegister.Awake();
    }
}
