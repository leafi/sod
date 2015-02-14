using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eto;
using Eto.Drawing;
using Eto.Forms;
using sod.EditorUI.Commands;

namespace sod.EditorUI
{
    public class MainForm : Form
    {
        public MainForm()
        {
            this.ClientSize = new Eto.Drawing.Size(1024, 640);
            this.Title = "sod " + ApplicationInformation.CompileDate.ToString();

            this.setupContent();
            this.setupMenuBar();
            this.setupToolBar();
            this.setupCloseEvents();
        }

        private void setupContent()
        {
            Content = new TableLayout
            {
                Spacing = new Size(5, 5),
                Padding = new Padding(10, 10, 10, 10),
                Rows =
                {
                    new TableRow(
                        //new TableCell(new Label { Text = "first testest" }, true),
                        new TableCell(new RenderBox(), true)
                    )
                }
            };
        }

        private void setupMenuBar()
        {
            Menu = new MenuBar
            {
                Items =
                {
                    new ButtonMenuItem
                    {
                        Text = "&File",
                        Items =
                        {
                            new TestMsgBoxCommand(this)
                        }
                    }
                },

                QuitItem = new Command((sender, e) => Application.Instance.Quit()) { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q }
            };
        }

        private void setupToolBar()
        {
            /*ToolBar = new ToolBar
            {
                Items =
                {
                    new TestMsgBoxCommand(this),
                    new SeparatorToolItem(),
                    new ButtonToolItem { Text = "bleh" }
                }
            };*/
        }

        private void setupCloseEvents()
        {
            Application.Instance.Terminating += (sender, cancelEv) =>
            {
                // TODO: check for unsaved changes/properly close down/etc!
            };

            // On Mac, quit the application when this window is closed.
            // I have no intention of supporting multiple 'documents' at once, or even ever not having one open.
            if (Platform.Instance.IsMac)
                this.Closed += (sender, e) => Application.Instance.Quit();
        }
    }
}
