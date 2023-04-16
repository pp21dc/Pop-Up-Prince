using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadin : MonoBehaviour
{
    [HideInInspector]
    public float rotation;
    public float speed = 0.5f;
    bool lk = false;
    // Update is called once per frame
    void Update()
    {
        if (transform.rotation.x < 0.9999 && !lk)
        {
            transform.Rotate(speed, 0, 0);

        }
        else
        {
            lk = true;
        }
        rotation = transform.rotation.x;
        
    }

    public void exit()
    {
        Debug.Log(rotation);
        transform.Rotate(-speed, 0, 0);
    }
}
