using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sod.Input
{
    public enum DeviceType { Xbox360Joystick, DInputJoystick, Keyboard, Mouse }
    public interface IDriver
    {
        DeviceType DeviceType { get; }
        string Name { get; }
        void Update();
    }



    public static class Drivers
    {
        public static List<IDriver> Drivers = new List<IDriver>();
        public static IDriver Active { get; private set; }



        public static void Pick()
        {
            Active = Drivers[0];
        }
    }
}
