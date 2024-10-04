using HarmonyLib;
using SCPlus.plugin;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace SCPlus.patch.lang
{
    [HarmonyPatch(typeof(CLocalisationManager), nameof(CLocalisationManager.GetText))]
    internal class TranslationUnlowercase
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            if (!Config.patchTranslationLowercasing.Value)
            {
                return codes.AsEnumerable();
            }

            for (int i = 0; i < codes.Count - 2; i++)
            {
                CodeInstruction code1 = codes[i];
                CodeInstruction code2 = codes[i + 1];
                CodeInstruction code3 = codes[i + 2];

                // look for these three opcodes one after the other: these load the first argument, second argument and the string "English"
                if (code1.opcode == OpCodes.Ldarg_0
                && code2.opcode == OpCodes.Ldarg_1
                && code3.opcode == OpCodes.Ldstr
                && (string)code3.operand == "English")
                {
                    // we just have to change the first to load a local variable, 'result', instead! 
                    code1.opcode = OpCodes.Ldloc_1;
                }
            }

            return codes.AsEnumerable();
        }
    }
}
