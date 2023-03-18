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
        
        if (PFC != null && grabbed)
        {
            Debug.Log(PM.dashing);
            if (PM.dashing)
            {
                //
                grabbed = false;
                PM.grabbed = false;
            }
            player.transform.parent.transform.position = new Vector3(transform.position.x, transform.position.y - PFC.TR.transform.localPosition.y, player.transform.position.z);
        }
        else
        {
            //Debug.Log(PM.dashing);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject;
            PFC = other.transform.GetComponent<PrinceFootCollider>();
            PM = other.transform.parent.GetComponent<PrinceMovement>();

            PFC.PM.current_grab = gameObject.GetComponent<GrabScript>();
            other.transform.parent.transform.position = transform.position;
            xOffset = (transform.position.x - player.transform.position.x);
            yOffset = (transform.position.y - player.transform.position.y);

            Debug.Log("PUSH: " + PM.dashing);

            if (!PM.dashing)
            {
                grabbed = true;
                PFC.PM.grabbed = true;
            }
            
            
        }
        
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            grabbed = false;
            player = other.gameObject;
            PFC = other.transform.GetComponent<PrinceFootCollider>();
            PFC.PM.current_grab = null;
            PFC.PM.grabbed = false;
        }
    }

}
