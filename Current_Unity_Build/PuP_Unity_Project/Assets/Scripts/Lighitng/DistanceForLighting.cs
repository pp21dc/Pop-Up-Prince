using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceForLighting : MonoBehaviour
{

    public Vector3 WorldStart;
    public Vector3 WorldEnd;
    public Vector3 PlayerLocation;

    public float currentPositionPercent;

    public GameObject Light;

    // Start is called before the first frame update
    void Start()
    {
        WorldStart = GameObject.Find("World Start").transform.position;
        WorldEnd = GameObject.Find("World End").transform.position;
        PlayerLocation = GameObject.Find("Player").transform.position;

        currentPositionPercent = (PlayerLocation.x - WorldStart.x) - (WorldEnd.x - WorldStart.x);

        //difference = difference*

    }

    // Update is called once per frame
    void Update()
    {
        currentPositionPercent = playerPercentage();
    }

    float playerPercentage()
    {
        return playerLocationFromStart() / endDistanceFromStart();
    }

    float playerLocationFromStart() //Gives player distance from start point (imaginary 0)
    {
        return PlayerLocation.x - WorldStart.x;
    }

    float endDistanceFromStart() //Gives worldEnd distance from start
    {
        return WorldEnd.x - WorldStart.x;
    }

}
