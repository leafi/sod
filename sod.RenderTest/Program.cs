using OpenTK;
using sod.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sod.RenderTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleOGL ogl = new SimpleOGL();

            using (var gw = new GameWindow())
            {
                gw.VSync = VSyncMode.On;

                gw.Load += (sender, ev) => { ogl.OnLoad(); };
                gw.Unload += (sender, ev) => { ogl.OnUnload(); };

                gw.Resize += (sender, ev) => { ogl.OnResize(gw.ClientRectangle.Left, gw.ClientRectangle.Top, gw.ClientRectangle.Width, gw.ClientRectangle.Height); };

                gw.RenderFrame += (sender, ev) => { ogl.Render(); gw.SwapBuffers(); };
                gw.UpdateFrame += (sender, ev) => { };

                gw.Run(60.0); // logic up to 60fps & render up to refresh rate. TODO: detect vsync/give option to player?
            }
        }
    }
}
