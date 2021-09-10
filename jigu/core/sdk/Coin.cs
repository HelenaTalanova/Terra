using Newtonsoft.Json;

namespace jigu.core.sdk
{
    /// <summary />
    public class Coin
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("denom")]
        public string Denom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public Coin() { }

        /// <summary>
        /// .ctor
        /// </summary>
        public Coin(string denom, decimal amount)
        {
            Denom = denom;
            Amount = amount;
        }
    }
}
