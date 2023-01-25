using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    bool pop = false;
    float FPS = 60;
    public float POP_SPEED = 1f;
    public float POP_TIME = 1f;

    private void Start()
    {
        //FPS = Application.targetFrameRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.rotation.x < 0 && pop)
        {
            //Debug.Log(FPS);
            transform.Rotate((POP_SPEED / (FPS*POP_TIME)), 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            pop = true;
        }
    }


}
