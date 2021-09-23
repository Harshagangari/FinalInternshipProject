using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using PensionDisbursment.Entities;

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
        public int calculatefromdetails()
        {
            using(var response = new HttpClient())
            {
                var id = "ABCD12351E";
                response.BaseAddress = new Uri("http://localhost:55345/api/pensionerdetails/");
                var responseTalk = response.GetAsync("getById?aadharId=" + id);
                responseTalk.Wait();
                var result = responseTalk.Result;
                if(result.IsSuccessStatusCode)
                {
                    var readTalk = result.Content.ReadAsStringAsync();
                    readTalk.Wait();
                    return (10);
                }
            }
            return (21);
            
        }
    }
}
