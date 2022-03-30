using Azure.Identity;
using Azure.Messaging.ServiceBus;
using System.Text;

ServiceBusClient svcClient = new ServiceBusClient("sessiondemoanish.servicebus.windows.net", new DefaultAzureCredential());

#region Processor
//ServiceBusReceiver ServiceBusReceiver = svcClient.CreateReceiver("anishq");
//ServiceBusSessionProcessor processor = svcClient.CreateSessionProcessor("anishq", new ServiceBusSessionProcessorOptions
//{
//    MaxConcurrentCallsPerSession = 1,
//    AutoCompleteMessages = false,
//});

//processor.ProcessMessageAsync += async (processSessionMessageEventArgs) =>
//{
//    Console.WriteLine($"Session Id: {processSessionMessageEventArgs.SessionId}, Message: {processSessionMessageEventArgs.Message}");
//    await processSessionMessageEventArgs.CompleteMessageAsync(processSessionMessageEventArgs.Message);
//};

//processor.ProcessErrorAsync += async (errorEventMesage) =>
//{
//    Console.WriteLine(errorEventMesage.Exception.Message);
//    await Task.CompletedTask;
//};
//await processor.StartProcessingAsync(); 
#endregion

#region Sender
ServiceBusSender ServiceBusClient = svcClient.CreateSender("anishq");

string sessions = string.Empty;
for (int i = 1; i < 11; i++)
{
    sessions = (i % 2 == 0) ? "even session" : "Odd-Session";
    await ServiceBusClient.SendMessageAsync(new ServiceBusMessage($"Message {i}")
    {
        SessionId = sessions
    });
}
#endregion

// var reciever =await  svcClient.AcceptNextSessionAsync("anishq", new ServiceBusSessionReceiverOptions
//{
//     ReceiveMode=ServiceBusReceiveMode.ReceiveAndDelete
//});

//await foreach (var item in reciever.ReceiveMessagesAsync())
//{
//    var data = await new StreamReader(item.Body.ToStream()).ReadToEndAsync();
//    Console.WriteLine($"Even: SessionId: {item.SessionId}, Data: {data} ");

//}

#region Receive from a specific session
var evenReceiver = await svcClient.AcceptSessionAsync("anishq", "even session", new ServiceBusSessionReceiverOptions
{
    ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete
});

await foreach (var item in evenReceiver.ReceiveMessagesAsync())
{
    var data = await new StreamReader(item.Body.ToStream()).ReadToEndAsync();
    Console.WriteLine($"Even: SessionId: {item.SessionId}, Data: {data} ");
}

//var oddReceiver = await svcClient.AcceptSessionAsync("anishq", "Odd-Session", new ServiceBusSessionReceiverOptions
//{
//    ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete
//});
//await foreach (var item in oddReceiver.ReceiveMessagesAsync())
//{
//    var data = await new StreamReader(item.Body.ToStream()).ReadToEndAsync();
//    Console.WriteLine($"Odd: SessionId: {item.SessionId}, Data: {data} ");
//} 
#endregion

Console.WriteLine("Press to exit");
Console.ReadLine();
//await processor.StopProcessingAsync();
Console.ReadKey();