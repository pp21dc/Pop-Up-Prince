using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkHand : MonoBehaviour
{
    public GameObject ARM;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //other.GetComponent<PrinceMovement>().GRABBED = true;
            //other.GetComponent<PrinceMovement>().HAND = gameObject;
            //ARM.GetComponent<InkArm>().RETURN = true;
        }
    }

}
