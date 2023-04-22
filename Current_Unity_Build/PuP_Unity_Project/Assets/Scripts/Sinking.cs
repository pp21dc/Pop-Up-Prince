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
    public Vector2 height = new Vector2(0,0.51f);

    public float rotate_max = 30f;
    int dir = 1;
    public float rotate_speed = 10f;


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.y;
        towerInk.SetVector("_Offset", new Vector2(0, 0.7f));
    }

    // Update is called once per frame
    void Update()
    {

        if (sink == true)
        {

            if (transform.localEulerAngles.y >= 0 && transform.localEulerAngles.y <= 180)
            {
                if (transform.localEulerAngles.y >= rotate_max)
                {

                    dir = -1;

                }
                else
                {

                    transform.Rotate(new Vector3(0, 0, rotate_speed * Time.deltaTime));
                }
            }
            else
            {
                if (360 - transform.localEulerAngles.y <= -rotate_max)
                {
                    //Debug.Log("2l");
                    dir = -1;

                }
                else
                {

                    transform.Rotate(new Vector3(0, rotate_speed * Time.deltaTime, 0));
                }
            }


            //move object up
            transform.Translate(Vector3.down * (moveSpeed * Time.deltaTime), Space.World);
            height += new Vector2(0,1) * (moveSpeed * Time.deltaTime);
            towerInk.SetVector("_Offset", height);

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
