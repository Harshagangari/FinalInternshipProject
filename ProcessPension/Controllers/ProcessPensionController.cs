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
using System.Dynamic;

namespace ProcessPension.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessPensionController : ControllerBase
    {
        double pensionToDisburse = 0;
        //PensionerInput inputOfPensioner = new PensionerInput() {Name="bunk seenu",dateOfBirth= new DateTime(1990,01,02),PAN= "ABCD12351E",
           // aadhaarNumber=123456789012,pensiontype=PensionType.family};

        static ProcessPensionInput inputForPensioner = new ProcessPensionInput();

        [Route("EnterPensionDetails")]
        [HttpGet]
        public IActionResult getPensionDetails(dynamic inputOfPensioner)
        {
            int serviceCharge = 0;
            using (var askPensionerDetails = new HttpClient())
            {              
                askPensionerDetails.BaseAddress = new Uri("http://localhost:55345/api/PensionerDetails/");
                var responseTalk = askPensionerDetails.GetAsync("getById?aadharID="+ inputOfPensioner.GetProperty("Pan"));
                responseTalk.Wait();
                
               
                var result =responseTalk.Result ;
    
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                   
                    JsonDocument jd = JsonDocument.Parse(readTask.Result);
                    //inputForPensioner.aadhaarNumber = long.Parse(jd.RootElement.GetProperty("aadharID").ToString());
                  
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
                        serviceCharge = 500;
                    }
                    else
                    {
                        pensionToDisburse += 550;
                        serviceCharge = 550;
                    }
                    inputForPensioner.PensionAmount = pensionToDisburse;

                  // var postResult = processPensionInput(inputForPensioner,serviceCharge );
                    return Ok(pensionToDisburse);
                }
            }
            return BadRequest("failed");
        }



        [Route("PostDetails")]
        [HttpPost]
        public IActionResult processPensionInput(ProcessPensionInput inputFromPensioner,int serviceCharge)
        {
            using(var askPensionDisbursement = new HttpClient())
            {
               var jsonstring = JsonConvert.SerializeObject(inputFromPensioner);
               askPensionDisbursement.BaseAddress = new Uri("http://localhost:51549/api/PensionDisbursements/");
                var responseTalk = askPensionDisbursement.GetAsync("calculate?inputFromPensioner="+jsonstring);
                responseTalk.Wait();


                int code = (int)responseTalk.Result.StatusCode;
                if(code==10)
                  {
                      var readTask = responseTalk.Result.Content.ReadAsStringAsync();

                      return Ok(readTask.Result);
                  }
                
                return Ok(responseTalk.Result+"hello");
               
            }
           
        }
    }
}
