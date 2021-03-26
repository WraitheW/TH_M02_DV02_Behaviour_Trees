using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sith : MonoBehaviour
{
    public Door aDore;
    public GameObject holocron;
    Task currentTask;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Checkpoint 1");
            currentTask = BuildTask_GetHolocron();

            currentTask.run();
        }
    }

    Task BuildTask_GetHolocron()
    {
        Debug.Log("Checkpoint 3");
        List<Task> taskList = new List<Task>();

        //if door isn't locked, open it
        Task isDoorNotLocked = new isFalse(aDore.isLocked);
        Task openDoor = new OpenDoor(aDore);
        taskList.Add(isDoorNotLocked);
        taskList.Add(openDoor);
        Sequence openUnlockedDoor = new Sequence(taskList);

        //barge a closed door
        taskList = new List<Task>();
        Task isDoorClosed = new isTrue(aDore.isClosed);
        Task darkside = new DarkSide(this.gameObject);
        Task bargeDoor = new PushDoor(aDore.GetComponent<Rigidbody>());
        taskList.Add(isDoorClosed);
        taskList.Add(darkside);
        taskList.Add(bargeDoor);
        Sequence bargeClosedDoor = new Sequence(taskList);

        //open a closed door, one way or another
        taskList = new List<Task>();
        taskList.Add(openUnlockedDoor);
        taskList.Add(bargeClosedDoor);
        Selector openTheDoor = new Selector(taskList);

        //get the treasure when the door is closed
        taskList = new List<Task>();
        Task moveToDoor = new MoveKinematicToObject(this.GetComponent<Kinematic>(), aDore.gameObject);
        Task moveToTreasure = new MoveKinematicToObject(this.GetComponent<Kinematic>(), holocron.gameObject);
        taskList.Add(moveToDoor);
        taskList.Add(openTheDoor);
        taskList.Add(moveToTreasure);
        Sequence getTreasureBehindClosedDoor = new Sequence(taskList);

        //get the treasure when the door is open
        taskList = new List<Task>();
        Task isDoorOpen = new isFalse(aDore.isClosed);
        taskList.Add(isDoorOpen);
        taskList.Add(moveToTreasure);
        Sequence getTreasureBehindOpenDoor = new Sequence(taskList);

        //get the treasure, one way or another???
        taskList = new List<Task>();
        taskList.Add(getTreasureBehindOpenDoor);
        taskList.Add(getTreasureBehindClosedDoor);
        Selector getTreasure = new Selector(taskList);

        return getTreasure;
    }
}
