using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hangman_00
{
    public partial class MainForm : Form
    {
        public MainForm() =>  InitializeComponent();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            IterateControlTree(null, initCheckBox, null);
		}

        internal delegate bool FxControlDlgt(Control control, Object args);
		internal bool IterateControlTree(Control control, FxControlDlgt fX, Object args)
		{
			if (control == null) control = this;
			if (!fX(control, args)) return false;
			foreach (Control child in control.Controls)
			{
				if (!IterateControlTree(child, fX, args))
				{
					return false;
				}
			}
			return true;
		}
        private bool initCheckBox(Control control, object args)
		{
			if (control is CheckBox checkbox)
			{
				// One way to autogenerate the Text from the Name property.
				string sval = checkbox.Name.Replace("checkBox", string.Empty);
				if(int.TryParse(sval, out int offset))
                {
					char c = (char)('A' + (offset - 1));
					checkbox.Text = c.ToString();
                }
				checkbox.Click += onAnyClick;
			}
			return true;
        }

        private void onAnyClick(object sender, EventArgs e)
		{
			if (sender is CheckBox checkbox)
			{
				Text = $"{checkbox.Text} = {checkbox.Checked}";
			}
		}
    }
}
