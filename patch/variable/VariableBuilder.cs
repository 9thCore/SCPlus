using SCPlus.patch.lang;
using System;
using System.Collections.Generic;
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

        internal VariableBuilder DefaultSCPlusTranslation()
        {
            return Translation($"{LanguageRegister.SCPLUS_TRANSLATION_KEY}_Event_Variable_{eventVariable.variable}");
        }

        internal VariableBuilder Category(CategoryType type)
        {
            return type switch
            {
                CategoryType.CUSTOM => Category("Custom"),
                CategoryType.HUMAN_RESPONSE => Category("Human Response"),
                CategoryType.DISEASE_STATS => Category("Disease Stats"),
                CategoryType.POPULATION => Category("Population"),
                CategoryType.APE => Category("Ape"),
                CategoryType.ZOMBIE => Category("Zombie"),
                CategoryType.GENERAL => Category("General"),
                CategoryType.DISEASE_EFFECTS => Category("Disease Effects"),
                _ => this,
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
                _ => this,
            };
        }

        internal VariableBuilder Complexity(string complexity)
        {
            eventVariable.complexity = complexity;
            return this;
        }

        internal VariableBuilder Condition(ConditionOps operations)
        {
            eventVariable.conditionOperations = JoinEnumFlags(typeof(ConditionOps), operations);
            return Condition();
        }

        internal VariableBuilder Condition()
        {
            eventVariable.condition = 1;
            return this;
        }

        internal VariableBuilder Outcome(OutcomeOps operators)
        {
            eventVariable.outcomeOperations = JoinEnumFlags(typeof(OutcomeOps), operators);
            return Outcome();
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

        internal VariableBuilder AsBoolean(BoolTypes types)
        {
            eventVariable.outcomeToggleDescription = JoinEnumFlags(typeof(BoolTypes), types);
            return Condition(ConditionOps.EQUAL | ConditionOps.NOT_EQUAL).Outcome(OutcomeOps.SET).Appearance(AppearanceType.BOOL);
        }

        internal VariableBuilder AsCheckableEnum(Type enumType)
        {
            return AsEnum(enumType).Condition(ConditionOps.EQUAL | ConditionOps.NOT_EQUAL);
        }

        internal VariableBuilder AsEnum(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                Plugin.Logger.LogError($"{enumType} is not an enum");
                return this;
            }

            eventVariable.outcomeListData = GetEnumName(enumType);
            return Outcome(OutcomeOps.SET);
        }

        internal VariableBuilder Appearance(AppearanceType type)
        {
            return type switch
            {
                AppearanceType.BOOL => Appearance("bool"),
                _ => this,
            };
        }

        internal VariableBuilder Appearance(string apperance)
        {
            eventVariable.appearance = apperance;
            return this;
        }

        internal void Register()
        {
            VariableHelpers.RegisterVariable(eventVariable);
        }

        private static string JoinEnumFlags(Type enumType, Enum enumValue)
        {
            if (!enumType.IsEnum)
            {
                return "";
            }

            Array allValues = enumType.GetEnumValues();
            List<string> values = [];

            foreach (Enum item in allValues)
            {
                if (enumValue.HasFlag(item))
                {
                    values.Add(item.ToString());
                }
            }

            return String.Join(",", values);
        }

        private static string GetEnumName(Type enumType)
        {
            if (enumType.ReflectedType == null)
            {
                return enumType.Name;
            }
            return $"{enumType.ReflectedType}.{enumType.Name}";
        }

        internal enum CategoryType
        {
            CUSTOM,
            HUMAN_RESPONSE,
            DISEASE_STATS,
            POPULATION,
            APE,
            ZOMBIE,
            GENERAL,
            DISEASE_EFFECTS
        }

        internal enum ComplexityType
        {
            BASIC,
            ADVANCED,
            SUPER_ADVANCED
        }

        internal enum AppearanceType
        {
            BOOL
        }

        // Violates naming convention, but this is the "Plague Inc." format regarding booleans
        // and it's easier to write to variable directly than have a middleman convert them :p
        [Flags]
        internal enum BoolTypes
        {
            UI_True = 1,
            UI_False = 2,
            UI_On = 4,
            UI_Off = 8
        }

        [Flags]
        internal enum ConditionOps
        {
            EQUAL = 1,
            NOT_EQUAL = 2
        }

        [Flags]
        internal enum OutcomeOps
        {
            SET = 1
        }
    }
}
