using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PensionerDetail.Entities;
using PensionerDetail.DbContexts;

namespace PensionerDetail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PensionerDetailsController : ControllerBase
    {
        /*private readonly PensionerDetailDbContext context;
        public PensionerDetailsController(PensionerDetailDbContext _context)
        {
            context = _context;
        }*/

        List<PensionerDetails> pensioners = new List<PensionerDetails>();

        public PensionerDetailsController()
        {
            pensioners.Add(
                new PensionerDetails()
                {
                    Name = "bunk seenu",
                    dateOfBirth = new DateTime(1990, 01, 02),
                    PAN = "ABCDE1234F",
                    salaryEarned = 45000,
                    allowances = 500,
                    pensionType = PensionerDetails.PensionType.family,
                    bankDetails = new BankDetails() { bankType=BankType.publicbank,bankName="SBI",accountNumber="SBI00001BS"}
                }
                );
            pensioners.Add(
                new PensionerDetails()
                {
                    Name = "pampacheck",
                    dateOfBirth = new DateTime(1976,05,03),
                    PAN = "NXXGF7589T",
                    salaryEarned = 65000,
                    allowances = 1000,
                    pensionType = PensionerDetails.PensionType.self,
                    bankDetails = new BankDetails() { bankType = BankType.publicbank, bankName = "Union", accountNumber = "UNB55002PC" }
                }
                );
             pensioners.Add(
                new PensionerDetails()
                {
                    Name = "James Bond",
                    dateOfBirth = new DateTime(1959,07,12),
                    PAN = "YTUIR7582N",
                    salaryEarned = 80900,
                    allowances = 15650,
                    pensionType = PensionerDetails.PensionType.self,
                    bankDetails = new BankDetails() { bankType = BankType.privatebank, bankName = "SWISS", accountNumber = "SW00723JB" }
                }
                );
        }

        [Route("getById")]
        [HttpGet]
        public IActionResult Get(string aadharID)
        {
            
           var result = getPensioner(Convert.ToString(aadharID));
            if(result!=null)
            {
                return Ok(result);
            }
            //return BadRequestResult("failed");
            return new BadRequestResult();
           
        }

        public BankDetails getbankdetails(string aadharId)
        {
            var result =  getPensioner(aadharId);
            if(result!=null)
            {
                return result.bankDetails;
            }
            return null;
        }


        public PensionerDetails getPensioner(string aadharID)
        {
            PensionerDetails findPensioner = pensioners.Find(x => x.PAN.Equals(aadharID));
            if (findPensioner != null)
            {
                return findPensioner;
            }
            return null;
        }

    }
}
