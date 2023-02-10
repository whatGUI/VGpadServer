using Communication.EventSender;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Communication.UDP
{
    public class UDPServer
    {
        private readonly int listenPort;
        private readonly UdpClient udp;
        private readonly ReceiveEventSender eventSender;
        private PackageReceivedEventArgs eventArgs;

        public UDPServer(ReceiveEventSender sender, int port = 5222)
        {
            listenPort = port;
            udp = new UdpClient(listenPort);
            eventSender = sender;
            eventArgs = new PackageReceivedEventArgs();
        }

        public void StartListener()
        {
            var groupEP = new IPEndPoint(IPAddress.Any, 0);

            try
            {
                while (true)
                {

                    //Console.WriteLine($"Listening on UDP port: {listenPort} ...");
                    var rec = udp.Receive(ref groupEP);
                    //
                    // 此处可能会解码失败导致程序中止，后续来此检查
                    //
                    string msg = Encoding.ASCII.GetString(rec, 0, rec.Length);

                    if (msg == "FIND_VGPAD_SERVER")
                    {
                        Console.WriteLine($"Received broadcast from {groupEP} :");
                        var jsonStr = JsonSerializer.Serialize(new ResponseData() { status = true });
                        var sendMsg = Encoding.ASCII.GetBytes(jsonStr);
                        udp.Send(sendMsg, sendMsg.Length, groupEP);
                    }
                    else
                    {
                        Console.WriteLine($"Instruction: {msg}");
                        DecodeUDPData(msg);
                        eventSender.SendReceivedEvent(eventArgs);
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                udp?.Dispose();
            }
        }

        public void DecodeUDPData(string data)
        {
            int player;
            string type;
            bool isPressed = false;
            double x = 0;
            double y = 0;
            using (var jsonData = JsonDocument.Parse(data))
            {
                JsonElement root = jsonData.RootElement;
                player = root.GetProperty("player").GetInt32();
                type = root.GetProperty("type").GetString()!;

                JsonElement args = root.GetProperty("args")[0];

                if (args.ValueKind == JsonValueKind.Object)
                {
                    x = args.GetProperty("x").GetDouble();
                    y = args.GetProperty("y").GetDouble();
                }
                else
                {
                    isPressed = args.GetBoolean();
                }
            };
            eventArgs.PlayerID = player;
            eventArgs.ControlType = type;
            eventArgs.IsPressed = isPressed;
            eventArgs.X = x;
            eventArgs.Y = y;
        }

        class ResponseData
        {
            public bool status { get; set; }
        }
    }
}
