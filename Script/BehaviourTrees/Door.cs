using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isClosed = false;
    public bool isLocked = false;

    Vector3 closedRotation = new Vector3(0, 0, 0);
    Vector3 openRotation = new Vector3(0, -135, 0);

    // Start is called before the first frame update
    void Start()
    {
        if (isClosed)
        {
            transform.eulerAngles = closedRotation;
        }
        else
        {
            transform.eulerAngles = openRotation;
        }
    }

    public bool Open()
    {
        if (isClosed && !isLocked)
        {
            isClosed = false;
            transform.eulerAngles = openRotation;
            return true;
        }

        return false;
    }

    public bool Close()
    {
        if (!isClosed)
        {
            transform.eulerAngles = closedRotation;
            isClosed = true;
        }

        return true;
    }

    public void Lock()
    {
        if (isLocked)
        {
            isLocked = false;
        }
        else
        {
            isLocked = true;
        }
    }

    public void Closer()
    {
        if (isClosed)
        {
            isClosed = false;
        }
        else
        {
            isClosed = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isClosed)
        {
            transform.eulerAngles = closedRotation;
        }
        else
        {
            transform.eulerAngles = openRotation;
        }
    }
}
