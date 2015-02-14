using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.XInput;

namespace sod.XInput
{
    public class Joystick1 : sod.Input.IDriver
    {
        public int PollThreadFrequency = 5; // ms

        List<Controller> controllers = new List<Controller>();
        bool[] connected = new bool[4];
        int[] lastPacketNumber = new int[4];
        State[] tempState = new State[4];

        public ConcurrentQueue<Tuple<State?, State?, State?, State?>> Packets = new ConcurrentQueue<Tuple<State?, State?, State?, State?>>();


        public Input.DeviceType DeviceType
        {
            get { return Input.DeviceType.Xbox360Joystick; }
        }

        public string Name
        {
            get { return "some 360 controller via xinput"; }
        }

        public Joystick1()
        {
            // TODO: Wireless controllers start in 'OFF' state. We should also handle replugging of wired devices.
            // Basically should have more than one shot at initialization. Hmm.

            // 'For performance reasons, don't call XInputGetState for an 'empty' user slot every frame. 
            //  We recommend that you space out checks for new controllers every few seconds instead.'

            // ^ could listen for WM_DEVICECHANGE and only poll empty slots after that.
            // Polling empty slots is surprisingly slow. Can lose frames.

            //controllers.Add(new Controller(UserIndex.))

            controllers.Add(new Controller(UserIndex.One));
            controllers.Add(new Controller(UserIndex.Two));
            controllers.Add(new Controller(UserIndex.Three));
            controllers.Add(new Controller(UserIndex.Four));

            for (int i = 0; i < 4; i++)
            {
                connected[i] = controllers[i].IsConnected;
            }

            Console.WriteLine("Connected XInput controllers: " + connected.Count((x) => x));
            
        }

        internal void threadedUpdate()
        {
            State tmp;
            State?[] ss = new State?[4];
            int valid = 0;

            for (int i = 0; i < 4; i++)
            {
                if (!connected[i])
                    continue;

                if (!controllers[i].GetState(out tmp))
                    connected[i] = false;
                else if (tmp.PacketNumber != lastPacketNumber[i])
                {
                    lastPacketNumber[i] = tmp.PacketNumber;
                    ss[i] = tmp;
                    valid++;
                }
            }

            if (valid > 0)
                Packets.Enqueue(Tuple.Create(ss[0], ss[1], ss[2], ss[3]));
            
            // TODO: set queue max length? wouldn't like to just keep flooding if nothing's reading...
        }

        public void Update()
        {
            //var s = controllers[0].GetState();
            threadedUpdate();
            

            // use packet nubmer to see whether anything's changed

            // use better getstate overload & use ret value to see whether controller is actually connected

            // Your application should check for dead zones and respond appopriately.

            // Builtins: #define XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE  7849
            //#define XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE 8689
            //#define XINPUT_GAMEPAD_TRIGGER_THRESHOLD    30
            //state.Gamepad.
        }
    }
}
