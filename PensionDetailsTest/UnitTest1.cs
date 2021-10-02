using NUnit.Framework;
using NUnit;
using PensionerDetail.Entities;
using PensionerDetail.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace PensionDetailsTest
{
        public class PensionerDetailsTest
        {
            List<PensionerDetails> list = new List<PensionerDetails>();
            List<BankDetails> actualBankdetails = new List<BankDetails>();


        public string PAN = "ABCDE1235F";
            PensionerDetails details = new PensionerDetails()
            {
                Name = "bunk seenu",
                dateOfBirth = new DateTime(1990, 01, 02),
                PAN = "ABCDE1234F",
                salaryEarned = 45000,
                allowances = 500,
                pensionType = PensionerDetails.PensionType.family,
                bankDetails = new BankDetails() { bankType = BankType.publicbank, bankName = "SBI", accountNumber = "SBI00001BS" }
            };

        [TestCase("ABCDE1234F")]
        public void PensionDetail_getPensioner(string PAN)
        {
            PensionerDetailsController contr = new PensionerDetailsController();
            PensionerDetails pensioner = contr.getPensioner(PAN);
            list.Add(details);
            Assert.AreEqual(list[0].allowances, pensioner.allowances);
            list.RemoveAt(0);
        }
        [TestCase("ABCDE1234F")]
        public void PensionDetail_getBankDetails(string PAN)
        {
            PensionerDetailsController BankDetails = new PensionerDetailsController();
            BankDetails expectedBankDetails = BankDetails.getbankdetails(PAN);
            
            actualBankdetails.Add(new BankDetails
            {
                bankName = "SBI",
                accountNumber = "SBI00001BS",
                bankType = BankType.publicbank
            });

            Assert.AreEqual(actualBankdetails[0].bankName, expectedBankDetails.bankName);
            Assert.AreEqual(actualBankdetails[0].bankType, expectedBankDetails.bankType);
            Assert.AreEqual(actualBankdetails[0].accountNumber, expectedBankDetails.accountNumber);

        }
        [TestCase("ABCDE1234F")]
        public void PensionDetail_GetOK200(string PAN)
        {
            PensionerDetailsController contr = new PensionerDetailsController();
            IActionResult pensioner = contr.Get(PAN);
          
            Assert.IsInstanceOf<Microsoft.AspNetCore.Mvc.OkObjectResult>(pensioner);
            
        }

        //readonly string PANnotFound = "ASDFN1234D";

        [TestCase("ASDFN1234D")]
        public void PensionDetail_getPensionerNull(string PANnotFound)
        {
            PensionerDetailsController contr = new PensionerDetailsController();
            PensionerDetails pensioner = contr.getPensioner(PANnotFound);
            Assert.IsNull(pensioner);
        }

        [TestCase("ASDFN1234D")]
        public void Pensiondetails_getBankDetailsNull(string PANnotFound)
        {
            PensionerDetailsController BankDetails = new PensionerDetailsController();
            BankDetails expectedBankDetails = BankDetails.getbankdetails(PAN);
            Assert.IsNull(expectedBankDetails);
        }

        
        [TestCase("ASDFN1234D")]
        public void PensionDetail_BadRequest400(string PANnotFound)
        {
            PensionerDetailsController contr = new PensionerDetailsController();
            IActionResult pensioner =  contr.Get(PANnotFound);
           
            Assert.IsInstanceOf<Microsoft.AspNetCore.Mvc.BadRequestResult>(pensioner);
          

        }
    }
}
