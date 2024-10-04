using System;
using System.Linq;

namespace SCPlus.patch.variable
{
    internal class VariableBuilder
    {
        private readonly EventVariable eventVariable = new();

        internal VariableBuilder(string variable, string file)
        {
            eventVariable.id = $"{variable}_{file}";
            eventVariable.variable = variable;
            eventVariable.file = file;
            eventVariable.iosMap = variable;
            eventVariable.condition = 0;
            eventVariable.outcome = 0;
            eventVariable.expression = 0;
            eventVariable.diseaseType = "";
            eventVariable.include = 1;
            eventVariable.appearance = "";
            eventVariable.conditionOperations = "";
            eventVariable.outcomeNoExpression = "";
            eventVariable.outcomeOperations = "";
            eventVariable.outcomeToggleDescription = "";
            eventVariable.outcomeListData = "";
            eventVariable.lastTranslation = "";
        }

        internal VariableBuilder Translation(string variable, string tooltip)
        {
            eventVariable.variableString = variable;
            eventVariable.tooltip = tooltip;
            return this;
        }

        internal VariableBuilder Translation(string translation)
        {
            return Translation($"UI_{translation}", $"Help_{translation}");
        }

        internal VariableBuilder DefaultTranslation()
        {
            return Translation($"Event_Variable_{eventVariable.variable}");
        }

        internal VariableBuilder Category(CategoryType type)
        {
            return type switch
            {
                CategoryType.CUSTOM => Category("Custom"),
                CategoryType.HUMAN_RESPONSE => Category("Human Response"),
                _ => throw new Exception($"Invalid CategoryType {type}"),
            };
        }

        internal VariableBuilder Category(string category)
        {
            eventVariable.category = category;
            return this;
        }

        internal VariableBuilder Complexity(ComplexityType type)
        {
            return type switch
            {
                ComplexityType.BASIC => Complexity("Basic"),
                ComplexityType.ADVANCED => Complexity("Advanced"),
                ComplexityType.SUPER_ADVANCED => Complexity("Super Advanced"),
                _ => throw new Exception($"Invalid ComplexityType {type}"),
            };
        }

        internal VariableBuilder Complexity(string complexity)
        {
            eventVariable.complexity = complexity;
            return this;
        }

        internal VariableBuilder Condition()
        {
            eventVariable.condition = 1;
            return this;
        }

        internal VariableBuilder Outcome()
        {
            eventVariable.outcome = 1;
            return this;
        }

        internal VariableBuilder Expression()
        {
            eventVariable.expression = 1;
            return this;
        }

        internal VariableBuilder AsNumber()
        {
            return Condition().Outcome().Expression();
        }

        internal void Register()
        {
            VariableHelpers.RegisterVariable(eventVariable);
        }

        internal enum CategoryType
        {
            CUSTOM,
            HUMAN_RESPONSE
        }

        internal enum ComplexityType
        {
            BASIC,
            ADVANCED,
            SUPER_ADVANCED
        }
    }
}
