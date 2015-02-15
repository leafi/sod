using Eto.Wpf.Forms;
using sod.EditorUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Eto.Drawing;
using OpenTK.Graphics;
using OpenTK.Platform;
using System.Threading;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SharpDX.WPF;
using System.Runtime.InteropServices;
using System.Reflection;

namespace sod.WPF
{

    // http://halogenica.net/sharing-resources-between-directx-and-opengl/
    // ^^ DING DING DING DING DING!!! Both AMD and Intel have implemented wgl_nv_dx_interop. (..interop2 for dx10+, but I really don't think that matters to us...)
    // A (GPU) copy is necessary, and a bit of synchronization, but I think we can pull it off.
    //
    // https://www.opengl.org/registry/specs/NV/DX_interop.txt includes sample code, for gods' sakes... how can we NOT do this.
    // 
    // don't forget the gpu stalling rules; halogenica says they apply here. ideally lock (frame n)'s render target when we've finished (frame n+1).

    // To actually render OpenGL offscreen we create an always-invisible native window.
    // (You're really not supposed to mix D3D and OpenGL in the same window, from what I can tell. DWM will spaz out.)

    // Re. multithreading: http://blog.gvnott.com/some-usefull-facts-about-multipul-opengl-contexts/
    // > You can however achieve significant performance improvements by using a second thread for data streaming (see the Performance section below).

    // ...actually, it's probably better to have one OpenGL thread and just do async uploads/downloads.


    // https://msdn.microsoft.com/en-us/library/cc656785(v=vs.110).aspx
    // ^ has LOTS of considerations, performance and otherwise, that we ignore here!!

    public class RenderBoxWPFOGLHandler : WpfFrameworkElement<DXElement, RenderBox, RenderBox.ICallback>, RenderBox.IRenderBox
    {
        [DllImport("user32.dll")]
        internal static extern IntPtr GetDC(IntPtr hwnd);

        IGraphicsContext oglContext;
        INativeWindow nw;

        private void createHiddenContext()
        {
            EventWaitHandle contextReady = new EventWaitHandle(false, EventResetMode.AutoReset);
            // TODO: ensure no context currently set! (i guess it shouldn't; everyone should release context when done.)

            nw = new NativeWindow();
            oglContext = new GraphicsContext(new GraphicsMode(), nw.WindowInfo); //, 3, 2, GraphicsContextFlags.Default);
            // TODO: should we be repeatedly calling nw.ProcessEvents() // nw.Sleep() in a new thread?
        }

        private void makeCurrent()
        {
            oglContext.GetOrThrow().MakeCurrent(nw.WindowInfo);
        }

        private void release()
        {
            oglContext.GetOrThrow().MakeCurrent(null);
        }

        public RenderBoxWPFOGLHandler()
        {
            createHiddenContext();

            makeCurrent();
            oglContext.LoadAll();
            nw.ProcessEvents();

            GL.ClearColor(0f, 0f, 1f, 1f);

            var wgl = new WGL_NV_DX_interop();

            release();

            var dxe = new DXElement();
            dxe.Renderer = new WPFOGLD3DRendererShim();
            dxe.Width = 400;
            dxe.Height = 300;

            this.Control = dxe;
        }

        public override Color BackgroundColor { get; set; }
    }
}
