using Eto.Mac.Forms;
using sod.EditorUI;
using MonoMac.AppKit;
using System;
using MonoMac.OpenGL;
using Eto.Drawing;

namespace sod.Mac
{
    public class GLView : NSOpenGLView, IMacControl
    {
        //public GLView (RectangleF rect, NSOpenGLPixelFormat format) : base (rect, format) {
        //}

        public GLView()
        {
        }

        static void DrawTriangle ()
        {
            GL.Color3 (1.0f, 0.85f, 0.35f);
            GL.Begin (BeginMode.Triangles);

            GL.Vertex3 (0.0, 0.6, 0.0);
            GL.Vertex3 (-0.2, -0.3, 0.0);
            GL.Vertex3 (0.2, -0.3 ,0.0);

            GL.End ();
        }

        public override void DrawRect(System.Drawing.RectangleF dirtyRect)
        {
            this.OpenGLContext.MakeCurrentContext ();

            //GL.ClearColor(0.5f, 0f, 0f, 1f);

            GL.ClearColor (0, 0, 0, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            DrawTriangle ();

            GL.Flush ();
        }

        public WeakReference WeakHandler { get; set; }

        public object Handler
        { 
            get { return WeakHandler.Target; }
            set { WeakHandler = new WeakReference(value); } 
        }
    }

    public class RenderBoxMacOGLHandler : MacView<GLView, RenderBox, RenderBox.ICallback>, RenderBox.IRenderBox
    {
        public virtual NSOpenGLPixelFormatAttribute[] ChoosePixelFormat()
        {
            return new NSOpenGLPixelFormatAttribute[] {
                NSOpenGLPixelFormatAttribute.MinimumPolicy
            };
        }

        public RenderBoxMacOGLHandler()
        {
            Control = new GLView { Handler = this, PixelFormat = new NSOpenGLPixelFormat(ChoosePixelFormat()) };
            //Eto.Mac.Forms.mac
            //new Eto.Drawing.RectangleFConverter(

            //this.Control = new GLView(
        }

        public override Eto.Drawing.SizeF GetPreferredSize(Eto.Drawing.SizeF availableSize)
        {
            return base.GetPreferredSize(availableSize);
        }

        public override bool Enabled { get; set; }

        public override NSView ContainerControl { get { return Control; } }

        //public override RenderBox.IRenderBox.
    }
}

