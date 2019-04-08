using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    class TransactionsModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public string[] resourceList { get; set; }
        public transactions data { get; set; }
        public string current_wallet_balance { get; set; }
    }
    public class transactions
    {
        public string type { get; set; }
        public string amount { get; set; }
        public string transaction_type { get; set; }
        public string date { get; set; }
        public int transaction_type_id { get; set; }
    }
}
