using System;

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
        
        internal VariableBuilder CategoryCustom()
        {
            return Category("Custom");
        }

        internal VariableBuilder CategoryHumanResponse()
        {
            return Category("Human Response");
        }

        internal VariableBuilder Category(string category)
        {
            eventVariable.category = category;
            return this;
        }

        internal VariableBuilder ComplexityBasic()
        {
            return Complexity("Basic");
        }

        internal VariableBuilder ComplexityAdvanced()
        {
            return Complexity("Advanced");
        }

        internal VariableBuilder ComplexitySuperAdvanced()
        {
            return Complexity("Super Advanced");
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

        internal void Register()
        {
            VariableHelpers.RegisterVariable(eventVariable);
        }

        internal VariableBuilder Complexity(string complexity)
        {
            eventVariable.complexity = complexity;
            return this;
        }
    }
}
