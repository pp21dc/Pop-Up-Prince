using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceFootCollider : MonoBehaviour
{
    float friciton;
    int contacts = 0;
    PrinceMovement PM;
    Quaternion camRotation;

    public MeshFilter body;
    public GameObject player;
    public GameObject camera;

    private void Start()
    {
        camRotation = new Quaternion(0, 0, 0, 0);
        camRotation.eulerAngles.Set(15, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.transform.tag == "ground")
        {
            contacts++;

            //Debug.Log(contacts + " :CONTACTS");
            PM = player.GetComponent<PrinceMovement>();
            friciton = other.GetComponent<Ground>().friction;
            

            float gY = other.GetComponent<MeshFilter>().mesh.bounds.extents.y;
            float pY = body.mesh.bounds.extents.y;

            if (PM.isFloor)
            {
                //Debug.Log(other.transform.position.y + gY < player.transform.position.y);
                PM.CollisionDetected(true, friciton);
            }


            //Debug.Log(other.transform.rotation);
            transform.parent.SetPositionAndRotation(transform.parent.transform.position, new Quaternion(0, 0, other.transform.rotation.z, other.transform.rotation.w));
            FollowPlayer script_followPlayer = camera.GetComponent<FollowPlayer>(); 
            Quaternion pRotation = transform.parent.transform.rotation;
            pRotation.eulerAngles = new Vector3(script_followPlayer.rotation.x,0,0);
            camera.transform.SetPositionAndRotation(new Vector3(transform.parent.position.x, transform.parent.transform.position.y + script_followPlayer.height, camera.transform.position.z), pRotation);
            //player.transform.position = new Vector3(player.transform.position.x, other.transform.position.y, 0);


        } 
        
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "ground")
        {
            contacts--;
            if (contacts < 0)
            {
                contacts = 0;
            }

            //Debug.Log(contacts + " :CONTACTS E");
            if (other.transform.tag == "ground" & contacts <= 0)
            {
                PM = player.GetComponent<PrinceMovement>();
                PM.CollisionDetected(false, PM.friction_air); //sets friction to air
            }
        }

    }

}
