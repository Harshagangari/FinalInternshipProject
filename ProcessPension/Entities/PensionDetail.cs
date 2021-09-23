using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessPension.Entities
{
    public class PensionDetail
    {
        public string Name { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string PAN { get; set; }
        public PensionType pensiontype { get; set; }
        public double PensionAmount { get; set; }
    }
}
