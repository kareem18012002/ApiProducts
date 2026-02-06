using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace NoRiskNoFun.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IOptionsMonitor<AttachmentOptions> _attachmentOptions;
        // IOptions ----IOptionsSnapshot----IOptionsMonitor
        // IOptions de lw 3delt mo4 hyban 8yer ma t3mel run
        // IOptionsSnapshot de bt3del m3 kol request y3ny bt3mel refresh fe  el  el gded
        // IOptionsMonitor de bt3del kol ma ykon fe t3del y3ny bt3mel refresh fe nafes el request
        public ConfigController(IConfiguration configuration , IOptionsMonitor<AttachmentOptions> attachmentOptions)
        {
            _configuration = configuration;
            _attachmentOptions = attachmentOptions;
            //var value = attachmentOptions.Value;
            var value = attachmentOptions.CurrentValue;


        }

        [HttpGet]
        [Route("")]
        public ActionResult GetConfig()
        {
            var config = new        
            {
                AllowedHosts = _configuration["AllowedHosts"],
                DefaultConnection = _configuration["ConnectionStrings:DefaultConnection"],
                DefaultLogging = _configuration["Logging:LogLevel:Default"],
                Testkey = _configuration["TestKey"],
                SigningKey = _configuration["SigningKey"],
                AttachmentsOption= _attachmentOptions.CurrentValue,


            };
            return Ok(config);
        }
    }
}
