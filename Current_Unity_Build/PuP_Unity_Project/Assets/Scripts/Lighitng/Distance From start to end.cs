using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceFromstarttoend : MonoBehaviour
{

    public Vector3 WorldStart;
    public Vector3 WorldEnd;

    public GameObject Light;

    // Start is called before the first frame update
    void Start()
    {
        WorldStart = GameObject.Find("World Start").transform.position;
        WorldEnd = GameObject.Find("World End").transform.position;

        float difference = WorldEnd.x - WorldStart.x;

        //difference = difference*

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
