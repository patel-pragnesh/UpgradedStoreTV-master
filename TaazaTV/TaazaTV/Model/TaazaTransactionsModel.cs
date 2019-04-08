using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    class TaazaTransactionsModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public string[] resourceList { get; set; }
        public transactions data { get; set; }

    }
    public class transactions
    {
        public taazatransactions[] transaction { get; set; }
        public string current_wallet_balance { get; set; }
    }

    public class taazatransactions
    {
        public string type { get; set; }
        public string amount { get; set; }
        public int transaction_type_id { get; set; }
        public string transaction_type { get; set; }
        public string date { get; set; }

        public bool IsMisusVisible
        {
            get
            {
                return type == "debit" ? true : false;
            }
            set { }
        }

        public string TransactionColor
        {
            get
            {
                return type == "debit" ? "Red" : "Green";
            }
            set { }
        }

        public string transactionimg
        {
            get
            {
                switch (transaction_type_id)
                {
                    case 1:
                        return "imgreferral.png";

                    case 2:
                        return "imgreferral.png";

                    case 3:
                        return "imgdefault.png";

                    case 4:
                        return "imgevent.png";

                    case 5:
                        return "imgrestaurant.png";

                    case 6:
                        return "imgcashback.png";

                    case 7:
                        return "imgcashback.png";

                    case 8:
                        return "imgexpired.png";

                    case 9:
                        return "imgexpired.png";

                    default:
                        return "imgdefault.png";
                }
            }
            set { }
        }



    }
}



