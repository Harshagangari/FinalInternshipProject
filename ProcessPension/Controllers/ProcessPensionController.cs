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
using System.Web.Helpers;
using System.Text.Json.Serialization;

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
        public string getPensionDetails(string aadhaarNo,string name,DateTime dob,string pan,int pensiontype)
        {
            int serviceCharge = 0;
            using (var askPensionerDetails = new HttpClient())
            {              
                askPensionerDetails.BaseAddress = new Uri("http://localhost:55345/api/PensionerDetails/");
                var responseTalk = askPensionerDetails.GetAsync("getById?aadharID="+pan);
                responseTalk.Wait();

                var result =responseTalk.Result ;
                
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    
                    JsonDocument jd = JsonDocument.Parse(readTask.Result);
                    //inputForPensioner.aadhaarNumber = long.Parse(jd.RootElement.GetProperty("aadharID").ToString());
                  
                    if (pensiontype.ToString().Equals("0"))
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
                    if (jd.RootElement.GetProperty("bankDetails").GetProperty("bankType").ToString().Equals("0"))
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
                    inputForPensioner.aadhaarNumber = jd.RootElement.GetProperty("pan").ToString();
                  var postResult = processPensionInput(inputForPensioner,serviceCharge );
                    //return pensionToDisburse.ToString()+" disburse" //23500;
                    //return postResult.ToString();
                    if (postResult.ToString().Equals("Ok"))
                    {
                        return pensionToDisburse.ToString();
                    }
                    else
                        return ("Check your details and fill again !");
                        //return result.Content.ReadAsStringAsync().Result;
                }
            }
            return ("failed");
        }



        [Route("PostDetails")]
        [HttpPost]
        public string processPensionInput([FromBody] ProcessPensionInput inputFromPensionerBody,int serviceCharge)
        {
           //return Ok(inputFromPensionerBody);
            using(var askPensionDisbursement = new HttpClient())
            {
                //return Ok(inputFromPensionerBody);
               askPensionDisbursement.BaseAddress = new Uri("http://localhost:51549/api/PensionDisbursements/");
                var responseTalk = askPensionDisbursement.GetAsync("calculate?aadhaarNo="+inputFromPensionerBody.aadhaarNumber+
                    "&pensionAmount="+inputFromPensionerBody.PensionAmount+"&serviceCharge="+serviceCharge);
                responseTalk.Wait();

                //return Ok(responseTalk.Result.Content.ReadAsStringAsync().Result);
                int code = (int)responseTalk.Result.StatusCode;
                
                if(code==10)
                  {
                      var readTask = responseTalk.Result.Content.ReadAsStringAsync();

                      return ("Ok");
                  }
                return ("NotOK");
               
            }
           
        }
    }
}
