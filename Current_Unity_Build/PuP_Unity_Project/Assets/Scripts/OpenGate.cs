using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    bool locked = true;
    PrinceMovement PM;
    Rigidbody RB;
    public Collider DCL;
    public GameObject confettiMaster1; //Empty object containing all 6 confetti particle systems.
    public GameObject confettiMaster2;
    public GameObject confettiMaster3;
    public GameObject confettiMaster4;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(111);
        if (other.tag == "Player")
        {
            PM = other.transform.parent.GetComponent<PrinceMovement>();
            RB = GetComponent<Rigidbody>();
            Debug.Log(222);
            if (PM.hasKey2)
            {
                Debug.Log(333);
                RB.useGravity = true;
                RB.isKinematic = false;
                DCL.gameObject.SetActive(false);
                PM.isWallRight = false;
                PrinceMovement.hasKey = false;
                confettiMaster1.gameObject.GetComponent<playConfetii>().playAllConfetti();
                confettiMaster2.gameObject.GetComponent<playConfetii>().playAllConfetti();
                confettiMaster3.gameObject.GetComponent<playConfetii>().playAllConfetti();
                confettiMaster4.gameObject.GetComponent<playConfetii>().playAllConfetti();
            }
        }
    }
}

