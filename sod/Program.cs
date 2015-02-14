using Eto;
using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sod
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Mac only: ensure main form can be closed. Code in MainForm.cs makes this also quit the application.
            // (this probably needs to be pulled out into a mac-specific project)
#if MAC
            Style.Add<Eto.Mac.Forms.ApplicationHandler>(null, (h) => h.AllowClosingMainForm = true);
#endif

            
            new Eto.Forms.Application().Run(new EditorUI.MainForm());
        }
    }
}
