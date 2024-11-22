using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dapr.Client;
using System.Text.Json.Serialization;

namespace ADFDaprDemo.Controllers
{
    public class RequestData
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("value")]
        public Value Value { get; set; }
    }

    public class Value
    {
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; }
    }

    [ApiController]
    [Route("api/calladf")]
    public class MyController : ControllerBase
    {
        private readonly DaprClient _daprClient;

        public MyController(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] object request)
        {
            // create a new request object
            var newRequest = new RequestData
            {
                Data = new Data
                {
                    Value = new Value
                    {
                        OrderId = "Order22"
                    }
                }
            };

            try
            {
                var response = await _daprClient.InvokeMethodAsync<object, object>(
                    appId: "adfdaprdemo-funcapp",
                    methodName: "CreateNewOrder",
                    data: newRequest
                );

                return Ok(new { Message = "Post request received" });
            }
            catch (Exception ex) {
                return StatusCode(500, new { Error = "An error occurred while invoking Dapr service", Details = ex.Message });
            }
        }
    }
}