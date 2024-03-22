using BehaviourTree;
using UnityEngine;

public class SparrowAI : AnimalAI
{
    // the root node of the sparrow's behaviour tree
    private Node root = null;

    //public SparrowAI() 
    //{
    //    root = BuildTree(this);
    //}
    private void Start()
    {
        BaseInit();

        CheckPlayerPrefs(this, "sparrow", maxHealth);

        root = BuildTree(this);


#if UNITY_PS4
        Debug.Log("This sparrow is on ps4");
#endif

        animalName = "Sparrow";


    }

    private void Update()
    {
        //if a tree has been created, run it
        if (root != null) root.Execute();
    }
}
