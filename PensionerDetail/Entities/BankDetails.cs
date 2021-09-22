using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PensionerDetail.Entities
{
    public enum BankType { publicbank, privatebank }
    public class BankDetails
    {
        public string bankName { get; set; }
        public string accountNumber { get; set; }
        public BankType bankType { get; set; }
    }
}
