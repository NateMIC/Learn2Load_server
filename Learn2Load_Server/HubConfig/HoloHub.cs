using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RealTimeCharts_Server.HubConfig
{
    public class HoloHub : Hub
    {
        public async Task BroadcastHoloData(string data)
        {
            await Clients.All.SendAsync("broadcastholodata", data);
            Debug.WriteLine(data);
        }
    }
}
