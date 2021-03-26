using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggle : MonoBehaviour
{
    public Door door;
    public Toggle locker;
    public Toggle closer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void isLocked()
    {
        door.Lock();
    }

    public void isClosed()
    {
        door.Closer();
    }
}
