
using UnityEngine;

public abstract class BehaviourTree : MonoBehaviour
{
    Node root = null;

    protected void Start()
    {
        root = SetupTree();
    }

    public void Update()
    {
        if (root != null)
        {
            root.Evaluate();
        }
    }
    protected abstract Node SetupTree();
}