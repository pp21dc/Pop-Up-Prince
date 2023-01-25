using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{
    public PrinceMovement Prince;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (transform.position.x > other.transform.position.x)
            {
                Prince.WallCollisionDetected(false, true);
            } else if (transform.position.x < other.transform.position.x)
            {
                Prince.WallCollisionDetected(true, false);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log(other.transform.position + " | " + transform.position);
        if (other.gameObject.tag == "Player")
        {
            if (transform.position.x > other.transform.position.x)
            {
                Prince.WallCollisionDetected(Prince.isWallLeft, false);
            }
            else if (transform.position.x < other.transform.position.x)
            {
                Prince.WallCollisionDetected(false, Prince.isWallRight);
            }

        }
    }

}
