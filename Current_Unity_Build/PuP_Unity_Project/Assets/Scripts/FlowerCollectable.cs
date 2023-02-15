using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerCollectable : MonoBehaviour
{

    /**
    * Script for the flower collectables
    * picks up flower and destroys game object
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
            if (PM.flowerCount < 3)
            {
                PM.flowerCount++;
                this.gameObject.SetActive(false);
            }
        }
    }

}
