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
                    "Transmission extra cost",
                    "Additional cost of Transmission tech. Increases for every Transmission tech evolved, unless the gene [Trans-Stasis] is used.");

                RegisterSCPlusVariable(
                    "symptom_extra_cost",
                    "Symptom extra cost",
                    "Additional cost of Symptom tech. Increases for every Symptom tech evolved, unless the gene [Sympto-Stasis] is used.");

                RegisterSCPlusVariable(
                    "ability_extra_cost",
                    "Ability extra cost",
                    "Additional cost of Ability tech. Increases for every Ability tech evolved, unless the gene [Patho-Stasis] is used.");

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

                RegisterSCPlusVariable(
                    "transmission_devolve_cost",
                    "Transmission devolve cost",
                    "Cost penalty applied when trying to devolve Transmission tech. Increases by [Transmission devolve cost increase] on Transmission tech devolve.");

                RegisterSCPlusVariable(
                    "transmission_devolve_cost_increase",
                    "Transmission devolve cost increase",
                    "How much the cost penalty increases on Transmission tech devolve. Defaults to 1, or 0 if the gene [Translesion +] is used.");

                RegisterSCPlusVariable(
                    "symptom_devolve_cost",
                    "Symptom devolve cost",
                    "Cost penalty applied when trying to devolve Symptom tech. Increases by [Symptom devolve cost increase] on Symptom tech devolve.");

                RegisterSCPlusVariable(
                    "symptom_devolve_cost_increase",
                    "Symptom devolve cost increase",
                    "How much the cost penalty increases on Symptom tech devolve. Defaults to 1, or 0 if the gene [Translesion +] is used.");

                RegisterSCPlusVariable(
                    "ability_devolve_cost",
                    "Ability devolve cost",
                    "Cost penalty applied when trying to devolve Ability tech. Increases by [Ability devolve cost increase] on Ability tech devolve.");

                RegisterSCPlusVariable(
                    "ability_devolve_cost_increase",
                    "Ability devolve cost increase",
                    "How much the cost penalty increases on Ability tech devolve. Defaults to 1, or 0 if the gene [Translesion +] is used.");
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
