using Eto.Forms;
using Eto.Wpf;
using sod.EditorUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sod.WPF
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Mac only: ensure main form can be closed. Code in MainForm.cs makes this also quit the application.
            // (this probably needs to be pulled out into a mac-specific project)
#if MAC
            Style.Add<Eto.Mac.Forms.ApplicationHandler>(null, (h) => h.AllowClosingMainForm = true);
#endif

            var platform = new Platform();
            platform.Add<RenderBox.IRenderBox>(pickRenderBox);

            sod.Graphics.Renderer.Renderers.Add(new sod.D3D11.Renderer());
            sod.Graphics.Renderer.Pick();

            new Application(platform).Run(new MainForm());
        }

        static RenderBox.IRenderBox pickRenderBox()
        {
            return sod.Graphics.Renderer.Active is sod.D3D11.Renderer ? (RenderBox.IRenderBox) new RenderBoxWPFD3DHandler() : new RenderBoxWPFOGLHandler();
        }
    }
}
