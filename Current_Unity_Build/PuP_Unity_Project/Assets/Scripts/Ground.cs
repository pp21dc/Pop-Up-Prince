using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{

    public float friction = 32f;

    [HideInInspector]
    public bool top;
    [HideInInspector]
    public bool left;
    [HideInInspector]
    public bool right;
    [HideInInspector]
    public bool roof;
    [HideInInspector]
    public Transform trans;
    [HideInInspector]
    public float slope;
    [HideInInspector]
    public Vector3 leftSide;
    [HideInInspector]
    public Vector3 rightSide;

    private void Start()
    {
        trans = gameObject.transform;
        Quaternion rot = gameObject.transform.rotation;
        float tanResult = 0;
        leftSide = transform.position;
        rightSide = transform.position;
        transform.SetPositionAndRotation(transform.position, new Quaternion());
        leftSide -= new Vector3(Mathf.Abs(transform.localScale.x / 2), 0, 0);
        rightSide += new Vector3(Mathf.Abs(transform.localScale.x / 2), 0, 0);
        transform.SetPositionAndRotation(transform.position, rot);




        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 90)
        {
            float rad = transform.eulerAngles.z * Mathf.PI / 180;
            tanResult = Mathf.Tan(rad);
            //Debug.Log(tanResult);
        }
        else if (transform.eulerAngles.z < 0 || transform.eulerAngles.z > 90)
        {
            float rad = (360f - Mathf.Abs(transform.eulerAngles.z)) * Mathf.PI / 180;
            tanResult = Mathf.Tan(rad) / -1;

        }
        slope = tanResult;
    }
}
