using BehaviourTree;
using UnityEngine;

public class SparrowAI : AnimalAI
{
    private Node root = null;

    //public SparrowAI() 
    //{
    //    root = BuildTree(this);
    //}
    private void Start()
    {
        root = BuildTree(this);
#if UNITY_PS4
        Debug.Log("This sparrow is on ps4");
#endif
    }

    private void Update()
    {
        //if a tree has been created, run it
        if (root != null) root.Execute();
    }
}
