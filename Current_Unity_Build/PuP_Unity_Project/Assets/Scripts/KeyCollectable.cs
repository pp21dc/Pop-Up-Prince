using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollectable : MonoBehaviour
{
    /**
     * Script for the key collectable
     * picks up key and destroys game object
     */

    PrinceMovement PM;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            PM = collider.gameObject.transform.parent.GetComponent<PrinceMovement>();
            PrinceMovement.hasKey = true;
            this.gameObject.SetActive(false);
            PM.playSound(PM.AS_collect);
        }
    }

}
