using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sod.Input
{
    public enum Sticks
    {
        Left,
        Right
    }

    public enum Button
    {
        A,
        B,
        Start
    }

    // TODO: Make this suitable for multiple, odd input sources! Actually do some damn mapping.
    // TODO: one Map per player? we're only single-player right now (i assume), so i'm just taking the easy route.

    // Philosophy:
    // -> all input comes in as precise as possible. e.g. timestamped XInput controller states. Usually >1/frame!
    // -> Map is responsible for collating events, preserving what's important, and discarding what isn't -
    //     while still providing a convenient programming interface.
    // 
    // This class will likely be completely different for different games.
    class Map
    {

    }
}
