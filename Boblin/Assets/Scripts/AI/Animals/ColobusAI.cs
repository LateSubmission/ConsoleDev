using BehaviourTree;

public class ColobusAI : AnimalAI
{
    private Node root = null;

    private void Start()
    {
        root = BuildTree(this);
    }

    private void Update()
    {
        //if a tree has been created, run it
        if (root != null) root.Execute();
    }
}
