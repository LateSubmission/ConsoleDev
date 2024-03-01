using UnityEngine;
using BehaviourTree;
using System.Collections.Generic;

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
    }

    private void Update()
    {
        //if a tree has been created, run it
        if (root != null) root.Execute();
    }

}
