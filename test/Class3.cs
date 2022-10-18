using ControlTreeView;

namespace test
{
	public partial class Class3 : NodeControl
	{
		public Class3()
		{
			InitializeComponent();
		}

		private void RadioButton1_CheckedChanged(object? sender, EventArgs e)
		{
			if (radioButton1.Checked) BackColor = Color.LightSteelBlue;
		}

		private void RadioButton4_CheckedChanged(object? sender, EventArgs e)
		{
			if (radioButton4.Checked) BackColor = Color.LightGoldenrodYellow;
		}

		private void RadioButton3_CheckedChanged(object? sender, EventArgs e)
		{
			if (radioButton3.Checked) BackColor = Color.LightPink;
		}

		private void RadioButton2_CheckedChanged(object? sender, EventArgs e)
		{
			if (radioButton2.Checked) BackColor = Color.LightCoral;
		}
	}
}
