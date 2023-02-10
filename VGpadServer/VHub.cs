using Communication.EventSender;
using VController;

namespace VHub
{
    class GamepadHub
    {
        private Controller[] controllers = new Controller[4];

        private void PlugInHub(int playerID)
        {
            if (controllers[playerID] == null)
            {
                controllers[playerID] = new Controller();
            }
        }

        private void SendInstruction(PackageReceivedEventArgs e)
        {
            controllers[e.PlayerID].Control(e.ControlType!, e.IsPressed, e.X, e.Y);
        }

        public void OnPackageReceived(object? sender, PackageReceivedEventArgs e)
        {
            PlugInHub(e.PlayerID);
            SendInstruction(e);
        }
    }
}

