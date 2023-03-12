using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkArm : MonoBehaviour
{

    Transform player;
    PrinceFootCollider PFC;
    bool move = false;
    Vector3 player_position = new Vector3(0,0,0);

    public Transform ANCHOR;
    //public Collider GRABBER;
    public float ARM_SPEED = 5f;
    public float ARM_REACH = 2f;
    public bool RETURN = false;
    public GrabScript GS;

    // Start is called before the first frame update
    void Start()
    {
        //GS = gameObject.GetComponentInChildren<GrabScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && PFC != null && GS != null)
        {
            player_position = new Vector3(player.transform.position.x, player.transform.position.y + PFC.TR.transform.localPosition.y / 2, player.transform.position.z);
            Vector3 moveTo = Vector3.MoveTowards(transform.position, player_position, ARM_SPEED * Time.deltaTime);
            Vector3 moveToA = Vector3.MoveTowards(transform.position, ANCHOR.position, ARM_SPEED * Time.deltaTime);
            if (move && (!Stretched(moveTo)) && !GS.grabbed)
            {
                Debug.Log("Move To Player");
                transform.position = moveTo;
                transform.LookAt(player);
            }
            else if (!move || GS.grabbed)
            {
                Debug.Log("Move To Anchor");
                transform.position = moveToA;
            }
        }
    }


    private bool Stretched(Vector3 mT)
    {
        float x = Mathf.Abs(transform.position.x - ANCHOR.position.x);
        float y = Mathf.Abs(transform.position.y - ANCHOR.position.y);

        float mX = Mathf.Abs(mT.x - ANCHOR.position.x);
        float mY = Mathf.Abs(mT.y - ANCHOR.position.y);

        

        if (Mathf.Pow(mX, 2) + Mathf.Pow(mY, 2) < Mathf.Pow(ARM_REACH, 2))
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
            PFC = other.gameObject.GetComponent<PrinceFootCollider>();
            player = other.transform;
            //player = other.transform;
            move = true;
            Debug.Log("Enter");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            move = true;
            Debug.Log("Stay");
        }
           
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PFC = other.gameObject.GetComponent<PrinceFootCollider>();
            player = other.transform;
            move = false;
            Debug.Log("Exit");
        }
    }

}
