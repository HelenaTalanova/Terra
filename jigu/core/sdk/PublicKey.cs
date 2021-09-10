using Newtonsoft.Json;

namespace TerraJigu.jigu.core.sdk
{
    /// <summary />
    public class PublicKey
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = "tendermint/PubKeySecp256k1";

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public PublicKey(string value)
        {
            Value = value;
        }
    }
}
