
using System.Collections.Generic;

public enum NodeState
{
    Running,
    Success,
    Failure
}

public abstract class Node
{
    protected NodeState state;

    public Node parent;
    protected List<Node> children = new List<Node>();
    public NodeState _state { get { return state; } }

    public Node()
    {
        parent = null;
    }
    public Node(List<Node> children)
    {
        foreach (Node child in children)
        {
            Attach(child);
        }
    }
    private void Attach(Node node)
    {
        node.parent = this;
        children.Add(node);
    }
    public virtual NodeState Evaluate() => NodeState.Failure;
}