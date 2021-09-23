using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessPension.Entities
{
    public enum PensionType { self,family}
    public class PensionerInput
    {
        public string Name { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string PAN  { get; set; }
        public long aadhaarNumber { get; set; }
        public PensionType pensiontype { get; set; }
    }
}
