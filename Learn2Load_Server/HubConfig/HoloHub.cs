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
        //Dictionnary common to all the != thread
        public static ConcurrentDictionary<string, string> MyClients = new ConcurrentDictionary<string, string>();

        //class of an object to be send as a JSON
        public class JsonToSend
        {
            public string destination { get; set; }
            public string source { get; set; }
            public int success { get; set; }
            public int error { get; set; }
            public float time { get; set; }

        }

        //Method automaticaly triggered when a client is disconnected
        public override Task OnDisconnectedAsync(Exception exception)
        {
            string idToRemove = Context.ConnectionId;
            var itemsToRemove = MyClients.Where(kvp => kvp.Value.Equals(idToRemove));

            //Remove the entry from the dictionnary
            foreach (var item in itemsToRemove)
                MyClients.TryRemove(item.Key, out idToRemove);

            return base.OnDisconnectedAsync(exception);
        }


        public async Task BroadcastHoloData(string data, string connectionId)
        {

            JsonToSend dataAsObject = JsonSerializer.Deserialize<JsonToSend>(data);

            //First connection of a client, used to record his connection id 
            if (dataAsObject.destination == null)
            {
                MyClients.TryAdd(dataAsObject.source, connectionId);
            }
            //request FROM UNITY TO ANGULAR
            else if (dataAsObject.destination.ToLower().Contains("angular"))
            {
                await Clients.Client(MyClients.FirstOrDefault(kvp => dataAsObject.destination.ToLower().Contains(kvp.Key)).Value).SendAsync("broadcastholodata", data);
            }
            //request FROM ANGULAR TO UNITY
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
