using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceWobble : MonoBehaviour
{
    public PrinceMovement PM;
    public float rotate_max = 30f;
    public float bob_height = 5f;
    public float bob_speed = 5f;
    public float rotate_speed = 10f;
    public float stop_threshold = 1f;
    int dir = 1;
    int bob_dir = 1;
    public GameObject idle_model;
    public GameObject jump_model;
    public GameObject dash_model;
    public GameObject key_model;

    bool idle = false;
    bool jump = false;
    bool dash = false;

    float keypos1;
    float keypos2;

    private void Start()
    {
        keypos1 = key_model.transform.localPosition.x;
        keypos2 = key_model.transform.localPosition.x + 0.32f;
    }

    // Update is called once per frame
    void Update()
    {
        state();

        if (idle)
        {
            key_model.transform.localPosition = new Vector3(keypos1, key_model.transform.localPosition.y, key_model.transform.localPosition.z);
            idle_model.SetActive(true);
            jump_model.SetActive(false);
            dash_model.SetActive(false);
            
            rotate();
            if (PM.isFloor)
            {
                bob();
            }
        }
        else if (jump)
        {
            key_model.transform.localPosition = new Vector3(keypos2, key_model.transform.localPosition.y, key_model.transform.localPosition.z);
            idle_model.SetActive(false);
            jump_model.SetActive(true);
            dash_model.SetActive(false);
        }
        else if (dash)
        {
            key_model.transform.localPosition = new Vector3(keypos2, key_model.transform.localPosition.y, key_model.transform.localPosition.z);
            idle_model.SetActive(false);
            jump_model.SetActive(false);
            dash_model.SetActive(true);
        }
    }

    private void state()
    {
        if (PM.dashing)
        {
            idle = false;
            jump = false;
            dash = true;
        }
        else if (!PM.isFloor)
        {
            idle = false;
            jump = true;
            dash = false;
        }
        else if (PM.isFloor)
        {
            idle = true;
            jump = false;
            dash = false;
        }
        else
        {
            idle = true;
            jump = false;
            dash = false;
        }

        idle_model.SetActive(idle);
        jump_model.SetActive(jump);
        dash_model.SetActive(dash);

    }

    private void bob()
    {
        float height = transform.parent.transform.position.y;
        //Debug.Log("Local:" + transform.localPosition.y);
        //Debug.Log("Height:" + bob_height);
        
        if (bob_dir == 1 && transform.localPosition.y < bob_height)
        {
            //Debug.Log(2.1);
            transform.localPosition += new Vector3(0, bob_speed * Time.deltaTime, 0);
            if (transform.localPosition.y > bob_height)
            {
                bob_dir = -1;
            }
        }
        else if (bob_dir == -1 && transform.localPosition.y > 0)
        {
            //Debug.Log(2.2);
            transform.localPosition += new Vector3(0, -(bob_speed * Time.deltaTime), 0);
            if (transform.localPosition.y < 0)
            {
                bob_dir = 1;
            }
        }
        
    }

    private void rotate()
    {
        float rotation = transform.parent.transform.rotation.eulerAngles.z;
        if (dir == 1 && PM.current_speed.x != 0)
        {
            //Debug.Log(transform.rotation.eulerAngles.z + " " + rotate_max);
            if (transform.rotation.eulerAngles.z >= rotate_max && transform.rotation.eulerAngles.z < 180)
            {
                //Debug.Log("Flip to Left");
                dir = -1;

            }
            else
            {
                //Debug.Log("Rotate RIGHT");
                transform.Rotate(new Vector3(0, 0, rotate_speed * Time.deltaTime));
            }

        }
        else if (dir == -1 && PM.current_speed.x != 0)
        {
            //Debug.Log(transform.rotation.eulerAngles.z + " " + rotate_max);
            if (transform.rotation.eulerAngles.z <= 360 - rotate_max && transform.rotation.eulerAngles.z > 180)
            {
                //Debug.Log("Flip to Right");
                dir = 1;
            }
            else
            {
                //Debug.Log("Rotate LEFT");
                transform.Rotate(new Vector3(0, 0, -rotate_speed * Time.deltaTime));
            }

        }
        else
        {
            //Debug.Log("RESET");
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

}
