using ControlTreeView;
using System.ComponentModel;

namespace test
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void RadioButtons_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton1.Checked) cTreeView1.DrawStyle = CTreeViewDrawStyle.LinearTree;
			else if (radioButton2.Checked) cTreeView1.DrawStyle = CTreeViewDrawStyle.HorizontalDiagram;
			else if (radioButton3.Checked) cTreeView1.DrawStyle = CTreeViewDrawStyle.VerticalDiagram;
		}

		private void RadioButton6_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton6.Checked) cTreeView1.ShowLines = true;
			else if (radioButton7.Checked) cTreeView1.ShowLines = false;
		}

		private void RadioButton9_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton9.Checked) cTreeView1.ShowRootLines = true;
			else if (radioButton8.Checked) cTreeView1.ShowRootLines = false;
		}

		private void RadioButton11_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton11.Checked) cTreeView1.ShowPlusMinus = true;
			else if (radioButton10.Checked) cTreeView1.ShowPlusMinus = false;
		}

		private void RadioButton13_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton13.Checked) cTreeView1.MinimizeCollapsed = true;
			else if (radioButton12.Checked) cTreeView1.MinimizeCollapsed = false;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			cTreeView2.BeginUpdate();
			cTreeView2.Nodes.Add(new CTreeNode(new Class4("root")));
			for (int i = 0; i < 2; i++)
			{
				cTreeView2.Nodes[0].Nodes.Add(new CTreeNode(new Class4("child " + i)));
				for (int j = 0; j < 3; j++)
				{
					cTreeView2.Nodes[0].Nodes[i].Nodes.Add(new CTreeNode(new Class4("child " + i + "-" + j)));
					for (int k = 0; k < 3; k++)
					{
						cTreeView2.Nodes[0].Nodes[i].Nodes[j].Nodes.Add(new CTreeNode(new Class4("child " + i + "-" + j + "-" + k)));
					}
				}
			}
			cTreeView2.EndUpdate();

			cTreeView3.BeginUpdate();
			cTreeView3.Nodes.Add(new CTreeNode("", new Class1("Button 1")));
			cTreeView3.Nodes.Add(new CTreeNode("", new Class3()));
			cTreeView3.Nodes[1].Nodes.Add(new CTreeNode("", new Class1("Button 2")));
			var n1 = new CTreeNode("", new Class1("Button 3"));
			n1.Nodes.Add(new CTreeNode("", new Class1("Button 4")));
			n1.Nodes.Add(new CTreeNode("", new Class2("")));
			cTreeView3.Nodes[0].Nodes.Add(n1);
			cTreeView3.Nodes[0].Nodes.Add(new CTreeNode("", new Class2("")));
			cTreeView3.Nodes[0].Nodes[1].Nodes.Add(new CTreeNode("", new Class1("Button 5")));
			cTreeView3.Nodes[0].Nodes[1].Nodes.Add(new CTreeNode("", new Class1("Button 6")));
			cTreeView3.Nodes[0].Nodes[1].Nodes.Add(new CTreeNode("", new Class1("Button 7")));
			cTreeView3.EndUpdate();

			cTreeView1.BeginUpdate();
			cTreeView1.Nodes.Add(new CTreeNode(new Button()));
			((Button)cTreeView1.Nodes[0].Control).Text = "button";
			((Button)cTreeView1.Nodes[0].Control).MouseClick += new MouseEventHandler((sender1, e1) => { MessageBox.Show("MouseClick!"); });
			cTreeView1.Nodes.Add(new CTreeNode(new CheckBox()));
			((CheckBox)cTreeView1.Nodes[1].Control).Checked = true;
			((CheckBox)cTreeView1.Nodes[1].Control).Width = 15;
			cTreeView1.Nodes[0].Nodes.Add(new CTreeNode(new ComboBox()));
			((ComboBox)cTreeView1.Nodes[0].Nodes[0].Control).Items.AddRange(new string[] { "1", "2", "3" });
			cTreeView1.Nodes[0].Nodes.Add(new CTreeNode(new RadioButton()));
			((RadioButton)cTreeView1.Nodes[0].Nodes[1].Control).Width = 15;
			((RadioButton)cTreeView1.Nodes[0].Nodes[1].Control).Checked = true;
			cTreeView1.Nodes[0].Nodes[1].Nodes.Add(new CTreeNode(new ProgressBar()));
			((ProgressBar)cTreeView1.Nodes[0].Nodes[1].Nodes[0].Control).Value = 20;
			cTreeView1.Nodes[1].Nodes.Add(new CTreeNode(new TextBox()));
			((TextBox)cTreeView1.Nodes[1].Nodes[0].Control).Text = "text box";
			cTreeView1.Nodes[1].Nodes.Add(new CTreeNode(new ListBox()));
			((ListBox)cTreeView1.Nodes[1].Nodes[1].Control).Items.AddRange(new string[] { "1", "2", "3", "4", "5", "6", "7", "8" });
			cTreeView1.Nodes[1].Nodes[1].Nodes.Add(new CTreeNode(new DataGridView()));
			((DataGridView)cTreeView1.Nodes[1].Nodes[1].Nodes[0].Control).Columns.Add("1", "1");
			((DataGridView)cTreeView1.Nodes[1].Nodes[1].Nodes[0].Control).Columns.Add("2", "2");
			((DataGridView)cTreeView1.Nodes[1].Nodes[1].Nodes[0].Control).Rows.Add(new string[] { "a", "b" });
			cTreeView1.EndUpdate();

			cTreeView4.BeginUpdate();

			cTreeView4.Nodes.Add(new CTreeNode(new Label { Text = "one", AutoSize = true }));

			cTreeView4.Nodes[0].Nodes.Add(new CTreeNode(new ListView
			{
				HeaderStyle = ColumnHeaderStyle.None,
				GridLines = true,
				View = View.Details,
				FullRowSelect = true,
				Columns = { new ColumnHeader { }, new ColumnHeader { } },
				Items = { new ListViewItem { Text = "hello", SubItems = { new ListViewItem.ListViewSubItem { Text = "subitem" } } }, new ListViewItem { Text = "hello 2" } },
				Width = 400
			}));

			cTreeView4.Nodes.Add(new CTreeNode(new Label { Text = "two", AutoSize = true }));
			cTreeView4.Nodes[1].Nodes.Add(new CTreeNode(new ListView
			{
				HeaderStyle = ColumnHeaderStyle.None,
				GridLines = true,
				View = View.Details,
				FullRowSelect = true,
				Columns = { new ColumnHeader { }, new ColumnHeader { } },
				Items = { new ListViewItem { Text = "hello", SubItems = { new ListViewItem.ListViewSubItem { Text = "subitem" } } }, new ListViewItem { Text = "hello 2" } },
				Width = 400
			}));

			cTreeView4.EndUpdate();

			tabControl1.SelectedIndex = 3;
		}

		private void CTreeView3_AfterSelect(object sender, CTreeViewEventArgs e)
		{
			propertyGrid1.SelectedObjects = cTreeView3.SelectedNodes.ToArray<CTreeNode>();
		}

		private void CTreeView3_MouseDown(object sender, MouseEventArgs e)
		{
			if (cTreeView3.SelectedNodes.Count == 0) propertyGrid1.SelectedObject = cTreeView3;
		}

		private void CheckBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox1.Checked) propertyGrid1.BrowsableAttributes = new AttributeCollection();
			else propertyGrid1.BrowsableAttributes = new AttributeCollection(BrowsableAttribute.Yes);
		}

		private void Button1_Click(object sender, EventArgs e)
		{
			NodeControl? c = null;
			if (radioButton4.Checked) c = new Class1("new node");
			if (radioButton5.Checked) c = new Class2("new node");
			if (radioButton14.Checked) c = new Class3();
			if(c is null) { return; }
			if (cTreeView3.SelectedNodes.Count == 0) cTreeView3.Nodes.Add(new CTreeNode(c));
			else if (cTreeView3.SelectedNodes.Count == 1) cTreeView3.SelectedNodes[0].Nodes.Add(new CTreeNode(c));
		}

		private void Button2_Click(object sender, EventArgs e)
		{
			int count = cTreeView3.SelectedNodes.Count;
			while (count > 0)
			{
				if (cTreeView3.SelectedNodes[0].Parent != null) cTreeView3.SelectedNodes[0].Parent?.Nodes.Remove(cTreeView3.SelectedNodes[0]);
				else cTreeView3.Nodes.Remove(cTreeView3.SelectedNodes[0]);
				count--;
			}
		}
	}
}
