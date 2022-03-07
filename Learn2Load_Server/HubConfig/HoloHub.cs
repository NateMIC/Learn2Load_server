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
        public static string connectionIdHololens;
        public static string connectionIdAngular;
        public static ConcurrentDictionary<string, string> MyUsers = new ConcurrentDictionary<string, string>();

        public class JsonToSend
        {
            public string destination { get; set; }
            public string source { get; set; }
            public int success { get; set; }
            public int error { get; set; }
            public float time { get; set; }

        }


        public async Task BroadcastHoloData(string data, string connectionId)
        {
            JsonToSend test = JsonSerializer.Deserialize<JsonToSend>(data);
            if (test.source == "hololens" && test.destination == null)
            {
                MyUsers.TryAdd("connectionIdHololens", connectionId);
                Console.WriteLine("hololens : " + connectionId);
            }
            else if (test.source == "angular" && test.destination == null)
            {
                MyUsers.TryAdd("connectionIdAngular", connectionId);
                Console.WriteLine("angular : " + connectionId);
            }
            else if (test.destination.ToLower().Contains("angular"))
            {
                await Clients.Client(MyUsers.FirstOrDefault(kvp => kvp.Key == "connectionIdAngular").Value).SendAsync("broadcastholodata", data);
            }
            else
            {
                await Clients.Client(MyUsers.FirstOrDefault(kvp => kvp.Key == "connectionIdHololens").Value).SendAsync("broadcastholodata", data);
            }
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }

}
