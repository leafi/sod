using Eto.Mac.Forms;
using sod.EditorUI;
using MonoMac.AppKit;
using System;
using MonoMac.OpenGL;
using Eto.Drawing;
using OpenTK.Graphics;

namespace sod.Mac
{
    public class GLView : NSOpenGLView, IMacControl
    {
        //public GLView (RectangleF rect, NSOpenGLPixelFormat format) : base (rect, format) {
        //}

        public GLView()
        {
            /*Tao.Glfw.glfwOpenWindow(640, 480, 8, 8, 8, 8, 16, 0, Tao.Glfw.GLFW_WINDOW);
OpenTK.Graphics.GraphicsContext.CreateDummyContext();*/
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

        bool first = true;



        public override void DrawRect(System.Drawing.RectangleF dirtyRect)
        {
            this.OpenGLContext.MakeCurrentContext ();

            if (first)
            {
                first = false;


                /*float points[] = {
   0.0f,  0.5f,  0.0f,
   0.5f, -0.5f,  0.0f,
  -0.5f, -0.5f,  0.0f
};
GLuint vbo = 0;
glGenBuffers (1, &vbo);
glBindBuffer (GL_ARRAY_BUFFER, vbo);
glBufferData (GL_ARRAY_BUFFER, 9 * sizeof (float), points, GL_STATIC_DRAW);
GLuint vao = 0;
glGenVertexArrays (1, &vao);
glBindVertexArray (vao);
glEnableVertexAttribArray (0);
glBindBuffer (GL_ARRAY_BUFFER, vbo);
glVertexAttribPointer (0, 3, GL_FLOAT, GL_FALSE, 0, NULL);
const char* vertex_shader =
"#version 400\n"
"in vec3 vp;"
"void main () {"
"  gl_Position = vec4 (vp, 1.0);"
"}";
// version 3.3/4.1 yosemite, else 3.2

const char* fragment_shader =
"#version 400\n"
"out vec4 frag_colour;"
"void main () {"
"  frag_colour = vec4 (0.5, 0.0, 0.5, 1.0);"
"}";
GLuint vs = glCreateShader (GL_VERTEX_SHADER);
glShaderSource (vs, 1, &vertex_shader, NULL);
glCompileShader (vs);
GLuint fs = glCreateShader (GL_FRAGMENT_SHADER);
glShaderSource (fs, 1, &fragment_shader, NULL);
glCompileShader (fs);
GLuint shader_programme = glCreateProgram ();
glAttachShader (shader_programme, fs);
glAttachShader (shader_programme, vs);
glLinkProgram (shader_programme);

while (!glfwWindowShouldClose (window)) {
  // wipe the drawing surface clear
  glClear (GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
  glUseProgram (shader_programme);
  glBindVertexArray (vao);
  // draw points 0-3 from the currently bound VAO with current in-use shader
  glDrawArrays (GL_TRIANGLES, 0, 3);
  // update other events like input handling 
  glfwPollEvents ();
  // put the stuff we've been drawing onto the display
  glfwSwapBuffers (window);
}
*/


            }

            //GL.ClearColor(0.5f, 0f, 0f, 1f);

            //GL.ClearColor (0, 0, 0, 0);
            //GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Disable(EnableCap.CullFace);
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

            //DrawTriangle ();

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
                NSOpenGLPixelFormatAttribute.Accelerated,
                //NSOpenGLPixelFormatAttribute.OpenGLProfile, (NSOpenGLPixelFormatAttribute)0x3200,
                NSOpenGLPixelFormatAttribute.ColorSize, (NSOpenGLPixelFormatAttribute)24,
                NSOpenGLPixelFormatAttribute.AlphaSize, (NSOpenGLPixelFormatAttribute)8,
            };
        }

        public RenderBoxMacOGLHandler()
        {
            var glv = new GLView { Handler = this, PixelFormat = new NSOpenGLPixelFormat(ChoosePixelFormat()) };
            Control = glv;

            //glv.OpenGLContext.MakeCurrentContext();
            //new GraphicsContext(

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

