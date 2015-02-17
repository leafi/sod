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
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows;
using SharpDX.Direct3D9;

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

    public class RenderBoxWPFOGLHandler : WpfFrameworkElement<System.Windows.Controls.Image, RenderBox, RenderBox.ICallback>, RenderBox.IRenderBox
    {
        [DllImport("user32.dll", SetLastError = false)]
        private static extern IntPtr GetDesktopWindow();

        IGraphicsContext oglContext;
        INativeWindow nw;

        D3DImage d3dImage;

        private void createHiddenContext()
        {
            EventWaitHandle contextReady = new EventWaitHandle(false, EventResetMode.AutoReset);
            // TODO: ensure no context currently set! (i guess it shouldn't; everyone should release context when done.)

            nw = new NativeWindow();
            oglContext = new GraphicsContext(new GraphicsMode(), nw.WindowInfo, 3, 0, GraphicsContextFlags.Default);
            oglContext.VSync = true;
            // TODO: should we be repeatedly calling nw.ProcessEvents() // nw.Sleep() in a new thread?
        }

        private void makeCurrent()
        {
            oglContext.MakeCurrent(nw.WindowInfo);
        }

        private void release()
        {
            oglContext.MakeCurrent(null);
        }

        DeviceEx device;
        Surface colorBuffer;

        WGL_NV_DX_interop wgl;

        IntPtr wglHandleDevice;
        int glColorBuffer;
        IntPtr wglHandleColorBuffer;
        IntPtr[] singleWglHandleColorBufferArray;

        int glSharedSurface;
        IntPtr wglHandleSharedSurface;
        IntPtr[] singleWglHandleSharedSurfaceArray;

        private void createD3D9ExContext()
        {
            var d3d = new Direct3DEx();

            PresentParameters presentparams = new PresentParameters();
            presentparams.Windowed = true;
            presentparams.SwapEffect = SwapEffect.Discard;
            presentparams.DeviceWindowHandle = GetDesktopWindow();
            presentparams.PresentationInterval = PresentInterval.Default;
            // FpuPreserve for WPF
            // Multithreaded so that resources are actually sharable between DX and GL
            device = new DeviceEx(d3d, 0, DeviceType.Hardware, IntPtr.Zero, CreateFlags.HardwareVertexProcessing | CreateFlags.Multithreaded | CreateFlags.FpuPreserve, presentparams);

            // TODO: recreate render target when w, h change?
            IntPtr colorBufferShareHandle = IntPtr.Zero;
            colorBuffer = Surface.CreateRenderTargetEx(device, 400, 300, Format.A8R8G8B8, MultisampleType.None, 1, false, Usage.None, ref colorBufferShareHandle);
            var tex = new Texture(device, 400, 300, 1, Usage.RenderTarget, Format.A8R8G8B8, Pool.Default);
            colorBuffer = tex.GetSurfaceLevel(0);
            //IntPtr colorBufferShareHandle = IntPtr.Zero;
            //colorBuffer = Surface.CreateOffscreenPlainEx(device, 400, 300, Format.A8R8G8B8, Pool.Default, Usage.Dynamic, ref colorBufferShareHandle);
            //Console.WriteLine("ColorBufferShareHandle: " + colorBufferShareHandle.ToInt64());
            
            
            
            wgl = new WGL_NV_DX_interop();
            wglHandleDevice = wgl.WglDXOpenDeviceNV(device.NativePointer);

            glColorBuffer = GL.GenTexture();
            //glColorBuffer = GL.GenFramebuffer();

            var fbo = GL.GenFramebuffer();

            //Surface.CreateOffscreenPlainEx()
            //wgl.WglDXSetResourceShareHandleNV()
            if (!wgl.WglDXSetResourceShareHandleNV(colorBuffer.NativePointer, colorBufferShareHandle))
            {
                throw new Exception("failed wglDXSetResourceShareHandleNV");
            }

            wglHandleColorBuffer = wgl.WglDXRegisterObjectNV(wglHandleDevice, colorBuffer.NativePointer, (uint)glColorBuffer, (uint)TextureTarget.Texture2D, WGL_NV_DX_interop.WGL_ACCESS_WRITE_DISCARD_NV);
            singleWglHandleColorBufferArray = new IntPtr[] { wglHandleColorBuffer };

            //wgl.WglDXLockObjectsNV(wglHandleDevice, 1, singleWglHandleColorBufferArray);

            Console.WriteLine(GL.GetError());
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
            Console.WriteLine(GL.GetError());
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, glColorBuffer, 0);
            Console.WriteLine(GL.GetError());
            
            GL.DrawBuffer((DrawBufferMode)FramebufferAttachment.ColorAttachment0);
            

            //wgl.WglDXUnlockObjectsNV(wglHandleDevice, 1, singleWglHandleColorBufferArray);

            
            // depth, stencil attachment? cba

            // ??? might not need this in final!
            device.SetRenderTarget(0, colorBuffer);
        }

        public RenderBoxWPFOGLHandler()
        {
            createHiddenContext();

            makeCurrent();
            oglContext.LoadAll();
            nw.ProcessEvents();

            GL.ClearColor(0f, 0f, 1f, 1f);

            createD3D9ExContext(); // needs OGL context set

            release();

            d3dImage = new D3DImage();
            //dxe.IsFrontBufferAvailableChanged <-- TODO: subscribe to this event if you only care about updating on e.g. button click!

            // fires 'just before objects in the composition tree are rendered'
            CompositionTarget.Rendering += new EventHandler(OnRender);

            // TODO: _sizeTimer

            //dxe.Renderer = new WPFOGLD3DRendererShim();
            //dxe.Width = 400;
            //dxe.Height = 300;

            //this.Control = d3dImage;
            var wpfImage = new System.Windows.Controls.Image();
            wpfImage.Source = d3dImage;
            this.Control = wpfImage;
        }

        protected TimeSpan lastRenderTime;

        protected virtual void OnRender(object sender, EventArgs e)
        {
            RenderingEventArgs args = (RenderingEventArgs)e;

            // OnRender may be called twice in the same frame. Only render the first time.
            if (d3dImage.IsFrontBufferAvailable && lastRenderTime != args.RenderingTime)
            {
                // IntPtr pSurface = IntPtr.Zero;

                // // !!! SET PSURFACE !!!



                d3dImage.Lock();
                d3dImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, colorBuffer.NativePointer);

                // !!! DO RENDER? !!!
                device.Clear(ClearFlags.Target, new SharpDX.ColorBGRA(1f, 1f, 0f, 1f), 0f, 0);
                
                device.Present();

                makeCurrent();
                wgl.WglDXLockObjectsNV(wglHandleDevice, 1, singleWglHandleColorBufferArray);
                GL.Viewport(0, 0, 400, 300);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();
                GL.Ortho(0, 400, 300, 0, -1, 1);
                GL.Disable(EnableCap.DepthTest);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();
                GL.ClearColor(0f, 1f, 1f, 1f);
                GL.Clear(ClearBufferMask.ColorBufferBit);
                GL.Begin(BeginMode.Triangles);
                GL.Color3(127, 127, 127);
                GL.Vertex2(100, 100);
                GL.Vertex2(100, 200);
                GL.Vertex2(0, 200);
                GL.Vertex2(100, 100);
                GL.Vertex2(100, 200);
                GL.Vertex2(200, 200);
                GL.Finish();
                //GL.Flush();
                oglContext.SwapBuffers();

                wgl.WglDXUnlockObjectsNV(wglHandleDevice, 1, singleWglHandleColorBufferArray);
                release();


                
                // !!!!!!

                d3dImage.AddDirtyRect(new Int32Rect(0, 0, d3dImage.PixelWidth, d3dImage.PixelHeight));
                d3dImage.Unlock();

                lastRenderTime = args.RenderingTime;
            }
        }

        public override Eto.Drawing.Color BackgroundColor { get; set; }
    }
}
