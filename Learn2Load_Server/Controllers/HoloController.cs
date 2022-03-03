using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RealTimeCharts_Server.HubConfig;

namespace RealTimeCharts_Server.Controllers
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