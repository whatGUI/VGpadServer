using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;

namespace VController
{
    public class Controller
    {
        private readonly IXbox360Controller xbox360Controller;


        public Controller()
        {
            var client = new ViGEmClient();
            xbox360Controller = client.CreateXbox360Controller();
            xbox360Controller.Connect();

        }
        ~Controller()
        {
            xbox360Controller.Disconnect();
        }

        public void Control(string controlType, bool isPressed, double x, double y)
        {
            if (controlType == "LEFT_AXIS")
            {
                xbox360Controller.SetAxisValue(0, (short)(32767 * x));
                xbox360Controller.SetAxisValue(1, (short)(32767 * y));
            }
            else if (controlType == "RIGHT_AXIS")
            {
                xbox360Controller.SetAxisValue(2, (short)(32767 * x));
                xbox360Controller.SetAxisValue(3, (short)(32767 * y));
            }
            else if (ButtonMap.ContainsKey(controlType))
            {
                xbox360Controller.SetButtonState(ButtonMap[controlType], isPressed);
            }
            else if (controlType == "LEFT_TRIGGER")
            {
                if (isPressed)
                {
                    xbox360Controller.SetSliderValue(0, byte.MaxValue);
                }
                else
                {
                    xbox360Controller.SetSliderValue(0, 0);
                }

            }
            else if (controlType == "RIGHT_TRIGGER")
            {
                if (isPressed)
                {
                    xbox360Controller.SetSliderValue(1, byte.MaxValue);
                }
                else
                {
                    xbox360Controller.SetSliderValue(1, 0);
                }
            };

        }

        private Dictionary<string, int> ButtonMap = new Dictionary<string, int>()
        {
            { "DPAD_UP", 0 },
            { "DPAD_DOWN", 1 },
            { "DPAD_LEFT", 2 },
            { "DPAD_RIGHT", 3 },
            { "START", 4 },
            { "BACK", 5 },
            { "LEFT_THUMB", 6 },
            { "RIGHT_THUMB", 7 },
            { "LEFT_SHOULDER", 8 },
            { "RIGHT_SHOULDER", 9 },
            { "GUIDE", 10 },
            { "A", 11 },
            { "B", 12 },
            { "X", 13 },
            { "Y", 14 },
        };
    }
}