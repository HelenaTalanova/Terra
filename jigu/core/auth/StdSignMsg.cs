using jigu.core.auth;
using Newtonsoft.Json;
using System.Collections.Generic;
using TerraJigu.jigu.core.msg;

namespace TerraJigu.jigu.core.auth
{
    /// <summary />
    public class StdSignMsg
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("chain_id")]
        public string ChainId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("account_number")]
        public int AccountNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("sequence")]
        public int Sequence { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("fee")]
        public StdFee Fee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("msgs")]
        public List<StdMsg> Msgs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("memo")]
        public string Memo { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public StdSignMsg(string chainId, int accountNumber, int sequence, StdFee fee, StdMsg msg, string memo)
        {
            ChainId = chainId;
            AccountNumber = accountNumber;
            Sequence = sequence;
            Fee = fee;
            Msgs = new List<StdMsg>() { msg };
            Memo = memo;
        }
    }
}
