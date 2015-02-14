using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sod.D3D11
{
    public class Renderer : sod.Graphics.IRenderer
    {
        public string Name
        {
            get { return "D3D11"; }
        }
    }
}
