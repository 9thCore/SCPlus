namespace SCPlus.patch.lang.localization
{
    internal class LocalizationEnglish
    {
        private static readonly string LANGUAGE = "English";

        internal static void MoreVariableTranslations()
        {
            LanguageRegister.RegisterSCPlusVariableFromExisting(
                LANGUAGE,
                "custom_global_variable_6");

            LanguageRegister.RegisterSCPlusVariableFromExisting(
                LANGUAGE,
                "local_cure_research",
                tooltipSuffix: $"\n{LanguageRegister.SCPLUS_PREFIX} Gets reset every turn, changes will not be kept");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "transmission_extra_cost",
                "Transmission extra cost",
                "Additional cost of Transmission tech. Increases for every Transmission tech evolved, unless the gene [Trans-Stasis] is used.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "symptom_extra_cost",
                "Symptom extra cost",
                "Additional cost of Symptom tech. Increases for every Symptom tech evolved, unless the gene [Sympto-Stasis] is used.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "ability_extra_cost",
                "Ability extra cost",
                "Additional cost of Ability tech. Increases for every Ability tech evolved, unless the gene [Patho-Stasis] is used.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "infected_this_turn",
                "Infected last turn",
                "Number of new infected humans last turn (one turn behind Daily Infections in Disease Overview).");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "dead_this_turn",
                "Dead last turn",
                "Number of new dead humans last turn (one turn behind Daily Deaths in Disease Overview).");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "zombies_this_turn",
                "Zombies last turn",
                "Number of new zombies last turn.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "infected_apes_this_turn",
                "Infected apes last turn",
                "Number of new infected apes last turn.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "transmission_devolve_cost",
                "Transmission devolve cost",
                "Cost penalty applied when trying to devolve Transmission tech. Increases by [Transmission devolve cost increase] on Transmission tech devolve.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "transmission_devolve_cost_increase",
                "Transmission devolve cost increase",
                "How much the cost penalty increases on Transmission tech devolve. Defaults to 1, or 0 if the gene [Translesion +] is used.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "symptom_devolve_cost",
                "Symptom devolve cost",
                "Cost penalty applied when trying to devolve Symptom tech. Increases by [Symptom devolve cost increase] on Symptom tech devolve.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "symptom_devolve_cost_increase",
                "Symptom devolve cost increase",
                "How much the cost penalty increases on Symptom tech devolve. Defaults to 1, or 0 if the gene [Translesion +] is used.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "ability_devolve_cost",
                "Ability devolve cost",
                "Cost penalty applied when trying to devolve Ability tech. Increases by [Ability devolve cost increase] on Ability tech devolve.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "ability_devolve_cost_increase",
                "Ability devolve cost increase",
                "How much the cost penalty increases on Ability tech devolve. Defaults to 1, or 0 if the gene [Translesion +] is used.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "transmission_random_mutations",
                "Transmission random mutations",
                "Whether Transmission tech can randomly mutate like Symptom tech. False by default, true if the gene [Base Oxidisation] is used.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "ability_random_mutations",
                "Ability random mutations",
                "Whether Ability tech can randomly mutate like Symptom tech. False by default, true if the gene [Base Hydrolysis] is used.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "aa_cost_modifier",
                "AA cost modifier",
                "Extra value applied to Active Ability costs. Positive values decrease cost, negative values increase. Cost cannot go below 1.");
        }
    }
}
