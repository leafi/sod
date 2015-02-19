using OpenTK.Graphics.OpenGL;
using System;

namespace sod.OpenGL
{
    public class SimpleOGL
    {
        public SimpleOGL()
        {
        }

        public void Render()
        {
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
        }
    }
}

