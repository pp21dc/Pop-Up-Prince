using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabScript : MonoBehaviour
{
    [HideInInspector]
    public bool grabbed;
    GameObject player;
    PrinceFootCollider PFC;
    PrinceMovement PM;

    float xOffset;
    float yOffset;

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject;
            PFC = other.transform.GetComponent<PrinceFootCollider>();
            PM = other.transform.parent.GetComponent<PrinceMovement>();
            PFC.PM.current_grab = gameObject.GetComponent<GrabScript>();


            if (!PM.dashing)
            {
                grabbed = true;
                PFC.PM.grabbed = true;
            }

        }
        
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            player = collision.gameObject;
            PFC = collision.transform.GetComponent<PrinceFootCollider>();
            PM = collision.transform.parent.GetComponent<PrinceMovement>();
            PFC.PM.current_grab = gameObject.GetComponent<GrabScript>();


            if (!PM.dashing)
            {
                grabbed = true;
                PFC.PM.grabbed = true;
            }

            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && PM.dashing && !PM.respawn)
        {
            grabbed = false;
            player = other.gameObject;
            PFC = other.transform.GetComponent<PrinceFootCollider>();
            PFC.PM.current_grab = null;
            PFC.PM.grabbed = false;
            PFC.CheckCollisions(PFC.PM.currentGroundScript.gameObject, player);
            PFC.SetPlayerY(other);
        }
        else if (PM.respawn && other.tag == "Player")
        {
            grabbed = false;
            player = other.gameObject;
            PFC = other.transform.GetComponent<PrinceFootCollider>();
            PFC.PM.current_grab = null;
            PFC.PM.grabbed = false;
            PFC.CheckCollisions(PFC.PM.currentGroundScript.gameObject, player);
            PFC.SetPlayerY(other);
        }


    }

}
