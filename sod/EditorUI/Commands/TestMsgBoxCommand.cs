using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sod.EditorUI.Commands
{
    public class TestMsgBoxCommand : Command
    {
        protected Control parent;

        public TestMsgBoxCommand(Control parent)
        {
            this.parent = parent;
            MenuText = "&Test message box";
            ToolBarText = "Test Box";
            ToolTip = "Just does a thing, y'know?";
            //Image = Icon.FromResource ("MyResourceName.ico");
            //Image = Bitmap.FromResource ("MyResourceName.png");
            Shortcut = Application.Instance.CommonModifier | Keys.T;
        }

        protected override void OnExecuted(EventArgs e)
        {
            base.OnExecuted(e);
            MessageBox.Show(parent, "Wow, it's a test message box!", "Test Box", MessageBoxType.Information);
            
        }
    }
}
