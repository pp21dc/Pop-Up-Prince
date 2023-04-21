using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sinking : MonoBehaviour
{
    /**
         * Script now for tower sinking
         */

    public float moveSpeed = 3f;
    public float moveTime = 1f;
    float startPos;
    public float sinkingMultiplier = 2;
    public bool sink = false;

    public ParticleSystem left;
    public ParticleSystem right;
    public Material towerInk;
    public float height = 0.51f;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        if (sink == true)
        {


            //move object up
            transform.Translate(Vector3.down * (moveSpeed * Time.deltaTime), Space.World);
            height += Vector3.down.y * (moveSpeed * Time.deltaTime);
            towerInk.SetFloat("_Height", height);

            if (transform.position.y < (startPos * sinkingMultiplier))
            {
                sink = false;
                left.Stop();
                right.Stop();
            }

        }

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {

            //tell flag to rise
            sink = true;

            left.Play();
            right.Play();
        }
    }
}
