using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Asce.Core.SaveLoads
{
    /// <summary>
    ///     Provides pre-configured JsonSerializerSettings for the game.
    /// </summary>
    public static class JsonSettings
    {
        /// <summary>
        ///     Custom resolver: serialize only fields (ignore properties).
        /// </summary>
        private class FieldsOnlyContractResolver : DefaultContractResolver
        {
            protected override JsonObjectContract CreateObjectContract(System.Type objectType)
            {
                var contract = base.CreateObjectContract(objectType);
                contract.MemberSerialization = MemberSerialization.Fields;
                return contract;
            }
        }

        /// <summary>
        ///     Default settings for field-only serialization.
        /// </summary>
        public static readonly JsonSerializerSettings FieldOnly = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,                     // Pretty-print JSON
            TypeNameHandling = TypeNameHandling.Auto,             // Allow polymorphic serialization
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore, // Prevent infinite loops
            NullValueHandling = NullValueHandling.Include,        // Keep null values
            ContractResolver = new FieldsOnlyContractResolver()   // Only serialize fields
        };
    }
}
