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
        public static ConcurrentDictionary<string, string> MyClients = new ConcurrentDictionary<string, string>();

        public class JsonToSend
        {
            public string destination { get; set; }
            public string source { get; set; }
            public int success { get; set; }
            public int error { get; set; }
            public float time { get; set; }

        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string idToRemove = Context.ConnectionId;
            var itemsToRemove = MyClients.Where(kvp => kvp.Value.Equals(idToRemove));

            foreach (var item in itemsToRemove)
                MyClients.TryRemove(item.Key, out idToRemove);

            return base.OnDisconnectedAsync(exception);
        }


        public async Task BroadcastHoloData(string data, string connectionId)
        {
            JsonToSend dataAsObject = JsonSerializer.Deserialize<JsonToSend>(data);

            if (dataAsObject.destination == null)
            {
                MyClients.TryAdd(dataAsObject.source, connectionId);
            }
            else if (dataAsObject.destination.ToLower().Contains("angular"))
            {
                await Clients.Client(MyClients.FirstOrDefault(kvp => dataAsObject.destination.ToLower().Contains(kvp.Key)).Value).SendAsync("broadcastholodata", data);
            }
            else
            {
                await Clients.Client(MyClients.FirstOrDefault(kvp => kvp.Key == dataAsObject.destination).Value).SendAsync("broadcastholodata", data);
            }
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }

}
