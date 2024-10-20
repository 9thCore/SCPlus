using SCPlus.patch.game_event;

namespace SCPlus.patch.lang.localization
{
    internal static class LocalizationEnglish
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
                "Additional cost of Transmission traits. Increases for every Transmission trait evolved, unless the gene [Trans-Stasis] is used.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "symptom_extra_cost",
                "Symptom extra cost",
                "Additional cost of Symptom traits. Increases for every Symptom trait evolved, unless the gene [Sympto-Stasis] is used.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "ability_extra_cost",
                "Ability extra cost",
                "Additional cost of Ability traits. Increases for every Ability trait evolved, unless the gene [Patho-Stasis] is used.");

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
                "Cost penalty applied when trying to devolve Transmission trait. Increases by [Transmission devolve cost increase] on Transmission trait devolve.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "transmission_devolve_cost_increase",
                "Transmission devolve cost increase",
                "How much the cost penalty increases on Transmission trait devolve. Defaults to 1, or 0 if the gene [Translesion +] is used.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "symptom_devolve_cost",
                "Symptom devolve cost",
                "Cost penalty applied when trying to devolve Symptom trait. Increases by [Symptom devolve cost increase] on Symptom trait devolve.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "symptom_devolve_cost_increase",
                "Symptom devolve cost increase",
                "How much the cost penalty increases on Symptom trait devolve. Defaults to 1, or 0 if the gene [Translesion +] is used.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "ability_devolve_cost",
                "Ability devolve cost",
                "Cost penalty applied when trying to devolve Ability trait. Increases by [Ability devolve cost increase] on Ability trait devolve.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "ability_devolve_cost_increase",
                "Ability devolve cost increase",
                "How much the cost penalty increases on Ability trait devolve. Defaults to 1, or 0 if the gene [Translesion +] is used.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "transmission_random_mutations",
                "Transmission random mutations",
                "Whether Transmission traits can randomly mutate like Symptom traits. False by default, true if the gene [Base Oxidisation] is used.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "ability_random_mutations",
                "Ability random mutations",
                "Whether Ability traits can randomly mutate like Symptom traits. False by default, true if the gene [Base Hydrolysis] is used.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "aa_cost_modifier",
                "AA cost modifier",
                "Extra value applied to Active Ability costs. Positive values decrease cost, negative values increase. Cost cannot go below 1.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "Change_fort_state",
                "Change Fort State",
                "Activate, destroy or remove the Z Com/Templar fort in the country.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "Change_ape_lab_state",
                "Change Ape Lab State",
                "Activate, deactivate, destroy or remove the ape lab in the country.");

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "Change_ape_colony_state",
                "Change Ape Colony State",
                "Activate, destroy or remove the ape colony in the country.");
        }

        internal static void ExtraFunctionalityTranslations()
        {
            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetTranslation(EventHelper.TranslationKey.ExtraFunc, "GlobalToggleTooltip"),
                "Show extra functionality available for events.");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetTranslation(EventHelper.TranslationKey.ExtraFunc, "General"),
                "General Event Settings");

            // LockTech
            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetSetterTranslation(EventHelper.TranslationKey.ExtraFunc, "LockTech"),
                "Trait Lock & Unlock");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetSetterHelpTranslation(EventHelper.TranslationKey.ExtraFunc, "LockTech"),
                "Traits that will be locked/unlocked when this event fires.");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetTranslation(EventHelper.TranslationKey.ExtraFunc, "LockTech_Button"),
                "Configure");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetTranslation(EventHelper.TranslationKey.ExtraFunc, "TechScreen_Lock"),
                "Trait Lock Settings");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetTranslation(EventHelper.TranslationKey.ExtraFunc, "TechScreen_Title_Lock"),
                "Trait Lock");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetTranslation(EventHelper.TranslationKey.ExtraFunc, "TechScreen_Instruction_Lock"),
                "The following traits will be locked when this event fires.");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetTranslation(EventHelper.TranslationKey.ExtraFunc, "TechScreen_Help_Lock"),
                "Existing trait(s) that will be locked.");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetTranslation(EventHelper.TranslationKey.ExtraFunc, "TechScreen_Title_Not_Lock"),
                "Trait Unlock");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetTranslation(EventHelper.TranslationKey.ExtraFunc, "TechScreen_Instruction_Not_Lock"),
                "The following traits will be unlocked when this event fires.");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetTranslation(EventHelper.TranslationKey.ExtraFunc, "TechScreen_Help_Not_Lock"),
                "Existing trait(s) that will be unlocked.");

            // RandomTech
            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetTranslation(EventHelper.TranslationKey.ExtraFunc, "RandomTech"),
                "Forced Mutation");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetSetterTranslation(EventHelper.TranslationKey.ExtraFunc, "EvolveRandomTech"),
                "Enable");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetSetterHelpTranslation(EventHelper.TranslationKey.ExtraFunc, "EvolveRandomTech"),
                "Whether one of the traits selected will be forcibly mutated.\nNote that it will pick one out of the traits that can mutate when the event fires (or none, if none can).");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetSetterTranslation(EventHelper.TranslationKey.ExtraFunc, "RandomTech"),
                "Random Trait List");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetSetterHelpTranslation(EventHelper.TranslationKey.ExtraFunc, "RandomTech"),
                "List of possible traits that will be evolved when this event fires.");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetTranslation(EventHelper.TranslationKey.ExtraFunc, "RandomTech_Button"),
                "Configure");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetTranslation(EventHelper.TranslationKey.ExtraFunc, "TechScreen_Random"),
                "Random Trait List");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetTranslation(EventHelper.TranslationKey.ExtraFunc, "TechScreen_Title_Random"),
                "Trait List");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetTranslation(EventHelper.TranslationKey.ExtraFunc, "TechScreen_Instruction_Random"),
                "One of the following traits will be evolved when this event fires, if it can.");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                EventHelper.GetTranslation(EventHelper.TranslationKey.ExtraFunc, "TechScreen_Help_Random"),
                "Possible trait that will be evolved.");
        }
    }
}
