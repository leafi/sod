using SharpDX;
using SharpDX.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sod.WPF
{
    /// <summary>
    /// SharpDX.WPF's design is very strange. This bridges the gap until it can be refactored.
    /// </summary>
    public class WPFOGLD3DRendererShim : SharpDX.WPF.D3D9
    {

        public override void RenderScene(DrawEventArgs args)
        {
            //base.RenderScene(args);
            //var context = Device.ImmediateContext;
            //Device.SetRenderTarget()
            Device.Clear(SharpDX.Direct3D9.ClearFlags.Target, new ColorBGRA(1f, 0f, 0f, 1f), 0f, 0);
            //context.ClearRenderTargetView(RenderTargetView, new Color4(0f, 1f, 0f, 1f));

            // TODO: copy from OGL!

        }
    }
}
