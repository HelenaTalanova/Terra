using Newtonsoft.Json;

namespace TerraJigu.jigu.core.msg
{
    /// <summary />
    public abstract class StdMsg
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("type")]
        public abstract string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("value")]
        public abstract object Value { get; set; }
    }
}
