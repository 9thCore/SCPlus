using System;
using System.Diagnostics;

namespace SCPlus.patch.lang
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal class LocalizationHandlerAttribute : Attribute
    {
        internal readonly string language;
        internal readonly string moreVariables;
        internal readonly string extraFunctionality;
        internal readonly string perCountry;
        internal readonly string techExtraFunctionality;
        internal readonly string patchedVariables;
        internal readonly string widenedAccess;
        internal readonly string describeVariableType;

        internal LocalizationHandlerAttribute(
            string language,
            string moreVariables,
            string extraFunctionality,
            string perCountry,
            string techExtraFunctionality,
            string patchedVariables,
            string widenedAccess,
            string describeVariableType)
        {
            this.language = language;
            this.moreVariables = moreVariables;
            this.extraFunctionality = extraFunctionality;
            this.perCountry = perCountry;
            this.techExtraFunctionality = techExtraFunctionality;
            this.patchedVariables = patchedVariables;
            this.widenedAccess = widenedAccess;
            this.describeVariableType = describeVariableType;
        }
    }
}
