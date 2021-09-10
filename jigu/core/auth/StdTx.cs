using jigu.core.auth;
using Newtonsoft.Json;
using System.Collections.Generic;
using TerraJigu.Extensions;
using TerraJigu.jigu.core.msg;

namespace TerraJigu.jigu.core.auth
{
    /// <summary />
    public class StdTx
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; init; } = "core/StdTx";

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("value")]
        public object Value { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public StdTx(StdFee fee, List<StdMsg> msg, object signatures, string memo)
        {
            Value = new DataCoreStdTx()
            {
                Fee = fee,
                Msg = msg,
                Signatures = signatures,
                Memo = memo
            };
        }

        public string ToData()
        {
            //var data = new Dictionary<string, string>();
            var mode = "block";
            var data = $"{{ \"tx\": {Value.ToJson(enIndentedFormat: true)}, \"mode\": \"{mode}\"}}";
            return data;

        }

        /// <summary>
        /// Data type "core/StdTx"
        /// </summary>
        public class DataCoreStdTx
        {
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("fee")]
            public StdFee Fee { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("msg")]
            public List<StdMsg> Msg { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("signatures")]
            public object Signatures { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("memo")]
            public string Memo { get; set; }
        }
    }
}
