using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RealTimeCharts_Server.HubConfig
{
    public class ChartHub : Hub
    {
        public async Task BroadcastChartData(string data)
        {
            await Clients.All.SendAsync("broadcastchartdata", data);
            Debug.WriteLine(data);
        }
    }
}
