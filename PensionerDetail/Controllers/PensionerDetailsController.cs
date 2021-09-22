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
                    PAN = "ABCD12351E",
                    salaryEarned = 45000,
                    allowances = 500,
                    pensionType = PensionerDetails.PensionType.family,
                    bankDetails = new BankDetails() { bankType=BankType.publicbank,bankName="SBI",accountNumber="SBI00001BS"}
                }
                );
        }

        [Route("getById")]
        [HttpGet]
        public PensionerDetails Get(string aadharID)
        {
           
           var result = getPensioner(aadharID);
            if(result!=null)
            {
                return (result);
            }
            // return BadRequest("Details did not match. Check your details again...");
            return null;
           
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
