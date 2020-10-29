using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bandwidth.Standard.Messaging.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CallbacksController : ControllerBase
    {
        private readonly ILogger<CallbacksController> _logger;

        public CallbacksController(ILogger<CallbacksController> logger)
        {
            _logger = logger;
        }

        [HttpPost("messageCallback")]
        public async Task<ActionResult> Messages()
        {
            _logger.LogInformation("Received message callback request.");

            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();

            var message = JsonConvert.DeserializeObject<IEnumerable<BandwidthCallbackMessage>>(body).First();

            switch (message.Type)
            {
                case  "message-delivered":
                    _logger.LogInformation("Message delivered from Bandwidth's network.");
                    break;
                case "message-received":
                    _logger.LogInformation($"Message received from '{message.Message.From}' to '{string.Join(",", message.Message.To)}' with text '{message.Message.Text}'.");
                    break;
            }

            return new OkResult();
        }
    }
}
