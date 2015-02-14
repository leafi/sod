using Eto;
using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sod.EditorUI
{
    [Handler(typeof(IRenderBox))]
    public class RenderBox : Control
    {
        IRenderBox Handler { get { return (IRenderBox)base.Handler; } }

        // ... custom properties, forwarding {get, set} <-> this.Handler

        public interface IRenderBox : Control.IHandler
        {
            // ...
        }
    }
}
