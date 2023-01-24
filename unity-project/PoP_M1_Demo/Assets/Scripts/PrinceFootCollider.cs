using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceFootCollider : MonoBehaviour
{
    float friciton;
    int contacts = 0;
    PrinceMovement PM;

    public MeshFilter body;
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.transform.tag == "ground")
        {
            contacts++;

            PM = player.GetComponent<PrinceMovement>();
            friciton = other.GetComponent<Ground>().friction;
            PM.CollisionDetected(true, friciton);

            float gY = other.GetComponent<MeshFilter>().mesh.bounds.extents.y;
            float pY = body.mesh.bounds.extents.y;
            
            

            //Debug.Log(other.transform.rotation);
            transform.parent.SetPositionAndRotation(transform.parent.transform.position, new Quaternion(0, 0, other.transform.rotation.z, other.transform.rotation.w));
            player.transform.position = new Vector3(transform.parent.transform.position.x, transform.position.y - pY, 0);


        }
        
    }


    private void OnTriggerExit(Collider other)
    {   
        contacts--;
        if (contacts < 0)
        {
            contacts = 0;
        }

        if (other.transform.tag == "ground" & contacts <= 0)
        {
            PM = player.GetComponent<PrinceMovement>();
            PM.CollisionDetected(false, PM.friction_air); //sets friction to air
        }

    }

}
