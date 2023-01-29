using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    /**
     * Script for unlocking and opening the gates
     */

    public float moveSpeed = 3f;
    public float moveTime = 1f;
    public float counter = 60;
    public bool rise = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (rise == true)
        {
            //move object up
            transform.Translate(Vector3.up * (moveSpeed * Time.deltaTime), Space.World);
            counter--;

            if (counter == 0)
            {
                //rise = false;
            }

        }

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player" && PrinceVariables.hasKey == true)
        {

            // get rid of princes key
            PrinceVariables.hasKey = false;

            //tell gate to rise
            rise = true;

        }
    }
}
