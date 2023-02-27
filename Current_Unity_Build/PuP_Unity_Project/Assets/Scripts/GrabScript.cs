using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabScript : MonoBehaviour
{
    bool grabbed;
    GameObject player;
    PrinceMovement PM;
    void Update()
    {
        if (grabbed)
        {
            Debug.Log("Grabbed");
            player.transform.position = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            grabbed = true;
            player = other.gameObject;
            PM = other.transform.parent.GetComponent<PrinceMovement>();
            PM.grabbed = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

}
