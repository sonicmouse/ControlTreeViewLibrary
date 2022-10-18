namespace ControlTreeView
{
    /// <summary>
    /// Provides selection logic and DragAndDrop logic for the CTreeNode.Control.
    /// </summary>
    public interface INodeControl
    {
        /// <summary>
        /// The owner node of this control.
        /// </summary>
        CTreeNode OwnerNode { set; }
    }
}