using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProcessPension.Entities;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace ProcessPension.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessPensionController : ControllerBase
    {
        double pensionToDisburse = 0;
        PensionerInput inputOfPensioner = new PensionerInput() {Name="bunk seenu",dateOfBirth= new DateTime(1990,01,02),PAN= "ABCD12351E",
            aadhaarNumber=123456789012,pensiontype=PensionType.family};

        ProcessPensionInput inputForPensioner = new ProcessPensionInput();

        [Route("EnterPensionDetails")]
        [HttpGet]
        public IActionResult getPensionDetails(/*PensionerInput inputOfPensioner*/)
        {
            using (var askPensionerDetails = new HttpClient())
            {
                
                askPensionerDetails.BaseAddress = new Uri("http://localhost:55345/api/pensionerdetails/");
                var responseTalk = askPensionerDetails.GetAsync("getById?aadharId=" + inputOfPensioner.PAN);
                responseTalk.Wait();
                var result =responseTalk.Result ;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    JsonDocument jd = JsonDocument.Parse(readTask.Result);
                    //inputForPensioner.aadhaarNumber = long.Parse(jd.RootElement.GetProperty("aadharID").ToString());
                    inputForPensioner.aadhaarNumber = 123456789012;
                    if (jd.RootElement.GetProperty("pensionType").ToString().Equals("1"))
                    {
                        pensionToDisburse = (Double.Parse(jd.RootElement.GetProperty("salaryEarned").ToString()) * 0.8) +
                            (Double.Parse(jd.RootElement.GetProperty("allowances").ToString()));
                    }
                    else
                    {
                        pensionToDisburse = (Double.Parse(jd.RootElement.GetProperty("salaryEarned").ToString()) * 0.5) +
                           (Double.Parse(jd.RootElement.GetProperty("allowances").ToString()));
                    }
                    var bankDetails = jd.RootElement.GetProperty("bankDetails");
                    
                    
                    if (jd.RootElement.GetProperty("bankDetails").GetProperty("bankType").Equals("0"))
                    {
                        pensionToDisburse += 500;
                    }
                    else
                    {
                        pensionToDisburse += 550;
                    }
                    inputForPensioner.PensionAmount = pensionToDisburse;
                    return Ok(pensionToDisburse);
                }
            }
            return BadRequest("failed");
        }

        [Route("PostDetails")]
        [HttpPost]
        public IActionResult processPensionInput(/*ProcessPensionInput inputFromPensioner,int serviceCharge*/)
        {
            using(var askPensionDisbursement = new HttpClient())
            {
                int jsonres = 0;
                askPensionDisbursement.BaseAddress = new Uri("http://localhost:51549/api/PensionDisbursements/");
                var responseTalk = askPensionDisbursement.GetAsync("calculate");
                responseTalk.Wait();
                if(responseTalk.Result.IsSuccessStatusCode)
                {
                    var readTask = responseTalk.Result.Content.ReadAsStringAsync();
                    jsonres = JsonConvert.DeserializeObject<int>(readTask.Result);
                }
                return Ok(jsonres);
            }
           
        }
    }
}
