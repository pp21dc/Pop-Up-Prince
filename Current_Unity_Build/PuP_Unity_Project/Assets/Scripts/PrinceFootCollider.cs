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
    [Tooltip("How much the player can walk on the point of a slope before being affected by the next slope")]
    public float pointedFallOff = 0.5f;
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

    float dC = 0;

    private void Start()
    {
        camRotation = new Quaternion(0, 0, 0, 0);
        camRotation.eulerAngles.Set(15, 0, 0);
    }

    private void SetPlayerY(Collider other, float gY, float pY_bounds, float gX)
    {
        float px = player.transform.position.x;
        float py = player.transform.position.y; //Changed, removed bound minus
        float gx = ground_script.trans.position.x;
        float gy = ground_script.trans.position.y;
        float slopeY = (ground_script.slope) * ((px) - gx) + gy;

        if (ground_script.top && (!ground_script.left && !ground_script.right) && ground_script.slope == 0 && !PM.isSlope) //hitting flat ground
        {
            Debug.Log(0);
            player.transform.SetPositionAndRotation(new Vector3(px, gy + mF_yBounds + player_yBounds, 0), new Quaternion(0, 0, other.transform.rotation.z, other.transform.rotation.w));
        }
        else if (ground_script.left || ground_script.right && ground_script.slope == 0 && !PM.isSlope) // Hitting a wall and player is not currently on a slope
        {
            Debug.Log(1);
            //Debug.Log("Is Slope: " + PM.isSlope);
            if (ground_script.left && !PM.isSlope)
            {
                Debug.Log(1.1);
                player.transform.SetPositionAndRotation(new Vector3(ground_script.rightSide.x + player_xBounds, py, 0), new Quaternion(0, 0, other.transform.rotation.z, other.transform.rotation.w));
            }
            else if (ground_script.right && !PM.isSlope)
            {
                Debug.Log(1.2);
                player.transform.SetPositionAndRotation(new Vector3(ground_script.leftSide.x - player_xBounds, py, 0), new Quaternion(0, 0, other.transform.rotation.z, other.transform.rotation.w));
            }
        }
        else if (ground_script.slope != 0 || (ground_script.slope == 0 && PM.isSlope) && (!ground_script.left && !ground_script.right)) // On a slope or leaving a slope to a flat surface
        {
            

            float xBoundDir = findDirection(px);
            slopeY = (ground_script.slope) * ((px + (player_xBounds * 2.5f * xBoundDir)) - gx) + (ground_script.verticalWidthAP);
            if (ground_script.slope != 0 && PM.currentSlope == 0)
            {
                Debug.Log(2.1);
                player.transform.SetPositionAndRotation(new Vector3((px) + (player_xBounds * 2.5f * xBoundDir), gy + slopeY + player_yBounds, 0), other.transform.rotation);
            }
            else if (PM.isSlope && xBoundDir == 0)
            {
                Debug.Log(2.2);
                Debug.Log(PM.currentSlope);
                player.transform.SetPositionAndRotation(new Vector3((px) + (player_xBounds * 2.5f * xBoundDir), py, 0), player.transform.rotation);
            }
            else
            {
                Debug.Log(2.3);
                player.transform.SetPositionAndRotation(new Vector3((px) + (player_xBounds * 2.5f * xBoundDir), gy + mF_yBounds + player_yBounds, 0), other.transform.rotation);
            }
            
            PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, true, PM.isRoof, PM.friction_current);
            ground_script.top = true;
        }

    }

    private float findDirection(float px)
    {
        float xBoundDir = 0;
        Debug.Log(PM.currentGroundScript.gameObject.name);
        if (ground_script.slope > 0 && PM.currentSlope == 0)
        {
            if ((px + player_xBounds < ground_script.rightSide.x && px > ground_trans.position.x) && contacts != 0)
            {
                Debug.Log(9.20);
                xBoundDir = -1;//Player is on left side now
            }
            else if (contacts != 0)
            {
                Debug.Log(9.21);
                xBoundDir = 1;
            }
            else
            {
                Debug.Log(9.22);
                xBoundDir = 0;
            }
        }
        else if (ground_script.slope < 0 && PM.currentSlope == 0)
        {
            if ((px - player_xBounds > ground_script.leftSide.x && px < ground_trans.position.x) && contacts != 0)
            {
                Debug.Log(19.20);
                xBoundDir = 1;//Player is on left side now
            }
            else if (contacts != 0)
            {
                Debug.Log(19.21);
                xBoundDir = -1;
            }
            else
            {
                Debug.Log(19.22);
                xBoundDir = 0;
            }
        }
        else
        {
            if (contacts == 0)
            {
                Debug.Log(8.1);
                xBoundDir = 0;
            }
            else if(PM.currentGroundScript.slope > 0)
            {
                if (px < PM.currentGroundScript.trans.position.x)
                {
                    xBoundDir = -1;
                }
                else
                {
                    xBoundDir = 0;
                }
            } 
            else if (PM.currentGroundScript.slope < 0)
            {
                if (px > PM.currentGroundScript.trans.position.x)
                {
                    xBoundDir = 1;
                }
                else
                {
                    xBoundDir = 0;
                }
            }
            else
            {
                Debug.Log(8.2);
                xBoundDir = 0;
            }
            //Debug.Log(1.1);
        }
        return xBoundDir;
    }


    private void slopeCalc(Vector4 slope, float px, float gx)
    {
        //slope (x-b, y-c, z-B, w-C)
        slope.x = Mathf.Abs(px - gx);

        if (ground_trans.eulerAngles.z > 0 && ground_trans.eulerAngles.z < 90)
        {
            //b = px - gx;
            slope.z = Mathf.Abs(ground_trans.eulerAngles.z);
            slope.w = 90f - slope.w;
            slope.y = (slope.x * (Mathf.Sin(slope.w))) / (Mathf.Sin(slope.z));
        }
        else if (ground_trans.eulerAngles.z < 0 || ground_trans.eulerAngles.z > 90)
        {
            //b = px-gx;
            slope.z = 360f - Mathf.Abs(ground_trans.eulerAngles.z);
            slope.w = 90f - slope.z;
            slope.y = (slope.x * (Mathf.Sin(slope.w))) / (Mathf.Sin(slope.z));
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



        if (groundObject.transform.eulerAngles.z == 0)
        {
            //Debug.Log("Collide with Non Anlge" + groundObject.name);
            //Left Wall Detection
            if (body_trans.position.x - player_xBounds - (player_sinkX) <= ground_script.rightSide.x && body_trans.position.x - player_xBounds - (player_sinkX) >= ground_script.rightSide.x - (player_sinkX * 2))
            {
                //Debug.Log("LEFT");
                if (PM.isSlope && body_trans.position.y  > ground_script.rightSide.y)
                {
                    //Debug.Log("LEFT SKIP");
                }
                else if (body_trans.position.y - player_yBounds - player_sinkY/2 < ground_trans.position.y + mF_yBounds && body_trans.position.y - player_yBounds > ground_trans.position.y + mF_yBounds - (player_sinkY))
                {
                    PM.CollisionDetected(true, PM.isWallRight, PM.isFloor, PM.isRoof, PM.friction_current);
                    ground_script.left = true;
                    //Debug.Log("Left Enter");
                    if (player.transform.eulerAngles.z != 0)
                    {
                        //Debug.Log("Slope!");
                        PM.isSlope = true;
                    }

                }
            }

            //Right Wall Detection
            if (body_trans.position.x + player_xBounds + (player_sinkX) >= ground_script.leftSide.x && body_trans.position.x + player_xBounds + (player_sinkX) <= ground_script.leftSide.x + (player_sinkX * 2))
            {
                //Debug.Log("RIGHT");
                //Debug.Log(ground_script.rightSide.x);
                if (PM.isSlope && body_trans.position.y > ground_script.leftSide.y)
                {
                    //Debug.Log("RIGHT SKIP");
                }
                else if (body_trans.position.y - player_yBounds - player_sinkY/2 < ground_trans.position.y + mF_yBounds && body_trans.position.y - player_yBounds > ground_trans.position.y + mF_yBounds - (player_sinkY))
                {
                    
                    PM.CollisionDetected(PM.isWallLeft, true, PM.isFloor, PM.isRoof, PM.friction_current);
                    ground_script.right = true;
                    //Debug.Log("Right Enter");
                    if (player.transform.eulerAngles.z != 0)
                    {
                        PM.isSlope = true;
                    }
                }
            }

            //Roof
            if (body_trans.position.y + player_yBounds > ground_trans.position.y - mF_yBounds && body_trans.position.y + player_yBounds < ground_trans.position.y - mF_yBounds + (player_sinkY * 4))
            {
                PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, PM.isFloor, true, PM.friction_current);
                ground_script.roof = true;
                //Debug.Log("Roof");
                if (player.transform.eulerAngles.z != 0)
                {
                    PM.isSlope = true;
                }
            }

            //Top Detection
            if (!ground_script.left && !ground_script.right && !ground_script.roof)
            {
                PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, true, PM.isRoof, friciton);
                ground_script.top = true;
                //Debug.Log("Top Enter");
                if (player.transform.eulerAngles.z == 0 || contacts == 0)
                {
                    //Debug.Log("NO SLOPE");
                    PM.isSlope = false;
                }
                else if (contacts == 0)
                {
                    PM.isSlope = true;
                }
            }
            else if (body_trans.position.y - player_yBounds <= ground_trans.position.y + mF_yBounds && body_trans.position.y - player_yBounds >= ground_trans.position.y + mF_yBounds - (player_sinkY * 2))
            {
                PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, true, PM.isRoof, friciton);
                ground_script.top = true;
                //Debug.Log("Top Enter 2");
                if (player.transform.eulerAngles.z == 0)
                {
                    //Debug.Log("NO SLOPE 2");
                    PM.isSlope = false;
                }
            }

        }
        else
        {
            /*if (player.transform.position.x - player_xBounds < ground_script.leftSide.x)
            {
                PM.CollisionDetected(PM.isWallLeft, true, PM.isFloor, PM.isRoof, friciton);
                ground_script.right = true;
                Debug.Log("RIGHT");
            } 
            else if (player.transform.position.x + player_xBounds > ground_script.rightSide.x)
            {
                
                PM.CollisionDetected(true, PM.isWallRight, PM.isFloor, PM.isRoof, friciton);
                ground_script.left = true;
                Debug.Log("LEFT");
            } */
        }

        
    }

    private void isSlope()
    {
        if (player.transform.eulerAngles.z == 0)
        {
            PM.isSlope = false;
        }
        else
        {
            PM.isSlope = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

       
        if (other.transform.tag == "ground")
        {

            PM = player.GetComponent<PrinceMovement>();
            PM.previousGroundScript = ground_script;
            ground_script = other.GetComponent<Ground>();
            friciton = ground_script.friction;
            mF = other.GetComponent<MeshFilter>();
            bmF = body.GetComponent<MeshFilter>();
            float gY = other.GetComponent<MeshFilter>().mesh.bounds.extents.y;
            float pY = body.GetComponent<MeshFilter>().mesh.bounds.extents.y;


            isSlope();
            CheckCollisions(other.gameObject, body);
            SetPlayerY(other, ground_trans.position.y + mF_yBounds, player_yBounds, mF_yBounds);
            PM.currentSlope = ground_script.slope;
            PM.currentGroundScript = ground_script;
            contacts++;
            SetPlayandCamRotandPos();
            isSlope();

            Debug.Log("===== ENTER END =====");

        } 
        else if (other.transform.tag == "checkpoint")
        {
            Checkpoint = new Vector3 (other.transform.position.x, other.transform.position.y, 0);
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


            PM.previousSlope = ground_script.slope;
            if (player.transform.eulerAngles.z == 0 || contacts == 0) //RECENT CHANGE
            {
                PM.currentSlope = 0;
                PM.isSlope = false;
                PM.isRoof = false;
                ground_script.roof = false;
                if (contacts == 0)
                {
                    Debug.Log(-500000);
                    PM.isFloor = false;
                    PM.isWallLeft = false;
                    PM.isWallRight = false;
                    
                    ground_script.left = false;
                    ground_script.right = false;
                    ground_script.top = false;
                }
            }

            

            //Off the ground
            if (contacts <= 0 || body_trans.position.y - player_yBounds <= ground_trans.position.y + mF_yBounds && body_trans.position.y - player_yBounds > ground_trans.position.y + mF_yBounds - (player_sinkY * 2))
            {
                if (!PM.isSlope)
                {
                    PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, false, PM.isRoof, PM.friction_air);
                    ground_script.top = false;
                   // Debug.Log("Top Exit");
                }
            }

            

            //Nothing Above
            if (body_trans.position.y + player_yBounds < ground_trans.position.y - mF_yBounds && body_trans.position.y + player_yBounds < ground_trans.position.y - mF_yBounds + (player_sinkY * 2))
            {
                PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, PM.isFloor, false, PM.friction_current);
                ground_script.roof = false;
                //Debug.Log("No Roof");
            }


            //Leaving Left Wall
            if (ground_script.left)
            {
                if (ground_script.slope > 0)
                {
                    PM.CollisionDetected(false, PM.isWallRight, PM.isFloor, PM.isRoof, PM.friction_current);
                    ground_script.left = false;
                }

                if ((body_trans.position.y - player_yBounds > ground_trans.position.y + mF_yBounds && body_trans.position.y - player_yBounds > ground_trans.position.y + mF_yBounds - (player_sinkY * 2)))
                {
                    PM.CollisionDetected(false, PM.isWallRight, PM.isFloor, PM.isRoof, PM.friction_air);
                    ground_script.left = false;
                    //Debug.Log("Left Exit, Height");
                }
                else if (body_trans.position.y + player_yBounds < ground_trans.position.y - mF_yBounds && body_trans.position.y + player_yBounds < ground_trans.position.y - mF_yBounds + (player_sinkY * 2)) 
                {
                    PM.CollisionDetected(false, PM.isWallRight, PM.isFloor, PM.isRoof, PM.friction_air);
                    ground_script.left = false;
                    //Debug.Log("Left Exit, Height");
                } 
                else if (body_trans.position.x - player_xBounds > ground_trans.position.x + (ground_script.trans.localScale.x / 2) && body_trans.position.x - player_xBounds > ground_trans.position.x + (ground_script.trans.localScale.x / 2) - (player_sinkX * 2))
                {
                    PM.CollisionDetected(false, PM.isWallRight, PM.isFloor, PM.isRoof, PM.friction_air);
                    ground_script.left = false;
                    //Debug.Log("Left Exit");
                }

            }

            //Leaving Right Wall
            if (ground_script.right)
            {
                if (ground_script.slope < 0)
                {
                    PM.CollisionDetected(PM.isWallLeft, false, PM.isFloor, PM.isRoof, PM.friction_current);
                    ground_script.right = false;
                }

                if ((body_trans.position.y - player_yBounds > ground_trans.position.y + mF_yBounds && body_trans.position.y - player_yBounds > ground_trans.position.y + mF_yBounds - (player_sinkY * 2)))
                {
                    PM.CollisionDetected(PM.isWallLeft, false, PM.isFloor, PM.isRoof, PM.friction_air);
                    ground_script.right = false;
                    //Debug.Log("Right Exit, Height");
                }
                else if (body_trans.position.y + player_yBounds < ground_trans.position.y - mF_yBounds && body_trans.position.y + player_yBounds < ground_trans.position.y - mF_yBounds + (player_sinkY * 2))
                {
                    PM.CollisionDetected(PM.isWallLeft, false, PM.isFloor, PM.isRoof, PM.friction_air);
                    ground_script.right = false;
                    //Debug.Log("Right Exit");
                }
                else if (body_trans.position.x + player_xBounds < ground_trans.position.x - (ground_script.trans.localScale.x/2) && body_trans.position.x + player_xBounds < ground_trans.position.x - (ground_script.trans.localScale.x/2) + (player_sinkX * 2))
                {
                    PM.CollisionDetected(PM.isWallLeft, false, PM.isFloor, PM.isRoof, PM.friction_air);
                    ground_script.right = false;
                    //Debug.Log("Right Exit");
                }
            }
        }

        Debug.Log("===== EXIT END =====");

    }

}
