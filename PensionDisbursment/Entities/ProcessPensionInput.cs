using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PensionDisbursment.Entities
{
    
    public class ProcessPensionInput
    {
        public long aadhaarNumber { get; set; }
        public double pensionAmount { get; set; }
        public double bankServiceCharges { get; set; }
    }
}
