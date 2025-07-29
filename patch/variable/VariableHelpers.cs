using SCPlus.plugin;
using System;
using System.Collections.Generic;

namespace SCPlus.patch.variable
{
    internal static class VariableHelpers
    {
        internal static List<EventVariable> widenedAccessVariables = [];

        // Update a variable by registering it in the correct dictionaries
        internal static void UpdateVariable(EventVariable variable)
        {
            ScenarioCreatorAPI instance = ScenarioCreatorAPI.Instance;
            string key = GetFullVariableKey(variable);

            if (variable.outcome > 0)
            {
                instance.variableDataOutcome[key] = variable;
            }

            if (variable.condition > 0)
            {
                instance.variableDataCondLocal[key] = variable;
                if (variable.file != "Country")
                {
                    instance.variableDataCondGlobal[key] = variable;
                }
            }

            if (variable.expression > 0)
            {
                instance.variableDataExpression[key] = variable;
            }
        }

        internal static void RegisterVariable(EventVariable variable)
        {
            ScenarioCreatorAPI instance = ScenarioCreatorAPI.Instance;
            string fullKey = GetFullVariableKey(variable);
            string key = GetVariableKey(variable);

            if (!instance.variableDataSorted.ContainsKey(fullKey) || !instance.variableDataSorted.ContainsKey(key))
            {
                instance.variableDataSorted[fullKey] = variable;
                instance.variableDataSorted[key] = variable;
                
                // Perform insertion sort on sorted event list
                int i = 0;

                // Find first instance of variable with the same category
                while (i < instance.sortedEventVariables.Count)
                {
                    if (instance.sortedEventVariables[i].category == variable.category)
                    {
                        break;
                    }
                    i++;
                }

                // Find where to insert
                while (i < instance.sortedEventVariables.Count)
                {
                    if (instance.sortedEventVariables[i].category != variable.category
                        || instance.sortedEventVariables[i].id.CompareTo(variable.id) > 0)
                    {
                        break;
                    }
                    i++;
                }

                instance.sortedEventVariables.Insert(i, variable);
            }
            UpdateVariable(variable);
        }

        internal static string GetVariableKey(EventVariable variable)
        {
            return $"{variable.CamelCaseID}/{variable.ReflectionTarget}";
        }

        internal static string GetFullVariableKey(EventVariable variable)
        {
            return variable.diseaseType.Length > 0 ? $"{GetVariableKey(variable)}/{variable.diseaseType}" : GetVariableKey(variable);
        }

        internal static void WidenAccess(string name, File file, AccessModifier modifier)
        {
            EventVariable variable = ScenarioCreatorAPI.Instance.GetEventVariable(name, file.ToString());
            if (variable == null)
            {
                return;
            }

            variable.condition = modifier.HasFlag(AccessModifier.CONDITION) ? 1 : variable.condition;
            variable.expression = modifier.HasFlag(AccessModifier.EXPRESSION) ? 1 : variable.expression;
            variable.outcome = modifier.HasFlag(AccessModifier.OUTCOME) ? 1 : variable.outcome;

            if (modifier != 0)
            {
                UpdateVariable(variable);
                widenedAccessVariables.Add(variable);
            }
        }

        internal enum File
        {
            LOCALDISEASE,
            COUNTRY,
            DISEASE
        }

        [Flags]
        internal enum AccessModifier
        {
            CONDITION = 1,
            EXPRESSION = 2,
            OUTCOME = 4
        }
    }
}
