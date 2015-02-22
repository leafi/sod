using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace sod.OpenGL
{
    public class SimpleOGL
    {
        public SimpleOGL()
        {
        }

        public event Action<int, int, int, int> Resize = null;

        public void OnResize(int x, int y, int w, int h)
        {
            GL.Viewport(x, y, w, h);
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4f, (float)w / (float)h, 1f, 1000f);
            Matrix4 ortho = Matrix4.CreateOrthographic(w, h, 1f, 1000f);
            GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadMatrix(ref ortho);
            GL.LoadMatrix(ref perspective);

            // o de fake
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();

            if (Resize != null)
                Resize(x, y, w, h);
        }

        public void OnLoad()
        {
            OnResize(0, 0, 400, 300);
        }

        public void OnUnload()
        {

        }

        public void Render()
        {
            //GL.Viewport(0, 0, 400, 300);
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadIdentity();
            //GL.Ortho(0, 400, 300, 0, -1, 1);
            //GL.Disable(EnableCap.DepthTest);
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();

            Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);

            GL.ClearColor(0f, 1f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Begin(PrimitiveType.Triangles);
            GL.Color3(127, 127, 127);
            GL.Vertex2(100, 100);
            GL.Vertex2(100, 200);
            GL.Vertex2(0, 200);
            GL.Vertex2(100, 100);
            GL.Vertex2(100, 200);
            GL.Vertex2(200, 200);
            GL.End();
        }
    }
}

