using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    bool pop = false;
    bool finished = false;

    float FPS = 60;
    public float POP_SPEED = 64f;
    public float POP_TIME = 1f;

    public GameObject popUp;

    HingeJoint hinge;
    float timer = 0;
    private void Start()
    {
        //FPS = Application.targetFrameRate;
         hinge = GetComponent<HingeJoint>();

    }

    // Update is called once per frame
    void Update()
    {
            //Debug.Log(transform.rotation.x);
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            pop = true;
            hinge.useSpring = true;
        }
    }


}
