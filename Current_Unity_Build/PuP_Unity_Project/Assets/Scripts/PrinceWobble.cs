using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceWobble : MonoBehaviour
{
    public PrinceMovement PM;
    public float rotate_max = 15f;
    public float rotate_speed = 1f;
    int dir = 1;

    // Update is called once per frame
    void Update()
    {
        if (dir == 1 && (PM.current_speed.x > 0 || PM.current_speed.x < 0))
        {
            if (transform.rotation.eulerAngles.z >= rotate_max)
            {
                dir = -1;
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, rotate_speed * Time.deltaTime));
            }
           
        }
        else if (dir == -1 && (PM.current_speed.x > 0 || PM.current_speed.x < 0))
        {
            if (transform.rotation.eulerAngles.z <= -rotate_max)
            {
                dir = 1;
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, -rotate_speed * Time.deltaTime));
            }

        }
        else if (PM.current_speed.x == 0)
        {

        }
    }
}
