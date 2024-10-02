using System;
using System.Runtime.CompilerServices;

namespace SCPlus.patch.variable
{
    internal class VariableHelpers
    {
        // Update a variable by registering it in the correct dictionaries
        internal static void UpdateVariable(EventVariable variable)
        {
            ScenarioCreatorAPI instance = ScenarioCreatorAPI.Instance;
            string key = GetVariableKey(variable);

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
            string key = GetVariableKey(variable);

            if (!instance.variableDataSorted.ContainsKey(key))
            {
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

                Plugin.Logger.LogInfo($"First event: {instance.sortedEventVariables[i]}, at {i}");

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

                Plugin.Logger.LogInfo($"Found event: {instance.sortedEventVariables[i]}, at {i}");

                if (i >= instance.sortedEventVariables.Count
                    || instance.sortedEventVariables[i].category != variable.category)
                {
                    throw new Exception($"Could not insert variable {variable} into sorted event list");
                }

                instance.sortedEventVariables.Insert(i, variable);

                Plugin.Logger.LogInfo("New list");
                Plugin.Logger.LogInfo(instance.sortedEventVariables);
            }
            UpdateVariable(variable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string GetVariableKey(EventVariable variable)
        {
            return string.Concat(
            [
                variable.CamelCaseID,
                "/",
                variable.ReflectionTarget.ToString(),
                "/",
                variable.diseaseType
            ]);
        }
    }
}
