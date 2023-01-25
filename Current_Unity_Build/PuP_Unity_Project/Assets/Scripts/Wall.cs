using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public PrinceMovement PM;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (transform.position.x > other.transform.position.x)
            {
                PM.WallCollisionDetected(PM.isWallLeft, true);
            } 
            else 
            {
                PM.WallCollisionDetected(true, PM.isWallRight);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (transform.position.x > other.transform.position.x)
            {
                PM.WallCollisionDetected(PM.isWallLeft, false);
            }
            else
            {
                PM.WallCollisionDetected(false, PM.isWallRight);
            }
        }
    }
}
