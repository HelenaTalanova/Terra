using jigu.core.sdk;
using Newtonsoft.Json;

namespace jigu.core.auth
{
    /// <summary />
    public class StdFee
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("gas")]
        public decimal Gas { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("amount")]
        public Coin[] Amount { get; set; }
    }
}
