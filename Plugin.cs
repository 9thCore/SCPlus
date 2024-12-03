using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SCPlus.patch.lang;
using SCPlus.patch.variable;

namespace SCPlus;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("PlagueIncSC.exe")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    internal static readonly Harmony harmony = new(MyPluginInfo.PLUGIN_GUID);
        
    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        plugin.Config.Awake();

        harmony.PatchAll();
        VariableRegister.Awake();
        VariableUnrestrict.RemoveDiseaseTypes();
        VariableUnrestrict.WidenAccessibility();
        LanguageRegister.Awake();
    }
}
