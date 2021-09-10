using jigu.core.auth;
using TerraJigu.jigu.core.auth;
using TerraJigu.jigu.core.msg;
using TerraJigu.jigu.key;

namespace jigu.client.object_query
{
    /// <summary />
    public class Wallet
    {
        private readonly Key key;

        public Wallet(Key key)
        {
            this.key = key;
        }

        /// <summary>
        /// Creates a sign message, signs it, and produces a transaction in one go.
        /// Outputs a ready-to-broadcast "StdTx".
        /// </summary>
        public StdTx CreateAndSignTx(StdMsg msg, StdFee fee = null, string memo = null)
        {
            return key.SignTx(CreateTx(msg, fee: fee, memo: memo));
        }

        /// <summary>
        /// Creates a sign message (`StdSignMsg`), which contains the necessary info to
        /// sign the transaction. Helpful to think of it as "create unsigned tx".
        /// </summary>
        public StdSignMsg CreateTx(StdMsg msg, StdFee fee = null, string memo = "")
        {
            //            if not fee:
            //# estimate our fee if fee not supplied
            //                tx = StdTx(msg = msgs, memo = memo)
            //            fee = self.terra.tx.estimate_fee(tx)
            //        if self._manual_sequence:
            //            sequence = self._sequence
            //            self._sequence += 1
            //        else:
            //            sequence = self.sequence

            return new StdSignMsg(chainId: "bombay-10",  // self.terra.chain_id,
                                  accountNumber: 60441,      // self.account_number,
                                  sequence: 9,
                                  fee: fee,
                                  msg: msg,
                                  memo: memo);
        }

    }
}
