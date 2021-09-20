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

        [HttpPost("inbound/messaging")]
        public async Task<ActionResult> MessagesInbound()
        {
            _logger.LogInformation("Received message callback request.");

            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();

            var message = JsonConvert.DeserializeObject<IEnumerable<BandwidthCallbackMessage>>(body).First();

            switch (message.Type)
            {
                case "message-received":
                    _logger.LogInformation($"Message received from '{message.Message.From}' to '{string.Join(",", message.Message.To)}' with text '{message.Message.Text}'.");
                    break;
                default:
                    _logger.LogInformation("Message type does not match endpoint. This endpoint is used for inbound messages only.\n      Outbound message callbacks should be sent to /callbacks/outbound/messaging.");
                    break;
            }

            return new OkResult();
        }

        [HttpPost("outbound/messaging")]
        public async Task<ActionResult> MessagesOutbound()
        {
            _logger.LogInformation("Received message callback request.");

            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();

            var message = JsonConvert.DeserializeObject<IEnumerable<BandwidthCallbackMessage>>(body).First();

            switch (message.Type)
            {
                case "message-delivered":
                    _logger.LogInformation("Message delivered from Bandwidth's network.");
                    break;
                case "message-sending":
                    _logger.LogInformation("message-sending type is only for MMS.");
                    break;
                case "message-failed":
                    _logger.LogInformation("For MMS and Group Messages, you will only receive this callback if you have enabled delivery receipts on MMS.");
                    break;
                default:
                    _logger.LogInformation("Message type does not match endpoint. This endpoint is used for outbound status callbacks only.\n      Inbound message callbacks should be sent to /callbacks/inbound/messaging.");
                    break;
            }

            return new OkResult();
        }
    }
}
