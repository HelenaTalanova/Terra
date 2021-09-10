using jigu.core.sdk;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TerraJigu.jigu.core.msg
{
    /// <summary />
    public class MsgSend : StdMsg
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("type")]
        public override string Type { get; set; } = "bank/MsgSend";

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("value")]
        public override object Value { get; set; }

        /// <summary>
        /// Data type "bank/MsgSend"
        /// </summary>
        public class DataBankMsgSend
        {
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("from_address")]
            public string FromAddress { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("to_address")]
            public string ToAddress { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("amount")]
            public List<Coin> Amount { get; set; } = new List<Coin>();
        }
    }
}
