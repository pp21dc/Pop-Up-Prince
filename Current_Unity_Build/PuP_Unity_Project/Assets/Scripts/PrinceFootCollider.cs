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
    [Tooltip("How much player sinks into ground | Default: 0.95")]
    public float player_groundSinkY = 1.0f;
    [Tooltip("How much player sinks into walls | Default: 0.1")]
    public float player_groundSinkX_right = 0.1f;
    [Tooltip("How much player sinks into walls | Default: 0.3")]
    public float player_groundSinkX_left = 0.3f;
    [Tooltip("Used to determine if player is above or below slope")]
    public float player_groundSinkY_slope = 0.1f;
    public float player_groundSinkY_slopeTop = 0.3f;
    [Tooltip("How much the player can walk on the point of a slope before being affected by the next slope")]
    public float pointedFallOff = 0.5f;
    public float slope_tpmultiplyer = 1.5f;
    public float slope_edgeDetector = 1.0f;
    [HideInInspector]
    public int contacts = -1;
    
    public PrinceMovement PM;
    Ground ground_script;
    MeshFilter mF;
    MeshFilter bmF;
    Quaternion camRotation;

    public GameObject body;
    public GameObject player;
    public Collider player_collider;
    public GameObject cam;
    public GameObject BL;
    public GameObject BR;
    public GameObject TL;
    public GameObject TR;

    float mF_xBounds;
    float mF_yBounds;
    float player_xBounds;
    float player_yBounds;
    float c_yb;
    float c_xb;
    Transform body_trans;
    Transform ground_trans;
    Vector3 Checkpoint = new Vector3(0,0,0);

    public float current_slope;

    float dC = 0;
    float store_csposx;

    private void Start()
    {
        store_csposx = player_collider.transform.localPosition.x;
        c_yb = player_collider.bounds.extents.y;
        c_xb = player_collider.bounds.extents.x;
        Vector3 pc_pos = player_collider.transform.localPosition;
        Vector3 pc_posG = player_collider.transform.position;
        Vector3 pc_scl = player_collider.bounds.size;
        Vector3 pc_cnt = player_collider.bounds.center;
        camRotation = new Quaternion(0, 0, 0, 0);
        camRotation.eulerAngles.Set(15, 0, 0);
        
        //Debug.Log(c_yb);
        //BL.transform.position = new Vector3(pc_posG.x-c_xb, pc_posG.y - c_yb, 0);
        //BR.transform.position = new Vector3(pc_posG.x+c_xb, pc_posG.y - c_yb, 0);
        //TR.transform.localPosition = new Vector3(0, player_collider.bounds.max.y/10, 0);
        Debug.Log(c_xb);
    }

    private void Update()
    {
        transform.localPosition = new Vector3(0,0,0);
        /*if (ground_script.leftSide.y < BR.transform.position.y && ground_script.top == false)
        {
            PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, false, PM.isRoof, PM.friction_air);
            ground_script.top = false;
            Debug.Log("Top Exit | NO TOP");
        }*/
    }


    public void SetPlayerY(Collider other)
    {
        float px = player.transform.position.x;
        float py = player.transform.position.y; //Changed, removed bound minus
        c_yb = player_collider.bounds.extents.y;
        float gx = ground_script.transform.position.x;
        float gy = ground_script.transform.position.y;
        float slopeY = (ground_script.slope) * ((px) - gx) + gy;
        slope_tpmultiplyer = ground_script.snap_multiplyer;
        float clDif = BL.transform.localPosition.x;
        float crDif = BR.transform.localPosition.x;
        float cryDif = BR.transform.localPosition.y;

        if (ground_script.top && (!ground_script.left && !ground_script.right) && ground_script.slope == 0 && PM.previousSlope == 0 && !PM.isSlope) //hitting flat ground
        {
            Debug.Log(0);
            player.transform.SetPositionAndRotation(new Vector3(px, gy + mF_yBounds + c_yb - player_groundSinkY, 0), new Quaternion(0, 0, other.transform.rotation.z, other.transform.rotation.w));
            transform.SetPositionAndRotation(transform.position, player.transform.rotation);
        }
        else if (ground_script.left || ground_script.right && ground_script.slope == 0 && !PM.isSlope) // Hitting a wall and player is not currently on a slope
        {
            Debug.Log(1);
            //Debug.Log("Is Slope: " + PM.isSlope);
            if (ground_script.left && !PM.isSlope)
            {
                Debug.Log(1.1);
                player.transform.SetPositionAndRotation(new Vector3(ground_script.rightSide.x + Mathf.Abs(clDif) + player_groundSinkX_left, py, 0), new Quaternion(0, 0, other.transform.rotation.z, other.transform.rotation.w));
                transform.SetPositionAndRotation(transform.position, player.transform.rotation);
            }
            else if (ground_script.right && !PM.isSlope)
            {
                Debug.Log(1.2);
                player.transform.SetPositionAndRotation(new Vector3(ground_script.leftSide.x - Mathf.Abs(crDif) - player_groundSinkX_right, py, 0), new Quaternion(0, 0, other.transform.rotation.z, other.transform.rotation.w));
                transform.SetPositionAndRotation(transform.position, player.transform.rotation);
            }
            
        }
        else if (ground_script.slope != 0 || (ground_script.slope == 0 && PM.isSlope) && (!ground_script.left && !ground_script.right)) // On a slope or leaving a slope to a flat surface
        {
            

            float xBoundDir = findDirection(px);
            slopeY = (ground_script.slope) * ((px + (player_xBounds * ground_script.snap_multiplyer * xBoundDir)) - gx) + ground_script.verticalWidthAP;
            float x = ((px + (player_xBounds * ground_script.snap_multiplyer * xBoundDir)));

            Vector3 S = Vector3.Lerp(ground_script.leftSide, ground_script.rightSide, x / Vector3.Distance(ground_script.leftSide, ground_script.rightSide));
            //Debug.DrawLine(ground_script.leftSide, ground_script.rightSide);

            //Debug.Log(ground_script.slope);
            if (py + player_groundSinkY_slope >= gy + slopeY)
            {
                if (ground_script.slope != 0 && PM.currentSlope == 0)
                {
                    Debug.Log(2.1);
                    player.transform.SetPositionAndRotation(new Vector3((px) + (player_xBounds * ground_script.snap_multiplyer * xBoundDir), gy + (slopeY) - player_groundSinkY_slopeTop, 0 ), other.transform.rotation);
                    transform.SetPositionAndRotation(transform.position, player.transform.rotation);
                }
                else if (PM.isSlope && xBoundDir == 0 && ground_script.slope != 0)
                {
                    Debug.Log(2.2);
                    player.transform.SetPositionAndRotation(new Vector3((px) + (player_xBounds * ground_script.snap_multiplyer * xBoundDir), gy + (slopeY) - player_groundSinkY_slopeTop, 0), other.transform.rotation);
                    transform.SetPositionAndRotation(transform.position, player.transform.rotation);
                }
                else if (ground_script.slope == 0)
                {
                    Debug.Log(2.3);
                    player.transform.SetPositionAndRotation(new Vector3((px) + (player_xBounds * ground_script.snap_multiplyer * xBoundDir), ground_script.leftSide.y, 0), other.transform.rotation);
                    transform.SetPositionAndRotation(transform.position, player.transform.rotation);
                }
                PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, true, PM.isRoof, PM.friction_current);
                ground_script.top = true;
            }

            else if (ground_script.slope == 0 && TR.transform.position.y > ground_script.bottom.y)
            {
                Debug.Log(3.3);
                PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, PM.isFloor, true, PM.friction_current);
                ground_script.roof = true;
                
            }

            else if (px > gx && ground_script.slope != 0 && py < gy + slopeY)
            {
                Debug.Log(3.1);
                PM.CollisionDetected(true, PM.isWallRight, PM.isFloor, PM.isRoof, PM.friction_current);
                ground_script.left = true;
            }
            else if (px < gx && ground_script.slope != 0 && py < gy + slopeY)
            {
                Debug.Log(3.2);
                PM.CollisionDetected(PM.isWallLeft, true, PM.isFloor, PM.isRoof, PM.friction_current);
                ground_script.right = true;
            }


        }

        /*if (ground_script.slope == 0 && TR.transform.position.y >= ground_script.bottom.y && !ground_script.top)
        {
            PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, PM.isFloor, true, PM.friction_current);
            ground_script.roof = true;
        
        }*/

    }

    private void SetPlayerX()
    {
        if (PM.isWallRight)
        {
            player.transform.SetPositionAndRotation(new Vector3(ground_script.leftSide.x - BR.transform.localPosition.x,player.transform.position.y,player.transform.position.z), player.transform.rotation);
        }
        else if (PM.isWallLeft)
        {
            player.transform.SetPositionAndRotation(new Vector3(ground_script.rightSide.x + BR.transform.localPosition.x, player.transform.position.y, player.transform.position.z), player.transform.rotation);
        }
    }

    private float findDirection(float px)
    {
        float xBoundDir = 0;
        //Debug.Log(PM.currentGroundScript.gameObject.name);
        if (PM.currentGroundScript != null && PM.previousGroundScript != null)
        {
            //Debug.Log(PM.currentGroundScript.slope);
            //Debug.Log(PM.currentSlope);


            if (PM.currentGroundScript.slope > 0 && PM.currentSlope == 0)
            {
                if ((BR.transform.position.x < ground_script.rightSide.x && px > ground_trans.position.x) && contacts != 0)
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
            else if (PM.currentGroundScript.slope < 0 && PM.currentSlope == 0)
            {
                if ((BL.transform.position.x > ground_script.leftSide.x && px < ground_trans.position.x) && contacts != 0)
                {
                    Debug.Log(19.20);
                    xBoundDir = 1;//Player is on left side now
                }
                else if (contacts != 0 && BR.transform.position.x < ground_script.rightSide.x)
                {
                    Debug.Log(19.21);
                    xBoundDir = 1;
                }
                else
                {
                    Debug.Log(19.22);
                    xBoundDir = 0;
                }
            }
            else if (PM.currentGroundScript.slope == 0 && PM.currentSlope == 0)
            {
                if (PM.previousGroundScript.slope > 0)
                {
                    if (BL.transform.position.x - slope_edgeDetector < PM.previousGroundScript.transform.position.x)
                    {
                        Debug.Log(20.1);
                        xBoundDir = -1;
                    }
                    else
                    {
                        Debug.Log(20.11);
                        xBoundDir = 0;
                    }
                }
                else if (PM.previousGroundScript.slope < 0)
                {
                    if (BR.transform.position.x + slope_edgeDetector > PM.previousGroundScript.transform.position.x)
                    {
                        Debug.Log(20.2);
                        xBoundDir = 1;
                    }
                    else
                    {
                        Debug.Log(20.21);
                        xBoundDir = 0;
                    }
                }
                else
                {
                    Debug.Log("SHLOPEL: " + PM.previousGroundScript.slope);
                }
            }

            else
            {

                if (contacts == 0)
                {
                    Debug.Log(8.1);
                    xBoundDir = 0;
                }
                else if (PM.currentGroundScript.slope >= 0)
                {
                    if (px > PM.currentGroundScript.trans.position.x)
                    {
                        Debug.Log(8.2);
                        xBoundDir = -1;
                    }
                    else
                    {
                        Debug.Log(8.3);
                        xBoundDir = 0;
                    }
                }
                else if (PM.currentGroundScript.slope <= 0)
                {
                    if (px < PM.currentGroundScript.trans.position.x)
                    {
                        Debug.Log(8.4);
                        xBoundDir = 1;
                    }
                    else
                    {
                        Debug.Log(8.5);
                        xBoundDir = 0;
                    }
                }
                else
                {

                    Debug.Log(8.6);
                    xBoundDir = 0;
                }
                //Debug.Log(1.1);
            }
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
        pRotation.eulerAngles = new Vector3(script_followPlayer.rotation.x, script_followPlayer.rotation.y, script_followPlayer.rotation.z);
        cam.transform.SetPositionAndRotation(new Vector3(player.transform.position.x + script_followPlayer.lead, player.transform.position.y + script_followPlayer.height, player.transform.position.z + script_followPlayer.zoom + PM.heightFactor), pRotation);
    }

    public void CheckCollisions(GameObject groundObject, GameObject playerBody)
    {
        mF_xBounds = groundObject.GetComponent<Collider>().bounds.extents.x;
        mF_yBounds = groundObject.GetComponent<Collider>().bounds.extents.y;
        player_xBounds = BR.transform.localPosition.x;
        player_yBounds = BR.transform.localPosition.y;

        body_trans = playerBody.transform;
        ground_trans = groundObject.transform;



        if (groundObject.transform.eulerAngles.z == 0 )
        {
            //Debug.Log("Collide with Non Anlge" + groundObject.name);
            //Left Wall Detection
            if (BL.transform.position.x <= ground_script.rightSide.x + player_sinkX/3)
            {
                if (BR.transform.position.x > ground_script.rightSide.x)
                {
                    Debug.Log("LEFTPRE");
                    if (BR.transform.position.y < ground_script.rightSide.y - player_sinkY)
                    {
                        Debug.Log("LEFT");
                        if (BR.transform.position.y + player_sinkY/2 > ground_script.rightSide.y)
                        {
                            Debug.Log("LEFT SKIP");
                        }
                        else if (BL.transform.position.x - player_sinkX/2 < ground_script.rightSide.x)
                        {
                            PM.CollisionDetected(true, PM.isWallRight, PM.isFloor, PM.isRoof, PM.friction_current);
                            ground_script.left = true;
                            Debug.Log("Left Enter");
                            if (player.transform.eulerAngles.z != 0)
                            {
                                Debug.Log("Slope!");
                                PM.isSlope = true;
                            }

                        }
                    }
                }
            }

            //Right Wall Detection
            if (BR.transform.position.x >= ground_script.leftSide.x - player_sinkX / 3)
            {
                if (BL.transform.position.x < ground_script.leftSide.x)
                {
                    Debug.Log("RIGHTPRE");
                    if (BR.transform.position.y < ground_script.leftSide.y - player_sinkY && !ground_script.left)
                    {
                        Debug.Log("RIGHT");
                        if (BR.transform.position.y + player_sinkY/2 > ground_script.leftSide.y)
                        {
                            
                            Debug.Log("RIGHT SKIP/SLOPE: " + PM.isSlope);
                        }
                        else if (BR.transform.position.x + player_sinkX / 2 > ground_script.leftSide.x)
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
                }
            }

            Debug.Log("1: " + (TR.transform.position.y >= ground_script.bottom.y));
            Debug.Log("2: " + (BR.transform.position.y < ground_script.bottom.y));
            //Roof
            /*if (TR.transform.position.y >= ground_script.bottom.y && BR.transform.position.y < ground_script.bottom.y && !PM.isWallLeft && !PM.isWallRight)
            {
               
                PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, PM.isFloor, true, PM.friction_current);
                ground_script.roof = true;
                Debug.Log("Roof");
                if (player.transform.eulerAngles.z != 0)
                {
                    PM.isSlope = true;
                }
            }*/

            //Top Detection
            if (!ground_script.left && !ground_script.right && !ground_script.roof && TR.transform.position.y > ground_script.leftSide.y)
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
            if (BR.transform.position.y <= ground_script.rightSide.y && BR.transform.position.y >= ground_script.rightSide.y - (player_sinkY * 2))
            {
                if (!ground_script.left && !ground_script.right && !ground_script.roof)
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

        }
        else
        {
           /* float slopeY = (ground_script.slope) * ((body_trans.position.x)) - ground_script.trans.position.x + (ground_script.verticalWidthAP);
            if (body_trans.position.y - player_yBounds > ground_script.trans.position.y + slopeY)
            {
                PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, true, PM.isRoof, friciton);
                ground_script.top = true;
                Debug.Log("Top Enter: Slope");
            }*/
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

    /**
     * Current Groundscript and direction (top or bottom) 1, -1
     */
    private float FindPositionOnSlope(Ground gs, int TB)
    { 
        float slopeY;
        if (gs.slope != 0)
        {
            slopeY = (gs.slope) * ((player.transform.position.x) - gs.transform.position.x) + (gs.verticalWidthAP * TB);
        }
        else
        {
            slopeY = gs.transform.position.y - gs.verticalWidthAP;
        }
        
        return (slopeY);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("CURRRENT CONTACTS: " + contacts);
       
        if (other.transform.tag == "ground")
        {

            PM = player.GetComponent<PrinceMovement>();

            ground_script = other.GetComponent<Ground>();
            if (ground_script != PM.currentGroundScript)
            {
                PM.previousGroundScript = PM.currentGroundScript;
                PM.currentGroundScript = ground_script;
            }

            friciton = ground_script.friction;
            //mF = other.GetComponent<MeshFilter>();
            bmF = body.GetComponent<MeshFilter>();
            //float gY = other.GetComponent<MeshFilter>().mesh.bounds.extents.y;
            float pY = body.GetComponent<MeshFilter>().mesh.bounds.extents.y;
            c_yb = player_collider.bounds.extents.y;

            if (!PM.grabbed)
            {
                isSlope();
                CheckCollisions(other.gameObject, body);
                SetPlayerY(other);
                PM.previousSlope = PM.currentSlope;
                PM.currentSlope = ground_script.slope;
                
                current_slope = ground_script.slope;
                PM.currentGroundScript = ground_script;
                contacts++;
                SetPlayandCamRotandPos();
                isSlope();
            }

            Debug.Log("===== ENTER END =====");

        } 
        else if (other.transform.tag == "checkpoint")
        {
            Checkpoint = new Vector3 (other.transform.position.x, other.transform.position.y, 0);
            PM.Checkpoint = Checkpoint;
            Debug.Log("Checkpoint Hit");
        }
        else if (other.transform.tag == "curve")
        {
            PM.onCurve = true;
            PM.BzF.routeToGo = other.GetComponent<curve>().Route;
            Debug.Log("CurveHit");
        }
        
    }


    private void OnTriggerExit(Collider other)
    {
        Debug.Log("==== EXIT START CONTACTS: " + contacts);
        if (other.transform.tag == "ground" && !PM.grabbed)
        {
            
            PM = player.GetComponent<PrinceMovement>();
            ground_script = other.GetComponent<Ground>();
            //mF = other.GetComponent<MeshFilter>();
            bmF = body.GetComponent<MeshFilter>();
            mF_xBounds = (other.transform.localScale.x / 2);
            mF_yBounds = (other.transform.localScale.y / 2);
            player_xBounds = (body.transform.localScale.x/2);
            player_yBounds = (body.transform.localScale.y/2);
            body_trans = body.transform;
            ground_trans = other.gameObject.transform;
            float slopeY = (ground_script.slope) * ((player.transform.position.x) - other.transform.position.x) + (ground_script.verticalWidthAP);
            if (!PM.grabbed)
            {
                
                contacts--;
                if (contacts < 0)
                {
                    contacts = 0;
                }
                Debug.Log("Contacts: " + contacts);
                Debug.Log(PM.isFloor);

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
                        PM.previousGroundScript = PM.currentGroundScript;
                        PM.currentGroundScript = null;
                        //PM.isFloor = false;
                    }
                }




                //Off the ground
                
                if (BR.transform.position.y >= ground_script.leftSide.y + 0.05f && ground_script.slope == 0 && PM.currentSlope == 0)
                {
                    Debug.Log("TopExit2 Start");
                    if (PM.currentGroundScript != null && PM.previousGroundScript != null)
                    {
                        Debug.Log("TopExit2 CheckTops");
                        //Debug.Log("PREVIOUS GS: " + PM.previousGroundScript.name);
                        if ((BR.transform.position.y > PM.previousGroundScript.leftSide.y)) { 
                            PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, false, PM.isRoof, PM.friction_air);
                            ground_script.top = false;
                            Debug.Log(other.name);
                            Debug.Log("Top Exit2");
                        }
                        else if (ground_script.leftSide.y < BR.transform.position.y)
                        {
                            PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, false, PM.isRoof, PM.friction_air);
                            ground_script.top = false;
                            Debug.Log(other.name);
                            Debug.Log("Top Exit | NO TOP");
                        }
                    }
                }
                



                //Nothing Above
                if (TR.transform.position.y < ground_trans.position.y - mF_yBounds && TR.transform.position.y < ground_trans.position.y - mF_yBounds + (player_sinkY * 2))
                {
                    PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, PM.isFloor, false, PM.friction_current);
                    ground_script.roof = false;
                    Debug.Log("No Roof");
                }
                
                if (TR.transform.position.y < other.transform.position.y - slopeY)
                {
                    
                    PM.CollisionDetected(PM.isWallLeft, PM.isWallRight, PM.isFloor, false, PM.friction_current);
                    ground_script.roof = false;
                    Debug.Log("No Roof2");
                }


                //Leaving Left Wall
                if (ground_script.left)
                {
                    Debug.Log("LeftExit Start");
                    if (ground_script.slope > 0)
                    {
                        PM.CollisionDetected(false, PM.isWallRight, PM.isFloor, PM.isRoof, PM.friction_current);
                        ground_script.left = false;
                        Debug.Log("Left Exit1");
                    }

                    if ((BR.transform.position.y > ground_script.rightSide.y))
                    {
                        PM.CollisionDetected(false, PM.isWallRight, PM.isFloor, PM.isRoof, PM.friction_air);
                        ground_script.left = false;
                        Debug.Log("Left Exit2");
                    }
                    else if (TR.transform.position.y < ground_trans.position.y - mF_yBounds)
                    {
                        PM.CollisionDetected(false, PM.isWallRight, PM.isFloor, PM.isRoof, PM.friction_air);
                        ground_script.left = false;
                        Debug.Log("Left Exit3");
                    }
                    else if (BL.transform.position.x > ground_script.rightSide.x)
                    {
                        PM.CollisionDetected(false, PM.isWallRight, PM.isFloor, PM.isRoof, PM.friction_air);
                        ground_script.left = false;
                        Debug.Log("Left Exit4");
                    }

                }

                //Leaving Right Wall
                if (ground_script.right)
                {
                    if (ground_script.slope < 0)
                    {
                        PM.CollisionDetected(PM.isWallLeft, false, PM.isFloor, PM.isRoof, PM.friction_current);
                        ground_script.right = false;
                        Debug.Log("Right Exit1");
                    }

                    if ((BR.transform.position.y > ground_script.leftSide.y))
                    {
                        PM.CollisionDetected(PM.isWallLeft, false, PM.isFloor, PM.isRoof, PM.friction_air);
                        ground_script.right = false;
                        Debug.Log("Right Exit2");
                    }
                    else if (TR.transform.position.y < ground_trans.position.y - mF_yBounds)
                    {
                        PM.CollisionDetected(PM.isWallLeft, false, PM.isFloor, PM.isRoof, PM.friction_air);
                        ground_script.right = false;
                        Debug.Log("Right Exit3");
                    }
                    else if (BR.transform.position.x < ground_script.leftSide.x)
                    {
                        PM.CollisionDetected(PM.isWallLeft, false, PM.isFloor, PM.isRoof, PM.friction_air);
                        ground_script.right = false;
                        Debug.Log("Right Exit4");
                    }
                }
            }
        }

        Debug.Log("===== EXIT END [ " + other.name + " CONTACTS: " + contacts + " ] =====");

    }

}
