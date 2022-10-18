using System.Drawing;

namespace ControlTreeView
{
	/// <summary>
	/// The draw style of CTreeView.
	/// </summary>
	public enum CTreeViewDrawStyle
	{
		LinearTree, HorizontalDiagram, VerticalDiagram
	}

	/// <summary>
	/// The selection mode of CTreeView.
	/// </summary>
	public enum CTreeViewSelectionMode
	{
		Multi, MultiSameParent, Single, None
	}

	/// <summary>
	/// The DragAndDrop mode of CTreeView.
	/// </summary>
	public enum CTreeViewDragAndDropMode
	{
		/*CopyReplaceReorder,*/
		ReplaceReorder, Reorder, Nothing
	}

	/// <summary>
	/// The bitmaps for plus and minus buttons of nodes.
	/// </summary>
	public struct CTreeViewPlusMinus
	{
		public Bitmap Plus { get; }
		public Bitmap Minus { get; }
		internal Size Size { get; }

		public CTreeViewPlusMinus(Bitmap plus, Bitmap minus)
		{
			Size = plus.Size;
			if (Size != minus.Size)
			{
				throw new ArgumentException("Images are of different sizes");
			}
			Plus = plus;
			Minus = minus;
		}
	}
}