using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{

    public float friction = 32f;
    public PrinceMovement PM;
    MeshFilter mF;

    bool top;
    bool left;
    bool right;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //PM.WallCollisionDetected(false, false, false);
            //Debug.Log(gameObject.name + " ENTERS: TOP-" + top);
            //Debug.Log(other.name + " | " + other.transform.position.y);
            mF = gameObject.GetComponent<MeshFilter>();
            //Debug.Log(other.transform.position.y + " | " + transform.position.y + mF.mesh.bounds.extents.y);
            if (transform.position.x > other.transform.position.x && (transform.position.y + mF.mesh.bounds.extents.y) > other.transform.position.y)
            {
                //Debug.Log("Wall Right");
                PM.WallCollisionDetected(PM.isWallLeft, true, PM.isFloor);
                right = true;
            }
            else if (transform.position.x < other.transform.position.x && (transform.position.y + mF.mesh.bounds.extents.y) > other.transform.position.y)
            {
                //Debug.Log("Wall Left");
                PM.WallCollisionDetected(true, PM.isWallRight, PM.isFloor);
                left = true;
            }

            if (!left && !right)
            {
                //Debug.Log("Yes Floor");
                PM.WallCollisionDetected(PM.isWallLeft, PM.isWallRight, true);
                top = true;
            }
            

            //Debug.Log(PM.isFloor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log(gameObject.name + " EXITS: TOP-" + top);
            mF = gameObject.GetComponent<MeshFilter>();
            if (transform.position.x > other.transform.position.x && !top)
            {
                //Debug.Log("No Wall Right");
                PM.WallCollisionDetected(PM.isWallLeft, false, PM.isFloor);
                right = false;
            }
            else if (transform.position.x<= other.transform.position.x && !top)
            {
                //Debug.Log("No Wall Left");
                PM.WallCollisionDetected(false, PM.isWallRight, PM.isFloor);
                left = false;
            }

            if ((other.transform.position.y > transform.position.y + mF.mesh.bounds.extents.y) || (other.transform.position.x > (transform.position.x + mF.mesh.bounds.extents.x) || other.transform.position.x < (transform.position.x + mF.mesh.bounds.extents.x)))
            {
                
                //Debug.Log("No Floor");
                PM.WallCollisionDetected(PM.isWallLeft, PM.isWallRight, false);
                top = false;
            }

            Debug.Log(PM.isFloor);
        }
    }
}
