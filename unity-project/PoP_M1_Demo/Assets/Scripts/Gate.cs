using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    /**
     * Script for unlocking and opening the gates
     */

    public float moveSpeed = 5f;

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
        if (collider.gameObject.tag == "Player" && PrinceVariables.hasKey == true)
        {

            PrinceVariables.hasKey = false;

            Destroy(gameObject);

          //transform.Translate(0, moveSpeed * Time.deltaTime, 0);

            // need to add code to have key object dissapear if hasKey is false
        }
    }
}
