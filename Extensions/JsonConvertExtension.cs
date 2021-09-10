using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;

namespace TerraJigu.Extensions
{
    /// <summary />
    public static class JsonConvertExtension
    {
        /// <summary>
        /// Сериализовать обьект в json
        /// </summary>
        public static string ToJson(this object obj, bool sorted = true, bool enIndentedFormat = false)
        {
            var format = enIndentedFormat ? Formatting.Indented : Formatting.None;

            if (sorted)
                JsonConvert.DefaultSettings = () => new JsonSerializerSettings() { ContractResolver = new OrderedContractResolver() };

            return JsonConvert.SerializeObject(value: obj,
                                               formatting: format,
                                               new DecimalFormatConverter(),
                                               new IntFormatConverter());
        }

        /// <summary />
        public class OrderedContractResolver : DefaultContractResolver
        {
            protected override System.Collections.Generic.IList<JsonProperty> CreateProperties(System.Type type, MemberSerialization memberSerialization)
            {
                return base.CreateProperties(type, memberSerialization).OrderBy(p => p.PropertyName).ToList();
            }
        }
    }
}
