using System;
using UnityEngine;

namespace SCPlus.patch.game_event
{
    internal class VariableSetterRedirector : MonoBehaviour
    {
        // Accepts the original parameter and returns a new object, which is the "redirected" object
        // Can return same object if no redirection is required
        internal Func<object, object> apply = null;

        public object Apply(object value)
        {
            if (apply == null)
            {
                return value;
            }

            try
            {
                return apply(value);
            }
            catch (Exception ex)
            {
                Plugin.LogError($"Encountered error while redirecting: {ex}");
                return value;
            }
        }
    }
}
