namespace NetCoreStack.Hisar.Extensions
{
    public static class StringExtensions
    {
        public static string ControllerName(this string controllerName)
        {
            return controllerName.EndsWith("Controller") ? controllerName.Substring(0, controllerName.Length - 10) : controllerName;
        }
    }
}
