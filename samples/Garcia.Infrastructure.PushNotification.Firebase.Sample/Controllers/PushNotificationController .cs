using Garcia.Application.Contracts.PushNotification;
using Garcia.Infrastructure.PushNotification.Firebase.Sample.Models;
using Microsoft.AspNetCore.Mvc;

namespace Garcia.Infrastructure.PushNotification.Firebase.Sample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PushNotificationController : ControllerBase
    {
        private readonly ILogger<PushNotificationController> _logger;
        private readonly IPushNotificationService _pushNotificationService;

        public PushNotificationController(ILogger<PushNotificationController> logger, IPushNotificationService pushNotificationService)
        {
            _logger = logger;
            _pushNotificationService = pushNotificationService;
        }

        [HttpPost(Name = "SendPushNotification")]
        public async Task<IActionResult> SendPushNotification([FromBody] SendPushNotificationModel model)
        {
            var response = await _pushNotificationService.SendPushNotificationAsync(model.Token, model.Title, model.Body);
            return Ok(response);
        }
    }
}