using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingInk : MonoBehaviour
{

    /**
     * Script for the rising ink mechanic,
     * 
     * put this script on the rising ink that is under the empty game object
     */

    float startPos;
    public float moveSpeed = 3f;
    public static bool canRise = false;
    //public float waitTime = 5f;
    public float heightMultiplier = 5f;

    public static GameObject player;

    // Start is called before the first frame update
    void Start()
    {

        //PrinceMovement PM = GameObject.GetComponent<PrinceMovement>();

        startPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (canRise == true)
        {
            //move object up
            transform.Translate(Vector3.up * (moveSpeed * Time.deltaTime), Space.World);

            if (transform.position.y > (startPos * heightMultiplier))
            {
                canRise = false;
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            //PM.Respawn();
            transform.position.y = startPos;
            canRise = false;

        }
    }

}
