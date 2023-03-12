using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    bool locked = true;
    PrinceMovement PM;
    Rigidbody RB;
    public Collider DCL;
    public GameObject confettiMasterRightFront; //Empty object containing all 6 confetti particle systems.
    public GameObject confettiMasterLeftFront;
    public GameObject confettiMasterLeftBack;
    public GameObject confettiMasterRightBack;
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
                confettiMasterRightFront.gameObject.GetComponent<playConfetii>().playAllConfetti();
                confettiMasterLeftFront.gameObject.GetComponent<playConfetii>().playAllConfetti();
                confettiMasterLeftBack.gameObject.GetComponent<playConfetii>().playAllConfetti();
                confettiMasterRightBack.gameObject.GetComponent<playConfetii>().playAllConfetti();
            }
        }
    }
}

