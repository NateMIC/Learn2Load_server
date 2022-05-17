using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Learn2LoadSignalR.HubConfig;

namespace Learn2LoadSignalR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoloController : ControllerBase
    {
        private IHubContext<HoloHub> _hub; 

        public HoloController(IHubContext<HoloHub> hub) 
        { 
            _hub = hub;
        }

        public IActionResult Get()
        {            
            return Ok(new { Message = "Request Completed" }); 
        }
    }
}