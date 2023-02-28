using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkArm : MonoBehaviour
{

    public Transform player;
    PrinceMovement PM;
    bool move = false;

    public Transform ANCHOR;
    //public Collider GRABBER;
    public float ARM_SPEED = 5f;
    public float ARM_REACH = 2f;
    public bool RETURN = false;

    // Start is called before the first frame update
    void Start()
    {
        PM = player.gameObject.GetComponent<PrinceMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveTo = Vector3.MoveTowards(transform.position, player.position, ARM_SPEED * Time.deltaTime);
        Vector3 moveToA = Vector3.MoveTowards(transform.position, ANCHOR.position, ARM_SPEED * Time.deltaTime);
        if (move && (!Stretched(moveTo)))
        {
            transform.position = moveTo;
        }
        else
        {
            transform.position = moveToA;
        } 
    }


    private bool Stretched(Vector3 mT)
    {
        float x = Mathf.Abs(transform.position.x - ANCHOR.position.x);
        float y = Mathf.Abs(transform.position.y - ANCHOR.position.y);

        float mX = Mathf.Abs(mT.x - ANCHOR.position.x);
        float mY = Mathf.Abs(mT.y - ANCHOR.position.y);


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
            //player = other.transform;
            move = true;
            //Debug.Log("Enter");
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
