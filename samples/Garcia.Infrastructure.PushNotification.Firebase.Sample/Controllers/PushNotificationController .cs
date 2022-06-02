using Garcia.Application.Contracts.PushNotification;
using Garcia.Infrastructure.PushNotification.Firebase.Sample.Models;
using Microsoft.AspNetCore.Mvc;

namespace Garcia.Infrastructure.PushNotification.Firebase.Sample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PushNotificationController : ControllerBase
    {
        private readonly IPushNotificationService _pushNotificationService;

        public PushNotificationController(IPushNotificationService pushNotificationService)
        {
            _pushNotificationService = pushNotificationService;
        }

        [HttpPost(Name = "SendPushNotification")]
        public async Task<IActionResult> SendPushNotification([FromBody] SendPushNotificationModel model)
        {
            try
            {
                var response = await _pushNotificationService.SendPushNotificationAsync(model.Token, model.Title, model.Body);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}