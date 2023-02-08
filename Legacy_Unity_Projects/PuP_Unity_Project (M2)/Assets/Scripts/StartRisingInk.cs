using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRisingInk : MonoBehaviour
{
    /**
     * Script to tell rising ink to start rising when player enters collider
     * 
     * put this script on the empty game object that the rising ink is under
     */

    //public bool canRise;

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
            RisingInk.canRise = true;
        }
    }

}
