using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class curve : MonoBehaviour
{
    public int Route;
    public float SpeedMod = 0.069f;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player")
        {
            other.transform.parent.GetComponent<BezierFollow>().routeToGo = Route;
            other.transform.parent.GetComponent<BezierFollow>().speedMod = SpeedMod;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent.GetComponent<BezierFollow>().speedMod = 0.069f;
        }
    }
}
