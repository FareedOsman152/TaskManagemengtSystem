using TaskManagmentSystem.Srvices.Interfaces;

namespace TaskManagmentSystem.Srvices
{
    public static class Check
    {
        public static void IsNull(object value)
        {
            if (value is null)
                throw new ArgumentNullException($"The {nameof(value)} is null");
        }
    }
}
