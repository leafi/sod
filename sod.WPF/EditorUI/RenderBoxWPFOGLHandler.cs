using Eto.Wpf.Forms;
using sod.EditorUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Eto.Drawing;
using SharpGL.WPF;

namespace sod.WPF
{
    // http://www.codeproject.com/Articles/26425/WPF-Bitmap-Effects-using-Pixel-Shaders-Net-sp-B
    // ^ sort of a way in to writing shaders on wpf?

    // http://jmorrill.hjtcentral.com/Home/tabid/428/EntryId/438/How-to-get-access-to-WPF-s-internal-Direct3D-guts.aspx
    // ^ purest evil: havking into IDirect3DSwapChain9::Present in memory.

    // http://halogenica.net/sharing-resources-between-directx-and-opengl/
    // ^^ DING DING DING DING DING!!! Both AMD and Intel have implemented wgl_nv_dx_interop. (..interop2 for dx10+, but I really don't think that matters to us...)
    // A (GPU) copy is necessary, and a bit of synchronization, but I think we can pull it off.
    //
    // https://www.opengl.org/registry/specs/NV/DX_interop.txt includes sample code, for gods' sakes... how can we NOT do this.
    // 
    // don't forget the gpu stalling rules; halogenica says they apply here. ideally lock (frame n)'s render target when we've finished (frame n+1).
    public class RenderBoxWPFOGLHandler : WpfFrameworkElement<OpenGLControl, RenderBox, RenderBox.ICallback>, RenderBox.IRenderBox
    {
        public RenderBoxWPFOGLHandler()
        {
            var ogc = new OpenGLControl();
            ogc.OpenGLDraw += new SharpGL.SceneGraph.OpenGLEventHandler((o, ea) => { ea.OpenGL.ClearColor(0f, 0f, 1f, 1f); ea.OpenGL.Clear(SharpGL.OpenGL.GL_COLOR_BUFFER_BIT); });
            ogc.RenderContextType = SharpGL.RenderContextType.NativeWindow;
            this.Control = ogc;
        }

        public override Color BackgroundColor { get; set; }
    }
}
