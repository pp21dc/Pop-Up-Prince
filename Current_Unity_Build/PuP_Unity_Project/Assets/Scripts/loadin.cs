using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadin : MonoBehaviour
{
    [HideInInspector]
    public float rotation;
    public float speed = 0.5f;
    public PrinceMovement PM;
    public float top = 2.2f;
    public float bottom = 4f;
    bool lk = false;
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > PM.transform.position.y - bottom && !lk)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, PM.transform.position.y - bottom, transform.position.z), speed * Time.deltaTime);

        }
        else
        {
            lk = true;
        }
        rotation = transform.rotation.x;
        
    }

    public bool exit()
    {
        //Debug.Log(rotation);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, PM.transform.position.y + top, transform.position.z), speed * Time.deltaTime);
        if (transform.position.y >= PM.transform.position.y + (top - 0.05f))
        {
            return true;
        }
        return false;
    }
}
