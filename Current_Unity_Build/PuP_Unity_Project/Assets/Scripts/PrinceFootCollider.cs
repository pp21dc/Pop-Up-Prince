using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceFootCollider : MonoBehaviour
{
    float friciton;
    [Tooltip("Default: 0.05f, can cause jittering if number is too small")]
    public float player_sinkY = 0.05f;
    [Tooltip("Default: 1.0f, avoid changing this value")]
    public float player_sinkX = 1.0f;
    int contacts = 0;
    PrinceMovement PM;
    Ground ground_script;
    MeshFilter mF;
    MeshFilter bmF;
    Quaternion camRotation;

    public GameObject body;
    public GameObject player;
    public GameObject cam;

    float mF_xBounds;
    float mF_yBounds;
    float player_xBounds;
    float player_yBounds;
    Transform body_trans;
    Transform ground_trans;
    Vector3 Checkpoint;

    private void Start()
    {
        camRotation = new Quaternion(0, 0, 0, 0);
        camRotation.eulerAngles.Set(15, 0, 0);
    }

    private void SetPlayerY(Collider other, float gY, float pY_bounds, float gX)
    {
        float px = player.transform.position.x;
        float py = player.transform.position.y - player_yBounds;
        float gx = ground_script.trans.position.x;
        float gy = ground_script.trans.position.y;

        if (ground_script.left || ground_script.right && !ground_script.top)
        {
            if (ground_script.left) 
            {
                //Debug.Log("LPort " + ground_script.trans.position.x);
                player.transform.SetPositionAndRotation(new Vector3(gx + (ground_trans.localScale.x / 2) + (player_xBounds) - (player_sinkX/4), py, 0), new Quaternion(0, 0, other.transform.rotation.z, other.transform.rotation.w));
            }

            if (ground_script.right) {
                //Debug.Log("RPort" + ground_script.trans.position.x);
                player.transform.SetPositionAndRotation(new Vector3(gx - (ground_trans.localScale.x / 2) - (player_xBounds) + (player_sinkX/4), py, 0), new Quaternion(0, 0, other.transform.rotation.z, other.transform.rotation.w));
            }
        }
        else if (!ground_script.left && !ground_script.right && !ground_script.roof)
        {
            if (ground_trans.eulerAngles == new Vector3(0, 0, 0) || player.transform.position.x == ground_trans.position.x)
            {
                Debug.Log("Flat");
                player.transform.SetPositionAndRotation(new Vector3(px, gY + pY_bounds - player_sinkY / 4, 0), new Quaternion(0, 0, other.transform.rotation.z, other.transform.rotation.w));
            }
            else if (!ground_script.roof)
            {
                Debug.Log(ground_trans.eulerAngles.z);
                float c;
                float C = 0;
                float B = 0;
                float b = 0;
                if (ground_trans.eulerAngles.z > 0 && ground_trans.eulerAngles.z < 90)
                {
                    c = player_xBounds;
                    B = Mathf.Abs(ground_trans.eulerAngles.z);
                    C = 90 - B;
                    b = Mathf.Abs((c * (Mathf.Sin(B))) / (Mathf.Sin(C)));
                }
                else if (ground_trans.eulerAngles.z < 0 || ground_trans.eulerAngles.z > 90)
                {
                    c = player_xBounds;
                    B = 360f - Mathf.Abs(ground_trans.eulerAngles.z);
                    C = 90 - B;
                    b = Mathf.Abs((c * (Mathf.Sin(B))) / (Mathf.Sin(C)));
                }

                Debug.Log(C);
                Debug.Log(B);
                Debug.Log(b);


                player.transform.SetPositionAndRotation(new Vector3(px, py - b + player_yBounds, 0), other.transform.rotation);

                

                /*Debug.Log("Slope");
                //player.transform.SetPositionAndRotation(new Vector3(px, player.transform.position.y - player_sinkY / 4 , 0), other.transform.rotation);
                if (py > gy)
                {
                    if (px < gx)
                    {
                        pythagPosYCalcs("R", other);
                    }
                    else if (px > gx)
                    {
                        pythagPosYCalcs("L", other);
                    }
                }
                else if (py < gy)
                {
                    if (px > gx)
                    {
                        pythagPosYCalcs("BR", other);
                    }
                    else if (px < gx)
                    {
                        pythagPosYCalcs("BL", other);
                    }
                }*/

                PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, true, PM.isRoof, PM.friction_current);

            }
            
        }
        else if (ground_script.roof)
        {
            player.transform.SetPositionAndRotation(new Vector3(player.transform.position.x, ground_script.trans.position.y - mF_yBounds - pY_bounds + player_sinkY / 4, 0), new Quaternion(0, 0, other.transform.rotation.z, other.transform.rotation.w));
        }
    }

    private void pythagPosYCalcs(string key, Collider other)
    {
        float px = player.transform.position.x;
        float py = player.transform.position.y - player_yBounds;
        float gx = ground_script.trans.position.x;
        float gy = ground_script.trans.position.y;

        if (key == "R")
        {
            Debug.Log("R");
            float x = (gx - px);
            float z = Mathf.Abs(Vector3.Distance(new Vector3(gx, gy, 0), new Vector3(px, py - player_yBounds, 0)));
            float y = Mathf.Sqrt((Mathf.Pow(z, 2) - Mathf.Pow(x, 2)));
            Debug.Log(Mathf.Sqrt((Mathf.Pow(z, 2) - Mathf.Pow(x, 2))) == y);
            player.transform.SetPositionAndRotation(new Vector3(px, py, 0), other.transform.rotation);
            
        }
        else if (key == "L")
        {
            Debug.Log("L");
            float x = (px - gx);
            float z = Mathf.Abs((Vector3.Distance(new Vector3(gx, gy, 0), new Vector3(px, py - player_yBounds, 0))));
            float y = Mathf.Sqrt((Mathf.Pow(z, 2) - Mathf.Pow(x, 2)));
            Debug.Log(Mathf.Sqrt((Mathf.Pow(z, 2) - Mathf.Pow(x, 2))) == y);
            player.transform.SetPositionAndRotation(new Vector3(px, gy + y - (player_sinkY) + (player_yBounds * 2), 0), other.transform.rotation);
        }
        else if (key == "BR")
        {
            Debug.Log("BR");
            float x = gx - px;
            float z = Mathf.Abs((Vector3.Distance(new Vector3(gx, gy, 0), new Vector3(px, py - player_yBounds, 0))));
            float y = Mathf.Sqrt((Mathf.Pow(z, 2) - Mathf.Pow(x, 2)));
            Debug.Log(Mathf.Sqrt((Mathf.Pow(z, 2) - Mathf.Pow(x, 2))) == y);
            player.transform.SetPositionAndRotation(new Vector3(px, gy - y - player_sinkY + (player_yBounds * 2), 0), other.transform.rotation);

        }
        else if (key == "BL")
        {
            Debug.Log("BL");
            float x = gx - px;
            float z = Mathf.Abs((Vector3.Distance(new Vector3(gx, gy, 0), new Vector3(px, py - player_yBounds, 0))));
            float y = Mathf.Sqrt((Mathf.Pow(z, 2) - Mathf.Pow(x, 2)));
            Debug.Log(Mathf.Sqrt((Mathf.Pow(z, 2) - Mathf.Pow(x, 2))) == y);
            player.transform.SetPositionAndRotation(new Vector3(px, gy - y - player_sinkY + player_yBounds, 0), other.transform.rotation);

        }
    }

    private void SetPlayandCamRotandPos()
    {
        FollowPlayer script_followPlayer = cam.GetComponent<FollowPlayer>();
        Quaternion pRotation = player.transform.rotation;
        pRotation.eulerAngles = new Vector3(script_followPlayer.rotation.x, 0, 0);
        cam.transform.SetPositionAndRotation(new Vector3(player.transform.position.x, player.transform.position.y + script_followPlayer.height, cam.transform.position.z), pRotation);
    }

    private void CheckCollisions(GameObject groundObject, GameObject playerBody)
    {
        mF_xBounds = (groundObject.transform.localScale.x/2);
        mF_yBounds = (groundObject.transform.localScale.y/2);
        player_xBounds = (playerBody.transform.localScale.x/2);
        player_yBounds = (playerBody.transform.localScale.y/2);
        body_trans = playerBody.transform;
        ground_trans = groundObject.transform;


        //Left Wall Detection
        if (body_trans.position.x - player_xBounds - (player_sinkX) <= ground_trans.position.x + mF_xBounds && body_trans.position.x - player_xBounds - (player_sinkX) >= ground_trans.position.x + mF_xBounds - (player_sinkX*2))
        {
            if (body_trans.position.y - player_yBounds - player_sinkY < ground_trans.position.y + mF_yBounds && body_trans.position.y - player_yBounds < ground_trans.position.y + mF_yBounds - (player_sinkY * 2))
            {
                PM.CollisionDetected(true, PM.isWallRight, PM.isFloor, PM.isRoof, PM.friction_current);
                ground_script.left = true;
                Debug.Log("Left Enter");
            }
        }

        //Right Wall Detection
        if (body_trans.position.x + player_xBounds + (player_sinkX) >= ground_trans.position.x - mF_xBounds && body_trans.position.x + player_xBounds + (player_sinkX) <= ground_trans.position.x - mF_xBounds + (player_sinkX * 2))
        {
            if (body_trans.position.y - player_yBounds - player_sinkY < ground_trans.position.y + mF_yBounds && body_trans.position.y - player_yBounds < ground_trans.position.y + mF_yBounds - (player_sinkY * 2))
            {
                PM.CollisionDetected(PM.isWallLeft, true, PM.isFloor, PM.isRoof, PM.friction_current);
                ground_script.right = true;
                Debug.Log("Right Enter");
            }
        }

        //Roof
        if (body_trans.position.y + player_yBounds > ground_trans.position.y - mF_yBounds && body_trans.position.y + player_yBounds < ground_trans.position.y - mF_yBounds + (player_sinkY * 2))
        {
            PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, PM.isFloor, true, PM.friction_current);
            ground_script.roof = true;
            Debug.Log("Roof");
        }

        //Top Detection
        if (!ground_script.left && !ground_script.right && !ground_script.roof)
        {
            PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, true, PM.isRoof, friciton);
            ground_script.top = true;
            Debug.Log("Top Enter");
        }
        else if (body_trans.position.y - player_yBounds <= ground_trans.position.y + mF_yBounds && body_trans.position.y - player_yBounds >= ground_trans.position.y + mF_yBounds - (player_sinkY*2))
        {
            PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, true, PM.isRoof, friciton);
            ground_script.top = true;
            Debug.Log("Top Enter");
        }

        SetPlayerY(groundObject.GetComponent<Collider>(), ground_trans.position.y + mF_yBounds, player_yBounds, mF_yBounds);
    }

    private void OnTriggerEnter(Collider other)
    {

       
        if (other.transform.tag == "ground")
        {
            

            PM = player.GetComponent<PrinceMovement>();
            ground_script = other.GetComponent<Ground>();
            friciton = ground_script.friction;
            mF = other.GetComponent<MeshFilter>();
            bmF = body.GetComponent<MeshFilter>();
            float gY = other.GetComponent<MeshFilter>().mesh.bounds.extents.y;
            float pY = body.GetComponent<MeshFilter>().mesh.bounds.extents.y;

            contacts++;
            CheckCollisions(other.gameObject, body);
            SetPlayandCamRotandPos();


        } 
        else if (other.transform.tag == "checkpoint")
        {
            Checkpoint = other.transform.position;
            PM.Checkpoint = Checkpoint;
        }
        
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "ground")
        {
            PM = player.GetComponent<PrinceMovement>();
            ground_script = other.GetComponent<Ground>();
            mF = other.GetComponent<MeshFilter>();
            bmF = body.GetComponent<MeshFilter>();
            mF_xBounds = (other.transform.localScale.x / 2);
            mF_yBounds = (other.transform.localScale.y / 2);
            player_xBounds = (body.transform.localScale.x/2);
            player_yBounds = (body.transform.localScale.y/2);
            body_trans = body.transform;
            ground_trans = other.gameObject.transform;

            contacts--;
            if (contacts < 0)
            {
                contacts = 0;
            }

            //Off the ground
            if (contacts <= 0 || body_trans.position.y - player_yBounds > ground_trans.position.y + mF_yBounds && body_trans.position.y - player_yBounds > ground_trans.position.y + mF_yBounds - (player_sinkY * 2))
            {
                PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, false, PM.isRoof, PM.friction_air);
                ground_script.top = false;
                Debug.Log("Top Exit");
                Debug.Log(contacts <= 0);
                Debug.Log(body_trans.position.y - player_yBounds > ground_trans.position.y + mF_yBounds);
                Debug.Log(body_trans.position.y - player_yBounds > ground_trans.position.y + mF_yBounds - (player_sinkY * 2));
            }

            //Nothing Above
            if (body_trans.position.y + player_yBounds < ground_trans.position.y - mF_yBounds && body_trans.position.y + player_yBounds < ground_trans.position.y - mF_yBounds + (player_sinkY * 2))
            {
                PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, PM.isFloor, false, PM.friction_current);
                ground_script.roof = false;
                Debug.Log("No Roof");
            }


            //Leaving Left Wall
            if (ground_script.left)
            {
                if ((body_trans.position.y - player_yBounds > ground_trans.position.y + mF_yBounds && body_trans.position.y - player_yBounds > ground_trans.position.y + mF_yBounds - (player_sinkY * 2)))
                {
                    PM.CollisionDetected(false, PM.isWallRight, PM.isFloor, PM.isRoof, PM.friction_air);
                    ground_script.left = false;
                    Debug.Log("Left Exit, Height");
                }
                else if (body_trans.position.y + player_yBounds < ground_trans.position.y - mF_yBounds && body_trans.position.y + player_yBounds < ground_trans.position.y - mF_yBounds + (player_sinkY * 2)) 
                {
                    PM.CollisionDetected(false, PM.isWallRight, PM.isFloor, PM.isRoof, PM.friction_air);
                    ground_script.left = false;
                    Debug.Log("Left Exit, Height");
                } 
                else if (body_trans.position.x - player_xBounds > ground_trans.position.x + (ground_script.trans.localScale.x / 2) && body_trans.position.x - player_xBounds > ground_trans.position.x + (ground_script.trans.localScale.x / 2) - (player_sinkX * 2))
                {
                    PM.CollisionDetected(false, PM.isWallRight, PM.isFloor, PM.isRoof, PM.friction_air);
                    ground_script.left = false;
                    Debug.Log("Left Exit");
                }
            }

            //Leaving Right Wall
            if (ground_script.right)
            {
                //Debug.Log(body_trans.position.y - player_yBounds > ground_trans.position.y + mF_yBounds);
                //Debug.Log(body_trans.position.y - player_yBounds > ground_trans.position.y + mF_yBounds - (player_sinkY * 2));
                if ((body_trans.position.y - player_yBounds > ground_trans.position.y + mF_yBounds && body_trans.position.y - player_yBounds > ground_trans.position.y + mF_yBounds - (player_sinkY * 2)))
                {
                    PM.CollisionDetected(PM.isWallLeft, false, PM.isFloor, PM.isRoof, PM.friction_air);
                    ground_script.right = false;
                    Debug.Log("Right Exit, Height");
                }
                else if (body_trans.position.y + player_yBounds < ground_trans.position.y - mF_yBounds && body_trans.position.y + player_yBounds < ground_trans.position.y - mF_yBounds + (player_sinkY * 2))
                {
                    PM.CollisionDetected(PM.isWallLeft, false, PM.isFloor, PM.isRoof, PM.friction_air);
                    ground_script.right = false;
                    Debug.Log("Right Exit");
                }
                else if (body_trans.position.x + player_xBounds < ground_trans.position.x - (ground_script.trans.localScale.x/2) && body_trans.position.x + player_xBounds < ground_trans.position.x - (ground_script.trans.localScale.x/2) + (player_sinkX * 2))
                {
                    PM.CollisionDetected(PM.isWallLeft, false, PM.isFloor, PM.isRoof, PM.friction_air);
                    ground_script.right = false;
                    Debug.Log("Right Exit");
                }
            }
        }

    }

}
