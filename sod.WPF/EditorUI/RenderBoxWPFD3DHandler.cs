using Eto.Wpf.Forms;
using sod.EditorUI;
using SharpDX.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Eto.Drawing;

// TODO: http://stackoverflow.com/questions/27894051/need-sharpdxelement-alternative-workaround-to-sharpdx-wpf-flicker ?

namespace sod.WPF
{
    public class RenderBoxWPFD3DHandler : WpfFrameworkElement<DXElement, RenderBox, RenderBox.ICallback>, RenderBox.IRenderBox
    {

        // An alternative to using SlimDX.WPF:
        // http://jmorrill.hjtcentral.com/Home/tabid/428/EntryId/437/Direct3D-10-11-Direct2D-in-WPF.aspx
        // It's a wrapper around D3DImage called D3DImageEx, that lets you use D3D10/11 shared render targets straight up.
        // Apparently needs a bit of bulletproofing, though... (device resets?)

        public RenderBoxWPFD3DHandler()
        {
            var dxe = new DXElement();
            dxe.Renderer = new WPFD3DRendererShim();
            dxe.Width = 400;
            dxe.Height = 300;
            
            this.Control = dxe;
        }

        public override Color BackgroundColor { get; set; }
    }
}
