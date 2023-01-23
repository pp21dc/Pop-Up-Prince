using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceFootCollider : MonoBehaviour
{
    float friciton;
    int contacts = 0;
    PrinceMovement PM;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Enter");
        if (other.transform.tag == "ground")
        {
            PM = transform.parent.GetComponent<PrinceMovement>();
            friciton = other.GetComponent<Ground>().friction;
            PM.CollisionDetected(true, friciton);

            transform.parent.transform.position = new Vector3(transform.parent.transform.position.x, transform.parent.transform.position.y, 0);
            
            contacts++;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        contacts--;
        if (other.transform.tag == "ground" & contacts <= 0)
        {
            PM = transform.parent.GetComponent<PrinceMovement>();
            PM.CollisionDetected(false, PM.friction_air); //sets friction to air
        }
        
    }

}
