using UnityEngine;

namespace BehaviourTree
{
    public abstract class Tree : MonoBehaviour
    {
        Node root = null;

        // Start is called before the first frame update
        void Start()
        {
            root = BuildTree();
        }

        // Update is called once per frame
        private void Update()
        {
            // if a tree has been created, run it
            if (root != null) root.Execute();
        }

        protected abstract Node BuildTree();
    }
}
