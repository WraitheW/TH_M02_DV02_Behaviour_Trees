using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Kinematic
{
    FollowPath myMoveType;
    LookWhereGoing myRotateType;

    public GameObject[] path = new GameObject[4];

    void Start()
    {
        myRotateType = new LookWhereGoing();
        myRotateType.character = this;
        myRotateType.target = myTarget;

        myMoveType = new FollowPath();
        myMoveType.character = this;
        myMoveType.path = path;
    }

    protected override void Update()
    {
        steeringUpdate = new SteeringOutput();
        steeringUpdate.angular = myRotateType.getSteering().angular;
        steeringUpdate.linear = myMoveType.getSteering().linear;
        base.Update();
    }
}
