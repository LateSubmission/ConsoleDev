using UnityEngine;
using BehaviourTree;
using System.Collections.Generic;

public class SparrowAI : AnimalAI
{
    public SparrowAI() { }

    protected override Node BuildTree()
    {
        // behaviour tree structure
        Node root = new Sequencer(new List<Node>
        {
            new Selector(new List<Node>
            {
                new Sequencer(new List<Node>
                {
                    // IsTamed();
                    new Selector(new List<Node>
                    {
                        new Sequencer(/*new List<Node> { CheckIsStay(), Stay() }*/)
                        // FollowPlayer();
                    })
                }),
                new Selector(new List<Node>
                {
                    new Sequencer(new List<Node>
                    {
                        // IsPlayerNear(),
                        new Selector(new List<Node>
                        {
                            new Sequencer(new List<Node>
                            {
                                // CheckIsThreatened(),
                                // FightOrFlight()
                            }),
                            // WaitForPlayer()
                        })
                    }),
                    // random selector
                    new Selector(/*true, new List<Node> { Wander(), Rest(), Search() }*/)
                })
            }),
            new Sequencer(new List<Node>
            {
                // CheckHasFood(),
                // EatFood(),
                // CheckFoodEffect(),
                // ReactToFood()
            }),
            
        });

        return root;
    }
}
