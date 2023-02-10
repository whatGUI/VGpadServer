namespace Communication.EventSender
{
    public class ReceiveEventSender
    {
        public event EventHandler<PackageReceivedEventArgs>? PackageReceived;

        public void SendReceivedEvent(PackageReceivedEventArgs eventArgs)
        {
            if (PackageReceived != null)
            {
                PackageReceived(this, eventArgs);
            }
            else
            {
                Console.WriteLine("没有事件订阅者！");
            }
        }
    }

    public class PackageReceivedEventArgs : EventArgs
    {
        public int PlayerID { get; set; }
        public string? ControlType { get; set; }
        public bool IsPressed { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}
