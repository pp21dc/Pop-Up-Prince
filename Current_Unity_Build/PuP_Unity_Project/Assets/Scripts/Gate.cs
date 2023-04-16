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
    public float riseMultiplier = 3;
    public bool rise = false;

    public bool lock1 = false;

    public GameObject confettiMaster1;

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
            if(lock1 == false)
            {
                translate();
            }

        }

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {

            //tell flag to rise
            rise = true;

            //confettiMaster1.gameObject.GetComponent<playConfetii>().playAllConfetti();

        }
    }

    void translate()
    {
        //move object up
        transform.Translate(Vector3.left * (moveSpeed * Time.deltaTime));

        if (transform.position.y > (Mathf.Abs(startPos) + riseMultiplier))
        {
            rise = false;
            confettiMaster1.gameObject.GetComponent<playConfetii>().playAllConfetti();
            lock1 = true;
        }
     
    }
}
