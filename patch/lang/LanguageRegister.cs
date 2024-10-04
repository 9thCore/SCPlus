using SCPlus.plugin;
using System;

namespace SCPlus.patch.lang
{
    internal class LanguageRegister
    {
        internal static void Awake()
        {
            if (Config.exposeMoreVariables.Value)
            {
                RegisterSCPlusVariable(
                    "transmission_extra_cost",
                    "Transmission Tech Extra Cost",
                    "Additional cost of Transmission technology. Increases for every Transmission technology evolved, unless the gene Trans-Stasis is used.");

                RegisterSCPlusVariable(
                    "symptom_extra_cost",
                    "Symptom Tech Extra Cost",
                    "Additional cost of Symptom technology. Increases for every Symptom technology evolved, unless the gene Sympto-Stasis is used.");

                RegisterSCPlusVariable(
                    "ability_extra_cost",
                    "Ability Tech Extra Cost",
                    "Additional cost of Ability technology. Increases for every Ability technology evolved, unless the gene Patho-Stasis is used.");

                RegisterSCPlusVariable(
                    "infected_this_turn",
                    "Infected last turn",
                    "Number of new infected humans last turn (one turn behind Daily Infections in Disease Overview).");

                RegisterSCPlusVariable(
                    "dead_this_turn",
                    "Dead last turn",
                    "Number of new dead humans last turn (one turn behind Daily Deaths in Disease Overview).");

                RegisterSCPlusVariable(
                    "zombies_this_turn",
                    "Zombies last turn",
                    "Number of new zombies last turn.");

                RegisterSCPlusVariable(
                    "infected_apes_this_turn",
                    "Infected apes last turn",
                    "Number of new infected apes last turn.");
            }
        }

        internal static void RegisterSCPlusVariable(string commonSuffix, string name, string tooltip, string language = CLocalisationManager.FALLBACK_LANGUAGE)
        {
            RegisterVariable($"SCPlus_Event_Variable_{commonSuffix}", name, tooltip, language);
        }

        internal static void RegisterSCPlusLine(string suffix, string line, string language = CLocalisationManager.FALLBACK_LANGUAGE)
        {
            RegisterLine($"SCPlus_{suffix}", line, language);
        }

        internal static void RegisterVariable(string commonSuffix, string name, string tooltip, string language = CLocalisationManager.FALLBACK_LANGUAGE)
        {
            RegisterLine($"UI_{commonSuffix}", $"[+] {name}", language);
            RegisterLine($"Help_{commonSuffix}", tooltip, language);
        }

        internal static void RegisterLine(string tag, string line, string language)
        {
            if (!CLocalisationManager.LanguageExists(language) || !CLocalisationManager.mpLocalisedTexts.ContainsKey(language))
            {
                throw new Exception($"Invalid language {language}");
            }

            CLocalisationManager.mpLocalisedTexts[language].Add(tag, line);
            CLocalisationManager.mpLocalisedTexts[language].Add(tag.ToLower(), line);
        }
    }
}
