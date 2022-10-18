using System.Runtime.InteropServices;

namespace ControlTreeView
{
	public partial class CTreeView
	{
		[DllImport("user32.dll", EntryPoint = "SendMessageW", CharSet = CharSet.Unicode)]
		static extern IntPtr SendMessage(
			   IntPtr hWnd,      // handle to destination window
			   uint Msg,       // message
			   IntPtr wParam,  // first message parameter
			   IntPtr lParam   // second message parameter
			   );

		/// <summary>
		/// 
		/// </summary>
		public struct DragTargetPositionClass
		{
			internal DragTargetPositionClass(CTreeNode? nodeDirect, CTreeNode? nodeBefore, CTreeNode? nodeAfter)
			{
				_nodeDirect = nodeDirect;
				_nodeBefore = nodeBefore;
				_nodeAfter = nodeAfter;
			}

			/// <summary>
			/// Gets a value indicating whether drag destination nodes are not empty.
			/// </summary>
			public bool Enabled
			{
				get { return (_nodeDirect != null || _nodeBefore != null || _nodeAfter != null); }
			}

			private readonly CTreeNode? _nodeDirect;
			/// <summary>
			/// The direct node of drag target position.
			/// </summary>
			public CTreeNode? NodeDirect => _nodeDirect;

			private readonly CTreeNode? _nodeBefore;
			/// <summary>
			/// The upper node of drag target position.
			/// </summary>
			public CTreeNode? NodeBefore => _nodeBefore;

			private readonly CTreeNode? _nodeAfter;
			/// <summary>
			/// The lower node of drag target position.
			/// </summary>
			public CTreeNode? NodeAfter => _nodeAfter;
		}

		private readonly Pen dragDropLinePen;
		private Point dragDropLinePoint1, dragDropLinePoint2;
		private Rectangle dragDropRectangle;
		private readonly System.Windows.Forms.Timer scrollTimer;

		private void ScrollTimer_Tick(object? sender, EventArgs e)
		{
			if (scrollDown) SendMessage(Handle, 277, new IntPtr(1), IntPtr.Zero);
			else if (scrollUp) SendMessage(Handle, 277, IntPtr.Zero, IntPtr.Zero);
			if (scrollRigh) SendMessage(Handle, 276, new IntPtr(1), IntPtr.Zero);
			else if (scrollLeft) SendMessage(Handle, 276, IntPtr.Zero, IntPtr.Zero);
		}

		private bool scrollUp, scrollDown, scrollRigh, scrollLeft;
		/// <summary>
		/// Sets the directions in which need scroll.
		/// </summary>
		/// <param name="scrollUp">true if need scroll up, otherwise, false.</param>
		/// <param name="scrollDown">true if need scroll down, otherwise, false.</param>
		/// <param name="scrollRigh">true if need scroll right, otherwise, false.</param>
		/// <param name="scrollLeft">true if need scroll left, otherwise, false.</param>
		internal void SetScrollDirections(bool scrollUp, bool scrollDown, bool scrollRigh, bool scrollLeft)
		{
			if (scrollUp || scrollDown || scrollRigh || scrollLeft) scrollTimer.Enabled = true;
			else scrollTimer.Enabled = false;
			this.scrollUp = scrollUp;
			this.scrollDown = scrollDown;
			this.scrollRigh = scrollRigh;
			this.scrollLeft = scrollLeft;
		}

		private void UpdateDragTargetPosition()
		{
			if (DragTargetPosition.NodeDirect != null || DragTargetPosition.NodeBefore != null || DragTargetPosition.NodeAfter != null)
			{
				DragTargetPosition = new DragTargetPositionClass(null, null, null);
				dragDropLinePoint1 = Point.Empty;
				dragDropLinePoint2 = Point.Empty;
				dragDropRectangle = Rectangle.Empty;
				Refresh();
			}
		}

		private void UpdateDragTargetPosition(CTreeNode node)
		{
			if (DragTargetPosition.NodeDirect != node)
			{
				DragTargetPosition = new DragTargetPositionClass(node, null, null);
				dragDropRectangle = node.Bounds;
				dragDropRectangle.Inflate(2, 2);
				dragDropLinePoint1 = Point.Empty;
				dragDropLinePoint2 = Point.Empty;
				Refresh();
			}
		}

		private void UpdateDragTargetPosition(CTreeNode? nodeBefore, CTreeNode? nodeAfter)
		{
			if (DragTargetPosition.NodeBefore != nodeBefore || DragTargetPosition.NodeAfter != nodeAfter)
			{
				DragTargetPosition = new DragTargetPositionClass(null, nodeBefore, nodeAfter);
				if (nodeBefore == null)
				{
					if (DrawStyle == CTreeViewDrawStyle.VerticalDiagram)
					{
						dragDropLinePoint1 = new Point(nodeAfter?.BoundsSubtree.X - 2 ?? 0, nodeAfter?.BoundsSubtree.Y ?? 0);
						dragDropLinePoint2 = new Point(nodeAfter?.BoundsSubtree.X - 2 ?? 0, nodeAfter?.BoundsSubtree.Bottom ?? 0);
					}
					else
					{
						dragDropLinePoint1 = new Point(nodeAfter?.BoundsSubtree.X ?? 0, nodeAfter?.BoundsSubtree.Y - 2 ?? 0);
						dragDropLinePoint2 = new Point(nodeAfter?.BoundsSubtree.Right ?? 0, nodeAfter?.BoundsSubtree.Y - 2 ?? 0);
					}
				}
				else if (nodeAfter == null)
				{
					if (DrawStyle == CTreeViewDrawStyle.VerticalDiagram)
					{
						dragDropLinePoint1 = new Point(nodeBefore.BoundsSubtree.Right + 2, nodeBefore.BoundsSubtree.Y);
						dragDropLinePoint2 = new Point(nodeBefore.BoundsSubtree.Right + 2, nodeBefore.BoundsSubtree.Bottom);
					}
					else
					{
						dragDropLinePoint1 = new Point(nodeBefore.BoundsSubtree.X, nodeBefore.BoundsSubtree.Bottom + 2);
						dragDropLinePoint2 = new Point(nodeBefore.BoundsSubtree.Right, nodeBefore.BoundsSubtree.Bottom + 2);
					}
				}
				else
				{
					if (DrawStyle == CTreeViewDrawStyle.VerticalDiagram)
					{
						int y1 = nodeBefore.BoundsSubtree.Y;
						int y2 = Math.Max(nodeBefore.BoundsSubtree.Bottom, nodeAfter.BoundsSubtree.Bottom);
						int x = nodeBefore.BoundsSubtree.Right + IndentWidth / 2;
						dragDropLinePoint1 = new Point(x, y1);
						dragDropLinePoint2 = new Point(x, y2);
					}
					else
					{
						int x1 = nodeBefore.BoundsSubtree.X;
						int x2 = Math.Max(nodeBefore.BoundsSubtree.Right, nodeAfter.BoundsSubtree.Right);
						int y = nodeBefore.BoundsSubtree.Bottom + IndentWidth / 2;
						dragDropLinePoint1 = new Point(x1, y);
						dragDropLinePoint2 = new Point(x2, y);
					}
				}
				dragDropRectangle = Rectangle.Empty;
				Refresh();
			}
		}

		internal void ResetDragTargetPosition()
		{
			scrollTimer.Enabled = false;

			UpdateDragTargetPosition();
		}

		/// <summary>
		/// Sets the drag destination nodes according to specified cursor position.
		/// </summary>
		/// <param name="dragPosition">The position of mouse cursor during drag.</param>
		internal void SetDragTargetPosition(Point dragPosition)
		{
			CTreeNode? destinationNode = null;
			CTreeNodeCollection destinationCollection = Nodes;
			Nodes.TraverseNodes(node => node.Visible && node.BoundsSubtree.Contains(dragPosition), node =>
			{
				destinationNode = node;
				destinationCollection = node.Nodes;
			});
			if (destinationNode != null && destinationNode.Bounds.Contains(dragPosition)) //Drag position within node
			{
				//Find drag position within node
				int delta, coordinate, firstBound, secondBound;
				if (DrawStyle == CTreeViewDrawStyle.VerticalDiagram)
				{
					delta = destinationNode.Bounds.Width / 4;
					coordinate = dragPosition.X;
					firstBound = destinationNode.Bounds.Left;
					secondBound = destinationNode.Bounds.Right;
				}
				else
				{
					delta = destinationNode.Bounds.Height / 4;
					coordinate = dragPosition.Y;
					firstBound = destinationNode.Bounds.Top;
					secondBound = destinationNode.Bounds.Bottom;
				}
				if (coordinate >= firstBound + delta && coordinate <= secondBound - delta)
				{
					UpdateDragTargetPosition(destinationNode);
					return;
				}
				else if (coordinate < firstBound + delta) //before
				{
					UpdateDragTargetPosition(destinationNode.PrevNode, destinationNode);
					return;
				}
				else if (coordinate > secondBound - delta) //after
				{
					UpdateDragTargetPosition(destinationNode, destinationNode.NextNode);
					return;
				}
			}
			else //Drag position out of the nodes
			{
				//Check drag position between two nodes
				CTreeNode? upperNode = null, lowerNode = null;
				bool isBetween = false;
				for (int count = 0; count <= destinationCollection.Count - 2; count++)
				{
					upperNode = destinationCollection[count];
					lowerNode = destinationCollection[count + 1];
					Point betweenLocation = Point.Empty;
					Size betweenSize = Size.Empty;
					if (DrawStyle == CTreeViewDrawStyle.VerticalDiagram)
					{
						betweenLocation = new Point(upperNode.BoundsSubtree.Right, upperNode.BoundsSubtree.Top);
						betweenSize = new Size(lowerNode.BoundsSubtree.Left - upperNode.BoundsSubtree.Right, Math.Max(upperNode.BoundsSubtree.Height, lowerNode.BoundsSubtree.Height));
					}
					else
					{
						betweenLocation = new Point(upperNode.BoundsSubtree.Left, upperNode.BoundsSubtree.Bottom);
						betweenSize = new Size(Math.Max(upperNode.BoundsSubtree.Width, lowerNode.BoundsSubtree.Width), lowerNode.BoundsSubtree.Top - upperNode.BoundsSubtree.Bottom);
					}
					var betweenRectangle = new Rectangle(betweenLocation, betweenSize);
					if (betweenRectangle.Contains(dragPosition))
					{
						isBetween = true;
						break;
					}
				}
				if (isBetween) //Drag position between two nodes
				{
					UpdateDragTargetPosition(upperNode, lowerNode);
					return;
				}
				else if (destinationNode != null)
				{
					Rectangle ownerBounds = destinationNode.Bounds;
					bool isAbove, isBelow;
					if (DrawStyle == CTreeViewDrawStyle.VerticalDiagram)
					{
						isAbove = (dragPosition.X <= ownerBounds.Left);
						isBelow = (dragPosition.X >= ownerBounds.Right);
					}
					else
					{
						isAbove = (dragPosition.Y <= ownerBounds.Top);
						isBelow = (dragPosition.Y >= ownerBounds.Bottom);
					}
					if (isAbove) //before
					{
						UpdateDragTargetPosition(destinationNode.PrevNode, destinationNode);
						return;
					}
					else if (isBelow) //after
					{
						UpdateDragTargetPosition(destinationNode, destinationNode.NextNode);
						return;
					}
				}
			}
			UpdateDragTargetPosition();
		}

		/// <summary>
		/// Checking a valid of drop operation in current destination.
		/// </summary>
		/// <param name="sourceNodes">The source nodes of drag and drop operation.</param>
		/// <returns>true if drop of source nodes is allowed to current destination, otherwise, false.</returns>
		internal bool CheckValidDrop(List<CTreeNode> sourceNodes)
		{
			if (!DragTargetPosition.Enabled) return false;
			bool isValid = true;
			if (DragTargetPosition.NodeDirect != null)
			{
				if (DragAndDropMode == CTreeViewDragAndDropMode.Reorder) return false;
				else
				{
					//Check that destination node is not descendant of source nodes
					foreach (CTreeNode sourceNode in sourceNodes)
					{
						sourceNode.TraverseNodes(node => isValid, node =>
						{
							if (node == DragTargetPosition.NodeDirect) isValid = false;
						});
						if (!isValid) return false;
					}
				}
			}
			else if (DragTargetPosition.NodeBefore != null || DragTargetPosition.NodeAfter != null)
			{
				//Check that source nodes are not moved relative themselves
				if(DragTargetPosition.NodeBefore is not null && DragTargetPosition.NodeAfter is not null)
				{
					if (sourceNodes.Contains(DragTargetPosition.NodeBefore) && sourceNodes.Contains(DragTargetPosition.NodeAfter)) return false;
				}

				if(DragTargetPosition.NodeBefore is not null)
				{
					if (sourceNodes.Contains(DragTargetPosition.NodeBefore) && DragTargetPosition.NodeAfter == null) return false;
				}
				
				if(DragTargetPosition.NodeAfter is not null)
				{
					if (sourceNodes.Contains(DragTargetPosition.NodeAfter) && DragTargetPosition.NodeBefore == null) return false;
				}
				
				if (DragAndDropMode == CTreeViewDragAndDropMode.Reorder)
				{
					//Check that source and destination nodes have same parent
					if (DragTargetPosition.NodeBefore != null && DragTargetPosition.NodeBefore.Parent != sourceNodes[0].Parent) return false;
					if (DragTargetPosition.NodeAfter != null && DragTargetPosition.NodeAfter.Parent != sourceNodes[0].Parent) return false;
				}
				else
				{
					//Check that destination nodes is not descendants of source nodes
					foreach (CTreeNode sourceNode in sourceNodes)
					{
						sourceNode.Nodes.TraverseNodes(node => isValid, node =>
						{
							if (DragTargetPosition.NodeBefore == node || DragTargetPosition.NodeAfter == node) isValid = false;
						});
						if (!isValid) return false;
					}
				}
			}
			return true;
		}
	}
}
