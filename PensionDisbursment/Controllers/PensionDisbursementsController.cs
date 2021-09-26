using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using PensionDisbursment.Entities;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Newtonsoft.Json;

namespace PensionDisbursment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PensionDisbursementsController : ControllerBase
    {
        public PensionDisbursementsController()
        {
            //PensionerInput input = new PensionerInput()
            //{
            //    Name = "bunk seenu",
            //    dateOfBirth = new DateTime(1990, 01, 02),
            //    PAN = "ABCD12351E",
            //    aadhaar = 123456,
            //    selforfamily = 0
            //};
            //calculatefromdetails(5000, input);



        }

        [Route("calculate")]
        [HttpGet]
        public IActionResult calculatefromdetails(string aadhaarNo,double pensionAmount,int serviceCharge)
        {


            StatusCodeResult flag = new StatusCodeResult(21);
            using (var response = new HttpClient())
            {
                //var jsondes =JsonConvert.DeserializeObject(inputFromPensioner);
                // JsonDocument jd = JsonDocument.Parse(inputFromPensioner);

                response.BaseAddress = new Uri("http://localhost:55345/api/pensionerdetails/");
                var responseTalk = response.GetAsync("getById?aadharID=" + aadhaarNo);
                responseTalk.Wait();
                double actualPensionAmount = 0;
                var result = responseTalk.Result;
                string sum = "";
                if (result.IsSuccessStatusCode)
                {
                    var readTalk = result.Content.ReadAsStringAsync();
                    readTalk.Wait();
                    JsonDocument jdc = JsonDocument.Parse(readTalk.Result);

                    if (jdc.RootElement.GetProperty("pensionType").ToString().Equals("1"))
                    {
                        actualPensionAmount = (Double.Parse(jdc.RootElement.GetProperty("salaryEarned").ToString()) * 0.8) +
                            (Double.Parse(jdc.RootElement.GetProperty("allowances").ToString())) +
                            serviceCharge;

                        //sum = (Double.Parse(jdc.RootElement.GetProperty("salaryEarned").ToString()) * 0.8) + " "+
                        //    (Double.Parse(jdc.RootElement.GetProperty("allowances").ToString())) + " " +
                        //    (Double.Parse(inputFromPensioner.GetProperty("serviceCharge").ToString()));
                    }

                    else
                    {
                        actualPensionAmount = (Double.Parse(jdc.RootElement.GetProperty("salaryEarned").ToString()) * 0.5) +
                           (Double.Parse(jdc.RootElement.GetProperty("allowances").ToString())) +
                           serviceCharge;

                        //sum = (Double.Parse(jdc.RootElement.GetProperty("salaryEarned").ToString()) * 0.5) + " " +
                        //   (Double.Parse(jdc.RootElement.GetProperty("allowances").ToString())) + " " +
                        //   (Double.Parse(inputFromPensioner.GetProperty("serviceCharge").ToString()));
                    }


                    if (pensionAmount != actualPensionAmount)
                    {
                        // return Ok(actualPensionAmount + " "+jdc.RootElement+" In if"+ " "+ sum);
                        return new StatusCodeResult(21);
                    }
                }
                // return Ok(actualPensionAmount+ " "+sum);
                return new StatusCodeResult(10);
            }
        }
    }
}
