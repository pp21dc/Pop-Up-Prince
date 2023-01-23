using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkArm : MonoBehaviour
{

    Transform target;
    bool move = false;

    public Transform ANCHOR;
    public Collider GRABBER;
    public float ARM_SPEED = 5f;
    public float ARM_REACH = 2f;
    public bool RETURN = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((move && !Stretched()))
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, ARM_SPEED * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, ANCHOR.position, ARM_SPEED * Time.deltaTime);
        }
    }

    private bool Stretched()
    {
        float x = Mathf.Abs(transform.position.x - ANCHOR.position.x);
        float y = Mathf.Abs(transform.position.y - ANCHOR.position.y);

        if (Mathf.Pow(x, 2) + Mathf.Pow(y, 2) < Mathf.Pow(ARM_REACH, 2))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            target = other.transform;
            move = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            move = false;
        }
    }

}
