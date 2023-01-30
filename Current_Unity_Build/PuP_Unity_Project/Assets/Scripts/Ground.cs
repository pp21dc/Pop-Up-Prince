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

    private void Start()
    {
        trans = gameObject.transform;
    }
}
