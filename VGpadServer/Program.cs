using Communication.EventSender;
using Communication.UDP;
using VHub;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("VGpad server is running...");
        Console.WriteLine("Listening on port 5222");

        var eventSender = new ReceiveEventSender();
        var UDPServer = new UDPServer(eventSender);
        var vhub = new GamepadHub();

        eventSender.PackageReceived += vhub.OnPackageReceived;
        
        UDPServer.StartListener();
    }
}