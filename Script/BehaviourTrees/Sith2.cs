using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sith2 : MonoBehaviour
{
    public Door aDore;
    public GameObject holocron;
    Task currentTask;
    bool executingBehaviour = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!executingBehaviour)
            {
                executingBehaviour = true;
                currentTask = BuildTask_GetHolocron();

                EventBus.StartListening(currentTask.TaskFinished, OnTaskFinished);
                currentTask.run();
            }
        }
    }

    void OnTaskFinished()
    {
        EventBus.StopListening(currentTask.TaskFinished, OnTaskFinished);
        executingBehaviour = false;
    }

    Task BuildTask_GetHolocron()
    {
        List<Task> taskList = new List<Task>();

        Task isDoorNotLocked = new isFalse(aDore.isLocked);
        Task waitABeat = new Wait(0.5f);
        Task openDoor = new OpenDoor(aDore);
        taskList.Add(isDoorNotLocked);
        taskList.Add(waitABeat);
        taskList.Add(openDoor);
        Sequence openUnlockedDoor = new Sequence(taskList);

        taskList = new List<Task>();
        Task isDoorClosed = new isTrue(aDore.isClosed);
        Task darkside = new DarkSide(this.gameObject);
        Task bargeDoor = new PushDoor(aDore.GetComponent<Rigidbody>());
        Task darksidetwo = new DarkSideTwo(this.gameObject);
        taskList.Add(isDoorClosed);
        taskList.Add(waitABeat);
        taskList.Add(darkside);
        taskList.Add(waitABeat);
        taskList.Add(darksidetwo);
        taskList.Add(waitABeat);
        taskList.Add(bargeDoor);
        Sequence bargeClosedDoor = new Sequence(taskList);

        taskList = new List<Task>();
        taskList.Add(openUnlockedDoor);
        taskList.Add(bargeClosedDoor);
        Selector openTheDoor = new Selector(taskList);

        taskList = new List<Task>();
        Task moveToDoor = new MoveKinematicToObject(this.GetComponent<Kinematic>(), aDore.gameObject);
        Task moveToTreasure = new MoveKinematicToObject(this.GetComponent<Kinematic>(), holocron.gameObject);
        taskList.Add(moveToDoor);
        taskList.Add(waitABeat);
        taskList.Add(openTheDoor);
        taskList.Add(waitABeat);
        taskList.Add(moveToTreasure);
        Sequence getTreasureBehindClosedDoor = new Sequence(taskList);

        taskList = new List<Task>();
        Task isDoorOpen = new isFalse(aDore.isClosed);
        taskList.Add(isDoorOpen);
        taskList.Add(moveToTreasure);
        Sequence getTreasureBehindOpenDoor = new Sequence(taskList);

        taskList = new List<Task>();
        taskList.Add(getTreasureBehindOpenDoor);
        taskList.Add(getTreasureBehindClosedDoor);
        Selector getTreasure = new Selector(taskList);

        return getTreasure;
    }
}
