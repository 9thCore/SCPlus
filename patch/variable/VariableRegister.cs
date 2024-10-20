using SCPlus.plugin;
using static SCPlus.patch.variable.VariableBuilder;

namespace SCPlus.patch.variable
{
    internal class VariableRegister
    {
        internal static void Awake()
        {
            if (!Config.exposeMoreVariables.Value)
            {
                return;
            }

            new VariableBuilder("custom_global_variable_6", "World")
                .Category(CategoryType.CUSTOM)
                .DefaultSCPlusTranslation()
                .Complexity(ComplexityType.SUPER_ADVANCED)
                .AsNumber()
                .Register();

            new VariableBuilder("local_cure_research", "Country")
                .Category(CategoryType.HUMAN_RESPONSE)
                .DefaultSCPlusTranslation()
                .Complexity(ComplexityType.ADVANCED)
                .Condition()
                .Outcome()
                .Expression()
                .Register();

            new VariableBuilder("transmission_extra_cost", "World")
                .Category(CategoryType.DISEASE_STATS)
                .DefaultSCPlusTranslation()
                .Complexity(ComplexityType.ADVANCED)
                .AsNumber()
                .Register();

            new VariableBuilder("symptom_extra_cost", "World")
                .Category(CategoryType.DISEASE_STATS)
                .DefaultSCPlusTranslation()
                .Complexity(ComplexityType.ADVANCED)
                .AsNumber()
                .Register();

            new VariableBuilder("ability_extra_cost", "World")
                .Category(CategoryType.DISEASE_STATS)
                .DefaultSCPlusTranslation()
                .Complexity(ComplexityType.ADVANCED)
                .AsNumber()
                .Register();

            new VariableBuilder("infected_this_turn", "World")
                .Category(CategoryType.POPULATION)
                .DefaultSCPlusTranslation()
                .Complexity(ComplexityType.ADVANCED)
                .Condition()
                .Expression()
                .Register();

            new VariableBuilder("dead_this_turn", "World")
                .Category(CategoryType.POPULATION)
                .DefaultSCPlusTranslation()
                .Complexity(ComplexityType.ADVANCED)
                .Condition()
                .Expression()
                .Register();

            new VariableBuilder("zombies_this_turn", "World")
                .Category(CategoryType.ZOMBIE)
                .DefaultSCPlusTranslation()
                .Complexity(ComplexityType.ADVANCED)
                .Condition()
                .Expression()
                .Register();

            new VariableBuilder("infected_apes_this_turn", "World")
                .Category(CategoryType.APE)
                .DefaultSCPlusTranslation()
                .Complexity(ComplexityType.ADVANCED)
                .Condition()
                .Expression()
                .Register();

            new VariableBuilder("transmission_devolve_cost", "World")
                .Category(CategoryType.GENERAL)
                .DefaultSCPlusTranslation()
                .Complexity(ComplexityType.BASIC)
                .AsNumber()
                .Register();

            new VariableBuilder("transmission_devolve_cost_increase", "World")
                .Category(CategoryType.GENERAL)
                .DefaultSCPlusTranslation()
                .Complexity(ComplexityType.BASIC)
                .AsNumber()
                .Register();

            new VariableBuilder("symptom_devolve_cost", "World")
                .Category(CategoryType.GENERAL)
                .DefaultSCPlusTranslation()
                .Complexity(ComplexityType.BASIC)
                .AsNumber()
                .Register();

            new VariableBuilder("symptom_devolve_cost_increase", "World")
                .Category(CategoryType.GENERAL)
                .DefaultSCPlusTranslation()
                .Complexity(ComplexityType.BASIC)
                .AsNumber()
                .Register();

            new VariableBuilder("ability_devolve_cost", "World")
                .Category(CategoryType.GENERAL)
                .DefaultSCPlusTranslation()
                .Complexity(ComplexityType.BASIC)
                .AsNumber()
                .Register();

            new VariableBuilder("ability_devolve_cost_increase", "World")
                .Category(CategoryType.GENERAL)
                .DefaultSCPlusTranslation()
                .Complexity(ComplexityType.BASIC)
                .AsNumber()
                .Register();

            new VariableBuilder("transmission_random_mutations", "World")
                .Category(CategoryType.DISEASE_EFFECTS)
                .DefaultSCPlusTranslation()
                .Complexity(ComplexityType.ADVANCED)
                .AsBoolean(BoolTypes.UI_True | BoolTypes.UI_False)
                .Register();

            new VariableBuilder("ability_random_mutations", "World")
                .Category(CategoryType.DISEASE_EFFECTS)
                .DefaultSCPlusTranslation()
                .Complexity(ComplexityType.ADVANCED)
                .AsBoolean(BoolTypes.UI_True | BoolTypes.UI_False)
                .Register();

            new VariableBuilder("aa_cost_modifier", "World")
                .Category(CategoryType.DISEASE_STATS)
                .DefaultSCPlusTranslation()
                .Complexity(ComplexityType.ADVANCED)
                .AsNumber()
                .Register();

            new VariableBuilder("Change_fort_state", "Country")
                .Category(CategoryType.GENERAL)
                .DefaultSCPlusTranslation()
                .Complexity(ComplexityType.ADVANCED)
                .AsEnum(typeof(EFortState))
                .Register();

            new VariableBuilder("Change_ape_lab_state", "Country")
                .Category(CategoryType.APE)
                .DefaultSCPlusTranslation()
                .Complexity(ComplexityType.ADVANCED)
                .AsEnum(typeof(EApeLabState))
                .Register();

            new VariableBuilder("Change_ape_colony_state", "Country")
                .Category(CategoryType.APE)
                .DefaultSCPlusTranslation()
                .Complexity(ComplexityType.ADVANCED)
                .AsEnum(typeof(EApeColonyState))
                .Register();
        }
    }
}