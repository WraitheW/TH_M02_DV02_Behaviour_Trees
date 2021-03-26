using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : Kinematic
{
    public Node start;
    public Node goal;
    Graph graph;
    public bool isCheap;

    FollowPath myMoveType;
    LookWhereGoing myRotateType;

    GameObject[] myPath = new GameObject[4];

    void Start()
    {
        myRotateType = new LookWhereGoing();
        myRotateType.character = this;
        myRotateType.target = myTarget;

        Graph graph = new Graph();

        if (isCheap)
        {
            graph.Build(true);
        }
        else
        {
            graph.Build(false);
        }
        
        List<Connection> path = Dijkstra.pathFind(graph, start, goal);
        myPath = new GameObject[path.Count + 1];

        int i = 0;
        foreach (Connection co in path)
        {
            myPath[i] = co.getFromNode().gameObject;
            i++;
        }
        myPath[i] = goal.gameObject;

        myMoveType = new FollowPath();
        myMoveType.character = this;
        myMoveType.path = myPath;
    }

    protected override void Update()
    {
        steeringUpdate = new SteeringOutput();
        steeringUpdate.angular = myRotateType.getSteering().angular;
        steeringUpdate.linear = myMoveType.getSteering().linear;

        base.Update();
    }
}
