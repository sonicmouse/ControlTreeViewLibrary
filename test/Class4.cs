using ControlTreeView;

namespace test
{
	public partial class Class4 : NodeControl
	{
		public Class4(string text)
		{
			InitializeComponent();

			this.label1.Text = text;
			this.label1.MouseDown += new MouseEventHandler((sender, e) => { OnMouseDown(e); });
		}
	}
}
