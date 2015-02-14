using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sod.Graphics
{
    public static class Renderer
    {
        public static List<IRenderer> Renderers = new List<IRenderer>();
        public static IRenderer Active { get; private set; }

        public static void Pick()
        {
            Active = Renderers[0];
        }
    }
}
