using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    /**
     * Script for unlocking and raising the gates
     */

    public float moveSpeed = 3f;
    public float moveTime = 1f;
    float startPos;
    public bool lower = false;
    public bool rise = false;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        if (lower == true)
        {
            //move object up
            transform.Translate(Vector3.down * (moveSpeed * Time.deltaTime), Space.World);

            if (transform.position.y < (startPos / 2))
            {
                lower = false;
            }

        }

        if (rise == true)
        {
            //move object up
            transform.Translate(Vector3.up * (moveSpeed * Time.deltaTime), Space.World);

            if (transform.position.y > (startPos))
            {
                rise = false;
            }

        }



    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            //tell gate to rise
            lower = true;
            rise = false;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        rise = true;
        lower = false;
    }
}
