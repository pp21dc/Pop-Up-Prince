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
        float slopeY = 0;
        slopeY = (ground_script.slope) * ((px) - gx);

        //Debug.Log(other.name + " - Left:" + ground_script.left);
        //Debug.Log(other.name + " - Right:" + ground_script.right);
        //Debug.Log(other.name + " - Roof:" + ground_script.roof);
        //Debug.Log(other.name + " - Top:" + ground_script.top);
        //Debug.Log(PM.isSlope);
        if ((ground_script.left || ground_script.right) && !ground_script.top && !PM.isSlope) //Walls no ground no slope
        {
            if (ground_script.left) 
            {
                Debug.Log("LPort ");
                player.transform.SetPositionAndRotation(new Vector3(gx + (ground_trans.localScale.x / 2) + (player_xBounds) - (player_sinkX/4), py, 0), new Quaternion(0, 0, other.transform.rotation.z, other.transform.rotation.w));
            }

            if (ground_script.right) {
                Debug.Log("RPort");
                player.transform.SetPositionAndRotation(new Vector3(gx - (ground_trans.localScale.x / 2) - (player_xBounds) + (player_sinkX/4), py, 0), new Quaternion(0, 0, other.transform.rotation.z, other.transform.rotation.w));
            }
        }
        else if (!ground_script.left && !ground_script.right && !ground_script.roof) //No walls, No Roof,
        {
            if ((ground_trans.eulerAngles.z == 0) || player.transform.position.x == ground_trans.position.x) //Flat ground
            {
                //
                if (contacts == 0) //Player is not sloped
                {
                    Debug.Log("Flat");

                    player.transform.SetPositionAndRotation(new Vector3(px, gY + pY_bounds - player_sinkY / 4, 0), new Quaternion(0, 0, other.transform.rotation.z, other.transform.rotation.w));
                }
                else if (ground_script.top && PM.isSlope) //This is happening when the player enters a flat surface but they are still on a sloped surface
                {
                    Debug.Log(0);
                    if (player.transform.position.y > gy + slopeY) // If the player is above the slope
                    {
                        float xBoundDir = findDirection(px);

                        Debug.Log("DIR: " + xBoundDir);
                        slopeY = (ground_script.slope) * ((px + (player_xBounds * 2.5f * xBoundDir)) - gx);
                        if (PM.currentSlope == 0)
                        {
                            Debug.Log(6.1);
                            player.transform.SetPositionAndRotation(new Vector3((px) + (player_xBounds * 2.5f * xBoundDir), slopeY + gy + player_yBounds + mF_yBounds, 0), other.transform.rotation);
                        }
                        else
                        {
                            Debug.Log(6.2);
                            player.transform.SetPositionAndRotation(new Vector3((px), py, 0), player.transform.rotation);
                        }
                        //PM.previousSlope = ground_script.slope;
                        PM.currentSlope = ground_script.slope;

                    }
                    PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, true, PM.isRoof, PM.friction_current);
                    ground_script.top = true;

                }
            }
            else if (ground_trans.eulerAngles.z != 0 && !ground_script.roof) //Not on the floor of the new object
            {
                Debug.Log(1);
                if (player.transform.position.y > gy + slopeY) // If the player is above the slope
                {
                    float xBoundDir = findDirection(px);

                    Debug.Log("DIR: " + xBoundDir);
                    slopeY = (ground_script.slope) * ((px + (player_xBounds * 2.5f * xBoundDir)) - gx);
                    if (PM.currentSlope == 0)
                    {
                        Debug.Log(5.1);
                        player.transform.SetPositionAndRotation(new Vector3((px) + (player_xBounds * 2.5f * xBoundDir), slopeY + gy + player_yBounds + mF_yBounds + player_sinkY / 3, 0), other.transform.rotation);
                    }
                    else if (PM.currentSlope != 0 && xBoundDir == 0)
                    {
                        Debug.Log(5.2);
                        if (PM.currentSlope > 0 && px < ground_script.rightSide.x - pointedFallOff)
                        {
                            Debug.Log(5.23);
                            player.transform.SetPositionAndRotation(new Vector3((px), py, 0), other.transform.rotation);
                        }
                        else if (PM.currentSlope < 0 && px > ground_script.leftSide.x + pointedFallOff)
                        {
                            Debug.Log(5.24);
                            player.transform.SetPositionAndRotation(new Vector3((px), py, 0), other.transform.rotation);
                        }
                        else
                        {
                            Debug.Log(5.25);
                            player.transform.SetPositionAndRotation(new Vector3((px), py, 0), player.transform.rotation);
                        }
                        
                    }
                    else
                    {
                        Debug.Log(5.2);
                        player.transform.SetPositionAndRotation(new Vector3((px), py, 0), player.transform.rotation);
                        /*if (!PM.isSlope)
                        {
                            player.transform.SetPositionAndRotation(new Vector3((px), py, 0), player.transform.rotation);
                        }
                        else
                        {
                            player.transform.SetPositionAndRotation(new Vector3((px), py, 0), other.transform.rotation);
                        }*/
                    }
                    //PM.previousSlope = ground_script.slope;
                    PM.currentSlope = ground_script.slope;

                }
                PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, true, PM.isRoof, PM.friction_current);
                ground_script.top = true;

            }
            else if (!ground_script.roof) //Ground is not flat and is not a roof
            {
                Debug.Log(2);
                if (player.transform.position.y > gy + slopeY) // If the player is above the slope
                {
                }
                else // Player is below slope?
                {
                    PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, PM.isFloor, true, PM.friction_current);
                    ground_script.roof = true;
                    if (!PM.isFloor)
                    {
                        player.transform.SetPositionAndRotation(new Vector3(px, slopeY + gy - player_yBounds - mF_yBounds - (player_sinkY * 1.5f), 0), other.transform.rotation);
                        PM.isSlope = true;
                    }
                    else
                    {
                        if (ground_script.slope < 0)
                        {
                            PM.CollisionDetected(PM.isWallLeft, true, PM.isFloor, PM.isRoof, PM.friction_current);
                            ground_script.right = true;
                        }
                        else
                        {
                            PM.CollisionDetected(true, PM.isWallRight, PM.isFloor, PM.isRoof, PM.friction_current);
                            ground_script.left = true;
                        }
                    }
                }

                PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, true, PM.isRoof, PM.friction_current);
                ground_script.top = true;
            }
            
        }
        else if (ground_script.roof)
        {
            
            player.transform.SetPositionAndRotation(new Vector3(player.transform.position.x, ground_script.trans.position.y - mF_yBounds - pY_bounds + player_sinkY / 4, 0), new Quaternion(0, 0, other.transform.rotation.z, other.transform.rotation.w));
        }
    }

    private float findDirection(float px)
    {
        float xBoundDir = 0;
        //Debug.Log(PM.transform.rotation.z);
        Debug.Log("Previous Slope: " + PM.previousSlope);
        Debug.Log("Current Slope: " + PM.currentSlope);
        if (ground_script.slope > 0)
        {
            if (px < ground_script.rightSide.x && PM.isFloor && PM.isSlope && PM.currentSlope == 0)
            {
                Debug.Log(1.20);
                xBoundDir = -1;//Player is on left side now
            }
            else if (PM.isFloor && PM.currentSlope == 0)
            {
                Debug.Log(1.21);
                xBoundDir = 1;
            }
            else if (PM.currentSlope != 0)
            {
                Debug.Log(1.22);
                //xBoundDir = 1;
                //Debug.Log("DIR: " + xBoundDir);
            }
        }
        else if (ground_script.slope < 0)
        {
            if (px > ground_script.leftSide.x && PM.isFloor && PM.isSlope && PM.currentSlope == 0)
            {
                Debug.Log(1.30);
                xBoundDir = 1;
            }
            else if (PM.isFloor && PM.currentSlope == 0)
            {
                Debug.Log(1.31);
                xBoundDir = -1;
            }
            else if (PM.currentSlope == 0)
            {
                Debug.Log(1.32);
                //xBoundDir = -1;
                //Debug.Log("DIR: " + xBoundDir);
            }
        }
        else
        {
            if (px > ground_trans.position.x)
            {
                xBoundDir = 1;
            }
            else
            {
                xBoundDir = -1;
            }
            Debug.Log(1.1);
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
            Debug.Log("Collide with Non Anlge" + groundObject.name);
            //Left Wall Detection
            if (body_trans.position.x - player_xBounds - (player_sinkX) <= ground_trans.position.x + mF_xBounds && body_trans.position.x - player_xBounds - (player_sinkX) >= ground_trans.position.x + mF_xBounds - (player_sinkX * 2))
            {
                if (body_trans.position.y - player_yBounds - player_sinkY < ground_trans.position.y + mF_yBounds && body_trans.position.y - player_yBounds < ground_trans.position.y + mF_yBounds - (player_sinkY * 2))
                {
                    PM.CollisionDetected(true, PM.isWallRight, PM.isFloor, PM.isRoof, PM.friction_current);
                    ground_script.left = true;
                    Debug.Log("Left Enter");
                    if (player.transform.eulerAngles.z != 0)
                    {
                        //Debug.Log("Slope!");
                        PM.isSlope = true;
                    }

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
                    if (player.transform.eulerAngles.z != 0)
                    {
                        PM.isSlope = true;
                    }
                }
            }

            //Roof
            if (body_trans.position.y + player_yBounds > ground_trans.position.y - mF_yBounds && body_trans.position.y + player_yBounds < ground_trans.position.y - mF_yBounds + (player_sinkY * 2))
            {
                PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, PM.isFloor, true, PM.friction_current);
                ground_script.roof = true;
                Debug.Log("Roof");
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
                Debug.Log("Top Enter");
                if (player.transform.eulerAngles.z == 0 || contacts == 0)
                {
                    Debug.Log("NO SLOPE");
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
                Debug.Log("Top Enter 2");
                if (player.transform.eulerAngles.z == 0)
                {
                    Debug.Log("NO SLOPE 2");
                    PM.isSlope = false;
                }
            }

        }
        else
        {

        }

        
    }

    private void isSlope()
    {
        if (player.transform.eulerAngles.z == 0)
        {
            //Debug.Log("NO SLOPE 3");
            PM.isSlope = false;
        }
        else
        {
            //Debug.Log("Yes SLOPE 3");
            PM.currentSlope = ground_script.slope;
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
            PM.currentGroundScript = ground_script;
            friciton = ground_script.friction;
            mF = other.GetComponent<MeshFilter>();
            bmF = body.GetComponent<MeshFilter>();
            float gY = other.GetComponent<MeshFilter>().mesh.bounds.extents.y;
            float pY = body.GetComponent<MeshFilter>().mesh.bounds.extents.y;


            

            isSlope();
            CheckCollisions(other.gameObject, body);
            SetPlayerY(other, ground_trans.position.y + mF_yBounds, player_yBounds, mF_yBounds);
            contacts++;
            SetPlayandCamRotandPos();
            isSlope();

        } 
        else if (other.transform.tag == "checkpoint")
        {
            Checkpoint = new Vector3 (other.transform.position.x, other.transform.position.y, player.transform.position.y);
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

            if (player.transform.eulerAngles.z == 0 || contacts == 0) //RECENT CHANGE
            {
                PM.currentSlope = 0;
                PM.isSlope = false;
                PM.isRoof = false;
                ground_script.roof = false;
                if (contacts == 0)
                {
                    //Debug.Log(0);
                    PM.isFloor = false;
                    PM.isWallLeft = false;
                    PM.isWallRight = false;
                    
                    ground_script.left = false;
                    ground_script.right = false;
                    ground_script.top = false;
                }
            }

            //Off the ground
            if (contacts <= 0 || body_trans.position.y - player_yBounds > ground_trans.position.y + mF_yBounds && body_trans.position.y - player_yBounds > ground_trans.position.y + mF_yBounds - (player_sinkY * 2))
            {
                if (!PM.isSlope)
                {
                    PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, false, PM.isRoof, PM.friction_air);
                    ground_script.top = false;
                    //Debug.Log("Top Exit");
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

    }

}
