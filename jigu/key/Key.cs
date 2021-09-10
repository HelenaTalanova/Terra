using System;
using TerraJigu.Extensions;
using TerraJigu.jigu.core.auth;
using TerraJigu.jigu.core.sdk;

namespace TerraJigu.jigu.key
{
    /// <summary />
    public class Key
    {
        protected string private_key;// = "a50d36a53bb4fee91e225dfc54134cff88fea532e892e11af9dc2e4bb9b2fe79";
        //protected byte[] private_key = { 0xa5, 0x0d, 0x36, 0xa5, 0x3b, 0xb4, 0xfe, 0xe9, 0x1e, 0x22, 0x5d, 0xfc, 0x54, 0x13, 0x4c, 0xff, 0x88, 0xfe, 0xa5, 0x32, 0xe8, 0x92, 0xe1, 0x1a, 0xf9, 0xdc, 0x2e, 0x4b, 0xb9, 0xb2, 0xfe, 0x79 };
        protected byte[] public_key;// = new byte[] { 0x03, 0x6d, 0xa8, 0x0e, 0x2e, 0x46, 0x11, 0x84, 0xfc, 0xa1, 0x68, 0xd8, 0x84, 0x31, 0x43, 0x0a, 0xd9, 0x39, 0xb9, 0x20, 0xed, 0xa1, 0x29, 0xed, 0x03, 0x10, 0xfb, 0xcf, 0x4d, 0x4c, 0xab, 0xa5, 0xe0 };

        /// <summary>
        /// 
        /// </summary>
        public StdTx SignTx(StdSignMsg tx)
        {
            var signature = CreateSignature(tx);

            return new StdTx(fee: tx.Fee, msg: tx.Msgs, signatures: new[] { signature }, memo: tx.Memo);
        }

        /// <summary>
        /// 
        /// </summary>
        public StdSignature CreateSignature(StdSignMsg tx)
        {
            var sig_data = Sign(tx.ToJson());
            var pub_key = new PublicKey(value: Convert.ToBase64String(public_key));

            return new StdSignature(signature: Convert.ToBase64String(sig_data ?? new byte[1]), pubKey: pub_key);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual byte[] Sign(string value) => throw new NotImplementedException();
    }
}
