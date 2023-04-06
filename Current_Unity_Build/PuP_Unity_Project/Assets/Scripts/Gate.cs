using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    /**
     * Script now for flag raising
     */

    public float moveSpeed = 3f;
    public float moveTime = 1f;
    float startPos;
    public float riseMultiplier = 2;
    public bool rise = false;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        if (rise == true)
        {
            //move object up
            transform.Translate(Vector3.left * (moveSpeed * Time.deltaTime));

            if (transform.position.y > (Mathf.Abs(startPos) * riseMultiplier))
            {
                rise = false;
            }

        }

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {

            //tell flag to rise
            rise = true;

        }
    }
}
