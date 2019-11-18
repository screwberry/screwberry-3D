using UnityEngine;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System;
using UnityEngine.UI;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json.Linq;

public class EventHubsReceiver : MonoBehaviour
{
	private static EventHubClient eventHubClient;
    private static string ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
    private static string EntityPath = Environment.GetEnvironmentVariable("ENTITY_PATH");

    void Start() {
		var connectionStringBuilder = new EventHubsConnectionStringBuilder(ConnectionString){EntityPath = EntityPath};
		eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
        Initialize();
    }

    async void Initialize(){
        var runtimeInfo = await eventHubClient.GetRuntimeInformationAsync();
        var d2cPartitions = runtimeInfo.PartitionIds;

        CancellationTokenSource cts = new CancellationTokenSource();

        foreach (string partition in d2cPartitions)
        {
            ReceiveMessagesFromDeviceAsync(partition, cts.Token);
        }

        //await eventHubClient.CloseAsync();
    }

    private static async Task ReceiveMessagesFromDeviceAsync(string partition, CancellationToken ct)
    {
        var eventHubReceiver = eventHubClient.CreateReceiver("receive", partition, EventPosition.FromEnqueuedTime(DateTime.Now.AddMinutes(-10)));
        while (true)
        {
            if (ct.IsCancellationRequested) break;
            var events = await eventHubReceiver.ReceiveAsync(20);

            if (events != null) {
                foreach(EventData eventData in events)
                {
                    var data = JObject.Parse(Encoding.UTF8.GetString(eventData.Body.Array));
                    var label = GameObject.Find(data["alias"].ToString()).GetComponent<Text>();
                    label.text = data["alias"]
                    + "\n" + data["temperature"].ToString() + " °C"
                    + "\n" + data["humidity"].ToString() + "%";
                    Debug.Log(data);
                }
            }
        }
    }
}
