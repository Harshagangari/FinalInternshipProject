using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PensionerDetail.Entities
{
    public class PensionerDetails
    {
        public enum PensionType { self,family}

        public string Name { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string PAN { get; set; }
        public double salaryEarned { get; set; }
        public double allowances { get; set; }
        public PensionType pensionType { get; set; }
        public BankDetails bankDetails { get; set; }
      
    }
}
