namespace SCPlus.patch.variable
{
    internal static class VariableHelpers
    {
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
    }
}
