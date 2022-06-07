using Garcia.Application.Contracts.Localization;
using Garcia.Infrastructure.Localization.Local.Sample.Models;
using Microsoft.AspNetCore.Mvc;

namespace Garcia.Infrastructure.Localization.Local.Sample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocalizationController : ControllerBase
    {
        private readonly ILocalizationService _localizationService;

        public LocalizationController(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        [HttpGet(Name = "Localize")]
        public async Task<IActionResult> SendPushNotification()
        {
            try
            {
                var model = new TestModel(1, null, "Non-localized text");
                await _localizationService.Localize("en", model);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}