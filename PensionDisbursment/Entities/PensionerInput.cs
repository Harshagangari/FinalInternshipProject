using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PensionDisbursment.Entities
{
    public class PensionerInput
    {
        public string Name { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string PAN { get; set; }
        public int aadhaar { get; set; }
        public int selforfamily { get; set; }
    }
}
