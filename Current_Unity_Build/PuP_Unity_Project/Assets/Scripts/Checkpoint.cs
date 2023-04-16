using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    PrinceMovement PM;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PM = other.transform.parent.GetComponent<PrinceMovement>();
            PM.Checkpoint = new Vector3(transform.position.x, transform.position.y + 1, 0);
        }
    }
}
