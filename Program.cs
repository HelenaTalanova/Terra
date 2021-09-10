using jigu.client.object_query;
using jigu.core.auth;
using jigu.core.sdk;
using System;
using TerraJigu.Extensions;
using TerraJigu.jigu.core.msg;
using TerraJigu.jigu.key;

namespace TerraJigu
{
    class Program
    {
        static void Main(string[] args)
        {
            var fee = new StdFee()
            {
                Gas = 0.2m,
                Amount = new Coin[]
                {
                    new Coin("uluna", 0.2m)
                }
            };

            var msg = new MsgSend()
            {
                Value = new MsgSend.DataBankMsgSend()
                {
                    FromAddress = "terra14kxa83ctanwfx0qdaglzqnzhyhw4s36gpyxmhq",
                    ToAddress = "terra1wga8hv06ktm5mclwneyu9f6ehkcv7tvlh0edwu",
                    Amount =
                    {
                        new Coin("uluna", 10)
                    }
                }
            };

            var mnemonic = "outdoor example million loan zone earth danger fat walnut praise call aerobic any cancel caution leopard section mandate relief audit invest never evoke price";

            var key = new MnemonicKey(mnemonic);
            Console.WriteLine(key.base_address());
            var w = new Wallet(key);
            var tx = w.CreateAndSignTx(msg, fee, "Memo is optional.");
            
            var json = tx.ToData();// tx.ToJson(enIndentedFormat: true);
            Console.WriteLine(json);





            var signVerify = "92F7v/RRfgNyDc4HPfPQ3cq9d7nsbWo35YkeEpQ03cNhli2JhThb2rrf7XF6TKIQ/z2fKUcTGYhAQezN/Na+pg==";
            if (json.Contains(signVerify))
                Console.WriteLine("Ok");
            else
                throw new Exception("signatures error");
        }
    }
}

