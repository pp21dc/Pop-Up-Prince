using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceWobble : MonoBehaviour
{
    public PrinceMovement PM;
    public bool acclerationRotation = true;
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
    public GameObject dashAlt_model;
    public GameObject key_model;

    bool idle = false;
    bool jump = false;
    bool dash = false;
    bool dashAlt = false;

    float keypos1;
    float keypos2;
    public float key_moveMod = 0.2f;

    private void Start()
    {
        keypos1 = key_model.transform.localPosition.x;
        keypos2 = key_model.transform.localPosition.x + key_moveMod;
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
            dashAlt_model.SetActive(false);
            
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
            dashAlt_model.SetActive(false);
        }
        else if (dash)
        {
            key_model.transform.localPosition = new Vector3(keypos2, key_model.transform.localPosition.y, key_model.transform.localPosition.z);
            idle_model.SetActive(false);
            jump_model.SetActive(false);
            dash_model.SetActive(true);
            dashAlt_model.SetActive(false);
        }
        else if (dashAlt)
        {
            key_model.transform.localPosition = new Vector3(keypos2, key_model.transform.localPosition.y, key_model.transform.localPosition.z);
            idle_model.SetActive(false);
            jump_model.SetActive(false);
            dash_model.SetActive(false);
            dashAlt_model.SetActive(true);
        }
    }

    private void state()
    {
        if (PM.dashing && PM.speed_dashx < 0)
        {
            idle = false;
            jump = false;
            dash = true;
            dashAlt = false;
        }
        else if (PM.dashing && PM.speed_dashx > 0)
        {
            idle = false;
            jump = false;
            dash = false;
            dashAlt = true;
        }
        else if (!PM.isFloor)
        {
            idle = false;
            jump = true;
            dash = false;
            dashAlt = false;
        }
        else if (PM.isFloor)
        {
            idle = true;
            jump = false;
            dash = false;
            dashAlt = false;
        }
        else
        {
            idle = true;
            jump = false;
            dash = false;
            dashAlt = false;
        }

        idle_model.SetActive(idle);
        jump_model.SetActive(jump);
        dash_model.SetActive(dash);
        dashAlt_model.SetActive(dashAlt);

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
        if (true)
        {
            if (dir == 1 && ((PM.current_speed.x > 0 && acclerationRotation) || (PM.current_speed.x != 0 && !acclerationRotation)))
            {
                //Debug.Log(transform.rotation.eulerAngles.z + " " + rotate_max);
                if (transform.localEulerAngles.z >= 0 && transform.localEulerAngles.z <= 180)
                {
                    if (transform.localEulerAngles.z >= rotate_max)
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
                    if (360 - transform.localEulerAngles.z <= -rotate_max)
                    {
                        //Debug.Log("2l");
                        dir = -1;

                    }
                    else
                    {

                        transform.Rotate(new Vector3(0, 0, rotate_speed * Time.deltaTime));
                    }
                }


            }
            else if (dir == -1 && ((PM.current_speed.x > 0 && acclerationRotation) || (PM.current_speed.x != 0 && !acclerationRotation)))
            {
                //Debug.Log(transform.rotation.eulerAngles.z + " " + rotate_max);
                if (transform.localEulerAngles.z >= 0 && transform.localEulerAngles.z <= 180)
                {
                    if (transform.localEulerAngles.z <= -rotate_max)
                    {
                        //Debug.Log(360 - transform.localEulerAngles.z + " | " + -rotate_max);
                        //Debug.Log("1r");
                        dir = 1;
                    }
                    else
                    {

                        transform.Rotate(new Vector3(0, 0, -rotate_speed * Time.deltaTime));
                    }
                }
                else
                {
                    if (360 - transform.localEulerAngles.z >= rotate_max)
                    {
                        //Debug.Log(360 - transform.localEulerAngles.z + " | " + -rotate_max);
                        //Debug.Log("2r");
                        dir = 1;
                    }
                    else
                    {

                        transform.Rotate(new Vector3(0, 0, -rotate_speed * Time.deltaTime));
                    }
                }


            }
            else if (PM.current_speed.x == 0)
            {
                //Debug.Log("RESET");
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
        }
        else
        {
            
        }
    }

}
