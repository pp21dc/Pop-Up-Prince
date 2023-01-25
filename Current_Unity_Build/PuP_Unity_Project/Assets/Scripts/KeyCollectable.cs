using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollectable : MonoBehaviour
{
    /**
     * Script for the key collectable
     * picks up key and destroys game object, need to add key object sticking to player
     */

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
            PrinceVariables.hasKey = true;
            Destroy(gameObject);

            // need to add code to have key object stick to player upon pickup and then dissapear if hasKey is false
        }
    }

}
