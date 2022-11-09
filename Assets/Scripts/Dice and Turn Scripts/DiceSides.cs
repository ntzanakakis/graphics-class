using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSides : MonoBehaviour
{
    bool onGround;
    public int sideValue;

    void OnTriggerStay(Collider col)
    {
       if (col.tag == "Plane")
        {
            onGround = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Plane")
        {
            onGround = false;
        }
    }

    public bool OnGround()
    {
        return onGround;
    }
}
