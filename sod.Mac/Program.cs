using Eto.Forms;
using Eto.Mac;
using sod.EditorUI;
using System;
using Eto;

namespace sod.Mac
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // Mac only: ensure main form can be closed. Code in MainForm.cs makes this also quit the application.
            Style.Add<Eto.Mac.Forms.ApplicationHandler>(null, (h) => h.AllowClosingMainForm = true);

            var platform = new Eto.Mac.Platform();
            platform.Add<RenderBox.IRenderBox>(() => new RenderBoxMacOGLHandler());

            /*sod.Graphics.Renderer.Renderers.Add(new sod.D3D11.Renderer());
            sod.Graphics.Renderer.Pick();*/

            new Application(platform).Run(new MainForm());
        }
    }
}

