using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task
{
    public bool succeeded;
    public abstract void run();

    protected int eventID;
    const string EVENT_NAME_PREFIX = "FinishedTask";
    public string TaskFinished
    {
        get
        {
            return EVENT_NAME_PREFIX + eventID;
        }
    }
    public Task()
    {
        eventID = EventBus.GetEventID();
    }
}

public class isTrue : Task
{
    bool varToTest;

    public isTrue(bool someBool)
    {
        varToTest = someBool;
    }

    public override void run()
    {
        succeeded = varToTest;
        EventBus.TriggerEvent(TaskFinished);
    }
}

public class isFalse : Task
{
    bool varToTest;

    public isFalse(bool someBool)
    {
        varToTest = someBool;
    }

    public override void run()
    {
        succeeded = !varToTest;
        EventBus.TriggerEvent(TaskFinished);
    }
}

public class OpenDoor : Task
{
    Door door;

    public OpenDoor(Door someDoor)
    {
        door = someDoor;
    }

    public override void run()
    {
        succeeded = door.Open();
        EventBus.TriggerEvent(TaskFinished);
    }
}

public class PushDoor : Task
{
    Rigidbody rb;

    public PushDoor(Rigidbody someDoor)
    {
        rb = someDoor;
    }

    public override void run()
    {
        rb.AddForce(-10f, 4f, 0f, ForceMode.VelocityChange);
        succeeded = true;
        EventBus.TriggerEvent(TaskFinished);
    }
}

public class DarkSide : Task
{
    GameObject gO;

    public DarkSide(GameObject someGameObject)
    {
        gO = someGameObject;
    }

    public override void run()
    {
        gO.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        succeeded = true;
        EventBus.TriggerEvent(TaskFinished);
    }
}

public class DarkSideTwo : Task
{
    GameObject gO;

    public DarkSideTwo(GameObject someGameObject)
    {
        gO = someGameObject;
    }

    public override void run()
    {
        gO.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        succeeded = true;
        EventBus.TriggerEvent(TaskFinished);
    }
}

public class Wait : Task
{
    float mTimeToWait;

    public Wait(float time)
    {
        mTimeToWait = time;
    }

    public override void run()
    {
        succeeded = true;
        EventBus.ScheduleTrigger(TaskFinished, mTimeToWait);
    }
}

public class MoveKinematicToObject : Task
{
    Arriver myMoveType;
    GameObject gO;

    public MoveKinematicToObject(Kinematic mover, GameObject target)
    {
        myMoveType = mover as Arriver;
        gO = target;
    }

    public override void run()
    {
        myMoveType.OnArrived += MoverArrived;
        myMoveType.myTarget = gO;
    }

    public void MoverArrived()
    {
        myMoveType.OnArrived -= MoverArrived;
        succeeded = true;
        EventBus.TriggerEvent(TaskFinished);
    }
}

public class Sequence : Task
{
    List<Task> children;
    Task currentTask;
    int taskIndex = 0;

    public Sequence(List<Task> taskList)
    {
        children = taskList;
    }

    public override void run()
    {
        currentTask = children[taskIndex];
        EventBus.StartListening(currentTask.TaskFinished, OnChildTaskFinished);
        currentTask.run();
    }

    void OnChildTaskFinished()
    {
        if (currentTask.succeeded)
        {
            EventBus.StopListening(currentTask.TaskFinished, OnChildTaskFinished);
            taskIndex++;
            if (taskIndex < children.Count)
            {
                this.run();
            }
            else
            {
                succeeded = true;
                EventBus.TriggerEvent(TaskFinished);
            }
        }
        else
        {
            succeeded = false;
            EventBus.TriggerEvent(TaskFinished);
        }
    }
}

public class Selector : Task
{
    List<Task> children;
    Task currentTask;
    int taskIndex = 0;

    public Selector(List<Task> taskList)
    {
        children = taskList;
    }

    public override void run()
    {
        currentTask = children[taskIndex];
        EventBus.StartListening(currentTask.TaskFinished, OnChildTaskFinished);
        currentTask.run();
    }
    void OnChildTaskFinished()
    {
        if (currentTask.succeeded)
        {
            succeeded = true;
            EventBus.TriggerEvent(TaskFinished);
        }
        else
        {
            EventBus.StopListening(currentTask.TaskFinished, OnChildTaskFinished);
            taskIndex++;
            if (taskIndex < children.Count)
            {
                this.run();
            }
            else
            {
                succeeded = false;
                EventBus.TriggerEvent(TaskFinished);
            }
        }
    }
}