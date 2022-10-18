using ControlTreeView;

namespace test
{
	public partial class Class1 : NodeControl
	{
		public Class1(string text)
		{
			InitializeComponent();
			button1.Text = Name = text;
		}

		private void CheckBox1_CheckedChanged(object? sender, EventArgs e)
		{
			if (checkBox1.Checked) BackColor = Color.PaleGreen;
			else BackColor = Color.PaleTurquoise;
		}

		private void Button1_Click(object? sender, EventArgs e)
		{
			MessageBox.Show($"Button_Click from {button1.Text}!");
		}
	}
}
