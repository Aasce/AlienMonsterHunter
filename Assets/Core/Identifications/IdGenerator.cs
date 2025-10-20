namespace Asce.Managers
{
    public static class IdGenerator
    {
        /// <summary>  Generate globally unique, JSON-safe ID. </summary>
        public static string NewId(string prefix = "id")
        {
            return $"{prefix}_{System.Guid.NewGuid():N}";
        }
    }
}