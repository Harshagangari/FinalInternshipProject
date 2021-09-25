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
            PensionerInput input = new PensionerInput()
            {
                Name = "bunk seenu",
                dateOfBirth = new DateTime(1990, 01, 02),
                PAN = "ABCD12351E",
                aadhaar = 123456,
                selforfamily = 0
            };
            //calculatefromdetails(5000, input);



        }

        [Route("calculate")]
        [HttpGet]
        public StatusCodeResult calculatefromdetails([FromBody]string inputFromPensioner)
        {
            using(var response = new HttpClient())
              {
                //var jsondes =JsonConvert.DeserializeObject(inputFromPensioner);

                  JsonDocument jd = JsonDocument.Parse(inputFromPensioner);
                  response.BaseAddress = new Uri("http://localhost:55345/api/pensionerdetails/");
                  var responseTalk = response.GetAsync("getById?aadharID="+jd.RootElement.GetProperty("aadhaarNumber"));
                  responseTalk.Wait();
                  var result = responseTalk.Result;
                  if(result.IsSuccessStatusCode)
                  {
                      var readTalk = result.Content.ReadAsStringAsync();
                      readTalk.Wait();
                      return new StatusCodeResult(10);
                  }
                return new StatusCodeResult(21);
            }
        }
    }
}
