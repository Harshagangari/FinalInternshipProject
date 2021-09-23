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
        PensionerInput inputOfPensioner = new PensionerInput() {Name="bunk seenu",dateOfBirth= new DateTime(1990,01,02),PAN= "ABCD12351E",
            aadhaarNumber=123456789012,pensiontype=PensionType.family};
        [Route("EnterPensionDetails")]
        [HttpGet]
        public ActionResult<object> getPensionDetails(/*PensionerInput inputOfPensioner*/)
        {
            using (var askPensionerDetails = new HttpClient())
            {
                double pensionToDisburse = 0;
                askPensionerDetails.BaseAddress = new Uri("http://localhost:55345/api/pensionerdetails/");
                var responseTalk = askPensionerDetails.GetAsync("getById?aadharId=" + inputOfPensioner.PAN);
                responseTalk.Wait();
                var result =responseTalk.Result ;
                if (result.IsSuccessStatusCode)
                { 
                    var readTask =result.Content.ReadFromJsonAsync<string>();
                    readTask.Wait();
                    //JObject JsonRes = JObject.Parse(readTask.Result);
                    //var JsonResult = JsonConvert.DeserializeObject<JsonElement>(readTask.Result);
                    var obj = JsonConvert.DeserializeObject<dynamic>(readTask.Result);
                    return obj;
                }
            }
            return BadRequest("failed");
        }

        [HttpPost]
        public int processPensionInput(ProcessPensionInput inputFromPensioner)
        {
            return 0;
        }
    }
}
