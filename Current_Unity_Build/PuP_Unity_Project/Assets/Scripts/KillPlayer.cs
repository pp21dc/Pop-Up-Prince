using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    PrinceMovement PM;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PM = other.transform.parent.GetComponent<PrinceMovement>();
            PM.Respawn();
        }
    }


}
