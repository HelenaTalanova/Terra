using Newtonsoft.Json;
using TerraJigu.jigu.core.sdk;

namespace TerraJigu.jigu.core.auth
{
    /// <summary />
    public class StdSignature
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("signature")]
        public string Signature { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("pub_key")]
        public PublicKey PubKey { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public StdSignature(string signature, PublicKey pubKey)
        {
            Signature = signature;
            PubKey = pubKey;
        }
    }
}
