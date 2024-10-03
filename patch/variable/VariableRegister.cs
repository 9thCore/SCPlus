namespace SCPlus.patch.variable
{
    internal class VariableRegister
    {
        internal static void Awake()
        {
            new VariableBuilder("custom_global_variable_6", "World")
                .CategoryCustom()
                .Translation("Event_Variable_custom_global_variable_6")
                .ComplexitySuperAdvanced()
                .Condition()
                .Outcome()
                .Expression()
                .Register();
        }
    }
}
