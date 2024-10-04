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
                .DefaultTranslation()
                .Complexity(ComplexityType.SUPER_ADVANCED)
                .AsNumber()
                .Register();

            new VariableBuilder("local_cure_research", "Country")
                .Category(CategoryType.HUMAN_RESPONSE)
                .DefaultTranslation()
                .Complexity(ComplexityType.ADVANCED)
                .Condition()
                // .Outcome() // Seems to be reset every day, so don't let it be settable (for now)
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
        }
    }
}