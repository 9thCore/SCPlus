#if (!MELON_LOADER)
using BepInEx.Configuration;
using BepInEx;
#else
using AnimationOrTween;
using MelonLoader;
#endif

using System.IO;

namespace SCPlus.plugin
{
    internal class Config
    {
        public const string CATEGORY_QOL = "QOL";
        public const string CATEGORY_VARIABLE = "Variable";
        public const string CATEGORY_EVENT = "Event";
        public const string CATEGORY_TRAIT = "Trait";

        public const string PATCH_TRANSLATION_LOWERCASING_DESC = "Patch translation code to return un-lowercased text, if the string is not localised.";
        public const string DISEASE_SHAPE_NAME_FIX_DESC = "Patch the \"Disease shape\" setting to be ordered like basegame plague type ordering and remove the erronous \"empty\" setting.";
        public const string PATCH_VARIABLE_NAME_DESC = "Patch variable names such that they're accurate to their effect.\nMay cause some variables to lose translations in languages other than English.";
        public const string LIFT_VARIABLE_TYPE_CHECK_DESC = "Removes the checks for current disease type related to variables.";
        public const string EXPOSE_MORE_VARIABLES_DESC = "Expose variables to the Scenario Creator that are not usually accessible. Not all may be settable.";
        public const string WIDEN_VARIABLE_ACCESSIBILITY_DESC = "Widen the accessibility of some variables.";
        public const string DESCRIBE_VARIABLE_TYPE_DESC = "Mention the type of the variable chosen. Useful if an integer is to be desired (for, say, floor-ing or such)";
        public const string EXPAND_EVENT_FUNCTIONALITY_DESC = "Expand possibilities with an event, exposing inaccessible functionality, even through outcomes.";
        public const string PER_COUNTRY_EVENT_FUNCTIONALITY_DESC = "Allow creation of a special \"per-country\" event type, which will run for every country.";
        public const string EXPAND_TRAIT_FUNCTIONALITY_DESC = $"Expand possibilities with a trait, exposing inaccessible functionality. Synergies with {nameof(expandEventFunctionality)} (Trait Lock)";
        public const string REPLACE_TRAIT_REQUIREMENT_DESC = $"Subtype of {nameof(expandTraitFunctionality)} (will not work without), replaces trait requirement selection with custom implementation and adds \"not\" variants (allows more than 3 traits per type)";

#if (!MELON_LOADER)
        internal static readonly ConfigFile config = new(Path.Combine(Paths.ConfigPath, "SCPlus.cfg"), true);
        internal static ConfigEntry<bool> patchTranslationLowercasing;
        internal static ConfigEntry<bool> diseaseShapeNameFix;
        internal static ConfigEntry<bool> patchVariableName;
        internal static ConfigEntry<bool> liftVariableTypeCheck;
        internal static ConfigEntry<bool> exposeMoreVariables;
        internal static ConfigEntry<bool> widenVariableAccessibility;
        internal static ConfigEntry<bool> describeVariableType;
        internal static ConfigEntry<bool> expandEventFunctionality;
        internal static ConfigEntry<bool> perCountryEventFunctionality;
        internal static ConfigEntry<bool> expandTraitFunctionality;
        internal static ConfigEntry<bool> replaceTraitRequirement;

        internal static void Awake()
        {
            patchTranslationLowercasing = config.Bind(
                CATEGORY_QOL,
                nameof(patchTranslationLowercasing),
                true,
                PATCH_TRANSLATION_LOWERCASING_DESC);

            diseaseShapeNameFix = config.Bind(
                CATEGORY_QOL,
                nameof(diseaseShapeNameFix),
                true,
                DISEASE_SHAPE_NAME_FIX_DESC);

            patchVariableName = config.Bind(
                CATEGORY_QOL,
                nameof(patchVariableName),
                true,
                PATCH_VARIABLE_NAME_DESC);

            liftVariableTypeCheck = config.Bind(
                CATEGORY_VARIABLE,
                nameof(liftVariableTypeCheck),
                true,
                LIFT_VARIABLE_TYPE_CHECK_DESC);

            exposeMoreVariables = config.Bind(
                CATEGORY_VARIABLE,
                nameof(exposeMoreVariables),
                true,
                EXPOSE_MORE_VARIABLES_DESC);

            widenVariableAccessibility = config.Bind(
                CATEGORY_VARIABLE,
                nameof(widenVariableAccessibility),
                true,
                WIDEN_VARIABLE_ACCESSIBILITY_DESC);

            describeVariableType = config.Bind(
                CATEGORY_VARIABLE,
                nameof(describeVariableType),
                true,
                DESCRIBE_VARIABLE_TYPE_DESC);

            expandEventFunctionality = config.Bind(
                CATEGORY_EVENT,
                nameof(expandEventFunctionality),
                true,
                EXPAND_EVENT_FUNCTIONALITY_DESC);

            perCountryEventFunctionality = config.Bind(
                CATEGORY_EVENT,
                nameof(perCountryEventFunctionality),
                false,
                PER_COUNTRY_EVENT_FUNCTIONALITY_DESC);

            expandTraitFunctionality = config.Bind(
                CATEGORY_TRAIT,
                nameof(expandTraitFunctionality),
                true,
                EXPAND_TRAIT_FUNCTIONALITY_DESC);

            replaceTraitRequirement = config.Bind(
                CATEGORY_TRAIT,
                nameof(replaceTraitRequirement),
                true,
                REPLACE_TRAIT_REQUIREMENT_DESC);
        }
#else
        private static MelonPreferences_Category CategoryQOL;
        private static MelonPreferences_Category CategoryVariable;
        private static MelonPreferences_Category CategoryEvent;
        private static MelonPreferences_Category CategoryTrait;

        //internal static IFakeConfigEntry<bool> patchTranslationLowercasing;
        //internal static IFakeConfigEntry<bool> diseaseShapeNameFix;
        //internal static IFakeConfigEntry<bool> patchVariableName;
        //internal static IFakeConfigEntry<bool> liftVariableTypeCheck;
        //internal static IFakeConfigEntry<bool> exposeMoreVariables;
        //internal static IFakeConfigEntry<bool> widenVariableAccessibility;
        //internal static IFakeConfigEntry<bool> describeVariableType;
        //internal static IFakeConfigEntry<bool> expandEventFunctionality;
        //internal static IFakeConfigEntry<bool> perCountryEventFunctionality;
        //internal static IFakeConfigEntry<bool> expandTraitFunctionality;
        //internal static IFakeConfigEntry<bool> replaceTraitRequirement;

        internal static MelonPreferences_Entry<bool> patchTranslationLowercasing;
        internal static MelonPreferences_Entry<bool> diseaseShapeNameFix;
        internal static MelonPreferences_Entry<bool> patchVariableName;
        internal static MelonPreferences_Entry<bool> liftVariableTypeCheck;
        internal static MelonPreferences_Entry<bool> exposeMoreVariables;
        internal static MelonPreferences_Entry<bool> widenVariableAccessibility;
        internal static MelonPreferences_Entry<bool> describeVariableType;
        internal static MelonPreferences_Entry<bool> expandEventFunctionality;
        internal static MelonPreferences_Entry<bool> perCountryEventFunctionality;
        internal static MelonPreferences_Entry<bool> expandTraitFunctionality;
        internal static MelonPreferences_Entry<bool> replaceTraitRequirement;

        internal static void Awake()
        {
            CategoryQOL = SetupCategory(CATEGORY_QOL);
            CategoryVariable = SetupCategory(CATEGORY_VARIABLE);
            CategoryEvent = SetupCategory(CATEGORY_EVENT);
            CategoryTrait = SetupCategory(CATEGORY_TRAIT);

            patchTranslationLowercasing = CategoryQOL.CreateEntry(
                nameof(patchTranslationLowercasing),
                true,
                nameof(patchTranslationLowercasing),
                PATCH_TRANSLATION_LOWERCASING_DESC);

            diseaseShapeNameFix = CategoryQOL.CreateEntry(
                nameof(diseaseShapeNameFix),
                true,
                nameof(diseaseShapeNameFix),
                DISEASE_SHAPE_NAME_FIX_DESC);

            patchVariableName = CategoryQOL.CreateEntry(
                nameof(patchVariableName),
                true,
                nameof(patchVariableName),
                PATCH_VARIABLE_NAME_DESC);

            liftVariableTypeCheck = CategoryVariable.CreateEntry(
                nameof(liftVariableTypeCheck),
                true,
                nameof(liftVariableTypeCheck),
                LIFT_VARIABLE_TYPE_CHECK_DESC);

            exposeMoreVariables = CategoryVariable.CreateEntry(
                nameof(exposeMoreVariables),
                true,
                nameof(exposeMoreVariables),
                EXPOSE_MORE_VARIABLES_DESC);

            widenVariableAccessibility = CategoryVariable.CreateEntry(
                nameof(widenVariableAccessibility),
                true,
                nameof(widenVariableAccessibility),
                WIDEN_VARIABLE_ACCESSIBILITY_DESC);

            describeVariableType = CategoryVariable.CreateEntry(
                nameof(describeVariableType),
                true,
                nameof(describeVariableType),
                DESCRIBE_VARIABLE_TYPE_DESC);

            expandEventFunctionality = CategoryEvent.CreateEntry(
                nameof(expandEventFunctionality),
                true,
                nameof(expandEventFunctionality),
                EXPAND_EVENT_FUNCTIONALITY_DESC);

            perCountryEventFunctionality = CategoryEvent.CreateEntry(
                nameof(perCountryEventFunctionality),
                true,
                nameof(perCountryEventFunctionality),
                PER_COUNTRY_EVENT_FUNCTIONALITY_DESC);

            expandTraitFunctionality = CategoryTrait.CreateEntry(
                nameof(expandTraitFunctionality),
                true,
                nameof(expandTraitFunctionality),
                EXPAND_TRAIT_FUNCTIONALITY_DESC);

            replaceTraitRequirement = CategoryTrait.CreateEntry(
                nameof(replaceTraitRequirement),
                true,
                nameof(replaceTraitRequirement),
                REPLACE_TRAIT_REQUIREMENT_DESC);

            CategoryQOL.SaveToFile();
            CategoryVariable.SaveToFile();
            CategoryEvent.SaveToFile();
            CategoryTrait.SaveToFile();
        }

        private static MelonPreferences_Category SetupCategory(string name)
        {
            MelonPreferences_Category category = MelonPreferences.CreateCategory(name);
            category.SetFilePath("UserData/SCPlus.cfg");
            return category;
        }

        internal interface IFakeConfigEntry<T>
        {
            public T Value { get; }
        }

        internal class FakeFlagConfigEntry : IFakeConfigEntry<bool>
        {
            public bool Value => true;
        }
#endif
    }
}
