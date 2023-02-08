using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    CharacterController controller;
    Vector3 speed;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") > 1)
        {
            speed = new Vector3(5, speed.y);
        }


        controller.SimpleMove(speed);
    }
}
