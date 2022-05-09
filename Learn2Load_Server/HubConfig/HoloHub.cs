using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RealTimeCharts_Server.HubConfig
{
    public class HoloHub : Hub
    {

        //class of an object to be send as a JSON
        public class JsonToSend
        {
            public string destination { get; set; }
        }


        public async Task BroadcastHoloData(string data)
        {
            //convert the string data to an object
            JsonToSend dataAsObject = JsonSerializer.Deserialize<JsonToSend>(data);
            //Send the data to all clients connected to the hub
            await Clients.All.SendAsync("broadcastholodata", data);
        }

        public async Task BroadcastDataToAngular(string data)
        {
            JsonToSend dataAsObject = JsonSerializer.Deserialize<JsonToSend>(data);
            await Clients.Client(dataAsObject.destination).SendAsync("broadcastdatatoangular", data);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }

}
