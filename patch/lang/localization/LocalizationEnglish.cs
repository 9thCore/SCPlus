using SCPlus.patch.variable;
using SCPlus.plugin;

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

            LanguageRegister.RegisterSCPlusVariable(
                LANGUAGE,
                "country_number",
                "Country ID",
                "Numerical ID used to represent the current country.\nUseful for reliably picking a country randomly, by rolling a number between 1 and 58 (and checking equality with this variable).");
        }

        internal static void ExtraFunctionalityTranslations()
        {
            LanguageRegister.RegisterLine(
                LANGUAGE,
                LanguageRegister.GetLocalizationTag(LanguageRegister.LocalizationKey.ExtraFunc, "GlobalToggleTooltip"),
                $"{LanguageRegister.SCPLUS_PREFIX} Show extra functionality available for events.");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                LanguageRegister.GetLocalizationTag(LanguageRegister.LocalizationKey.ExtraFunc, "General"),
                "General Event Settings");

            // LockTech
            LanguageRegister.RegisterSetter(
                LANGUAGE,
                LanguageRegister.LocalizationKey.ExtraFunc,
                "LockTech",
                "Trait Lock & Unlock",
                "Traits that will be locked/unlocked when this event fires.");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                LanguageRegister.GetLocalizationTag(LanguageRegister.LocalizationKey.ExtraFunc, "LockTech_Button"),
                "Configure");

            LanguageRegister.RegisterTechScreen(
                LANGUAGE,
                LanguageRegister.LocalizationKey.ExtraFunc,
                "Lock",
                "Trait Lock Settings",
                new()
                {
                    title = "Trait Lock",
                    instruction = "The following traits will be locked when this event fires.",
                    help = "Existing trait(s) that will be locked."
                },
                new()
                {
                    title = "Trait Unlock",
                    instruction = "The following traits will be unlocked when this event fires.",
                    help = "Existing trait(s) that will be unlocked."
                });

            // RandomTech
            LanguageRegister.RegisterLine(
                LANGUAGE,
                LanguageRegister.GetLocalizationTag(LanguageRegister.LocalizationKey.ExtraFunc, "RandomTech"),
                "Forced Mutation");

            LanguageRegister.RegisterSetter(
                LANGUAGE,
                LanguageRegister.LocalizationKey.ExtraFunc,
                "EvolveRandomTech",
                "Enable",
                "Whether one of the traits selected will be forcibly mutated.\nNote that it will pick one out of the traits that can mutate when the event fires (or none, if none can).");

            LanguageRegister.RegisterSetter(
                LANGUAGE,
                LanguageRegister.LocalizationKey.ExtraFunc,
                "RandomTech",
                "Random Trait List",
                "List of possible traits that will be evolved when this event fires.");

            LanguageRegister.RegisterLine(
                LANGUAGE,
                LanguageRegister.GetLocalizationTag(LanguageRegister.LocalizationKey.ExtraFunc, "RandomTech_Button"),
                "Configure");

            LanguageRegister.RegisterTechScreen(
                LANGUAGE,
                LanguageRegister.LocalizationKey.ExtraFunc,
                "Random",
                "Random Trait List",
                new()
                {
                    title = "Trait List",
                    instruction = "One of the following traits will be evolved when this event fires, if it can.",
                    help = "Possible trait that will be evolved."
                });
        }

        internal static void PerCountryTranslations()
        {
            LanguageRegister.RegisterLine(
                LANGUAGE,
                LanguageRegister.GetLocalizationTag(LanguageRegister.LocalizationKey.PerCountry, "Button"),
                $"{LanguageRegister.SCPLUS_PREFIX} Create \"Per-Country\"");
        }

        internal static void TechExtraFunctionalityTranslations()
        {
            LanguageRegister.RegisterSetter(
                LANGUAGE,
                LanguageRegister.LocalizationKey.TechExtraFunc,
                "EventLocked",
                $"{LanguageRegister.SCPLUS_PREFIX} Event Lock",
                $"Determines whether a trait is event locked on start.\nWill not be visible or evolvable until unlocked through an event.{(Config.expandEventFunctionality.Value ? "\nCheck expanded event functionality for more information." : "\nEnable expanded event functionality for ability through the UI.")}");

            if (Config.replaceTraitRequirement.Value)
            {
                LanguageRegister.RegisterSetter(
                    LANGUAGE,
                    LanguageRegister.LocalizationKey.TechExtraFunc,
                    "Requirement",
                    $"{LanguageRegister.SCPLUS_PREFIX} Requirements",
                    $"Traits that have to be evolved before this trait becomes available.\nCombines the \"And\" and \"Or\" lists.");

                LanguageRegister.RegisterLine(
                    LANGUAGE,
                    LanguageRegister.GetLocalizationTag(LanguageRegister.LocalizationKey.TechExtraFunc, "Requirement_Button"),
                    "Configure");

                LanguageRegister.RegisterTechScreen(
                    LANGUAGE,
                    LanguageRegister.LocalizationKey.TechExtraFunc,
                    "Requirement",
                    "Tech Requirement Configuration",
                    new()
                    {
                        title = "Requires all of",
                        instruction = "Every following trait must be evolved before this trait becomes available.",
                        help = "Required trait before this trait becomes available."
                    },
                    new()
                    {
                        title = "Requires either",
                        instruction = "One of the following traits must be evolved before this trait becomes available.",
                        help = "One of the required traits before this trait becomes available."
                    });

                LanguageRegister.RegisterSetter(
                    LANGUAGE,
                    LanguageRegister.LocalizationKey.TechExtraFunc,
                    "RequirementNot",
                    $"{LanguageRegister.SCPLUS_PREFIX} Inverted Requirements",
                    $"Traits that have to not be evolved before this trait becomes available.\nCombines the \"And\" and \"Or\" lists.");

                LanguageRegister.RegisterLine(
                    LANGUAGE,
                    LanguageRegister.GetLocalizationTag(LanguageRegister.LocalizationKey.TechExtraFunc, "RequirementNot_Button"),
                    "Configure");

                LanguageRegister.RegisterTechScreen(
                    LANGUAGE,
                    LanguageRegister.LocalizationKey.TechExtraFunc,
                    "RequirementNot",
                    "Tech Requirement Configuration",
                    new()
                    {
                        title = "Requires none of",
                        instruction = "Every following trait must not be evolved before this trait becomes available.",
                        help = "Required to be unevolved trait before this trait becomes available."
                    },
                    new()
                    {
                        title = "Requires neither",
                        instruction = "One of the following traits must not be evolved before this trait becomes available.",
                        help = "One of the required traits to be unevolved before this trait becomes available."
                    });
            }
        }

        internal static void PatchedVariableNamesTranslations()
        {
            LanguageRegister.RegisterVariable(
                    LANGUAGE,
                    "Event_Variable_wealthy_country",
                    "Is wealthy country",
                    "Whether the country is wealthy");

            LanguageRegister.RegisterVariable(
                    LANGUAGE,
                    "Event_Variable_poverty_country",
                    "Is poor country",
                    "Whether the country is poor");

            LanguageRegister.RegisterVariable(
                    LANGUAGE,
                    "Event_Variable_urban_country",
                    "Is urban country",
                    "Whether the country is urban");

            LanguageRegister.RegisterVariable(
                    LANGUAGE,
                    "Event_Variable_rural_country",
                    "Is rural country",
                    "Whether the country is rural");

            LanguageRegister.RegisterVariable(
                    LANGUAGE,
                    "Event_Variable_hot_country",
                    "Is hot country",
                    "Whether the country is hot");

            LanguageRegister.RegisterVariable(
                    LANGUAGE,
                    "Event_Variable_cold_country",
                    "Is cold country",
                    "Whether the country is cold");

            LanguageRegister.RegisterVariable(
                    LANGUAGE,
                    "Event_Variable_humid_country",
                    "Is humid country",
                    "Whether the country is humid");

            LanguageRegister.RegisterVariable(
                    LANGUAGE,
                    "Event_Variable_arid_country",
                    "Is arid country",
                    "Whether the country is arid");
        }

        internal static void WidenedAccessVariableTranslations()
        {
            foreach (EventVariable variable in VariableHelpers.widenedAccessVariables)
            {
                LanguageRegister.ModifyLine(
                    LANGUAGE,
                    $"Help_Event_Variable_{variable.variable}",
                    suffix: $"\n{LanguageRegister.SCPLUS_PREFIX} Widened access");
            }
        }
    }
}
