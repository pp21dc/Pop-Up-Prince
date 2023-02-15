using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceMovement : MonoBehaviour
{
    // if the player has a key or not
    [Header("Collectables")]
    public static bool hasKey = false;
    [HideInInspector]
    public float flowerCount = 0;
    public GameObject key;
    public GameObject flower1;
    public GameObject flower2;
    public GameObject flower3;

    [Header("Jump Settings")]
    [Tooltip("Time Before the Game Forgets the Player Pressed Space. 0.5 would mean half a second | Default: 0.1f")]
    public float ForgetJump = 0.1f;
    [Tooltip("Default: 9f")]
    public float JUMP_SPEED = 9f;
    [Tooltip("Time it takes to reach the players max jump speed (in seconds) [NOT IN USE]")]
    public float JUMP_ACCEL_TIME = 0.8f;
    [Tooltip("Divides the players speed by this much after releasing the jump button")]
    public float JUMP_CANCEL = 4f;

    [Header("Movement Settings")]
    [Tooltip("Starting Friction, and the variable the holds the current friction of the prince")]
    public float friction_current = 48;
    public float friction_air = 24;
    public float MOVE_SPEED = 12f;
    [Tooltip("Time it takes to reach the players max speed (in seconds)")]
    public float MOVE_ACCEL_TIME = 1.5f;
    [Tooltip("Starting Acceleration speed")]
    public float MOVE_ACCEL_X = 0.1f;
    public float MOVE_ACCEL_ACOEF;
    public float ROTATE_TIME = 0.5f;
    public float ROTATE_SPEED = 0.5f;
    public bool CAN_SLIDE = false;
    public float SLIDE_SPEED = 2.2f;

    [Header("Dash Settings")]
    public float DASH_SPEED = 20f;
    [Tooltip("Time in seconds for the length of the players dash")]
    public float DASH_LENGTH = 2f;
    [Tooltip("Time in seconds for the players dash to recover")]
    public float DASH_DELAY = 0f;
    [Tooltip("Helps the player dash upwards")]
    public float DASH_UPWARDRESISTANCE = 1.75f;

    [Header("PlayerPieces")]
    public PrinceFootCollider PFC;

    [HideInInspector]
    public float MASS = 75f;
    [HideInInspector]
    public float GRAVITY = 9.8f;
    [HideInInspector]
    public float CS_AREA = 0.18f;
    [HideInInspector]
    public float DRAG = 0.7f;
    [HideInInspector]
    public float DENSITY = 1.2f;

    //public Transform pR; //this is the players current rotation
    public GameObject cam;


    float speed_x = 0f;
    float speed_y = 0f;
    float speed_dashx = 0f;
    float speed_dashy = 0f;

    

    float FPS;
    float frame = 0f;
    float fMove_Counter = 0f;

    float prevDir = 0f;


    float terminal_vel; // this is the max speed the player can fall at
    Vector3 current_speed;// current speed of the player
    bool isCollidingGround = false;

    [HideInInspector]
    public bool isWallLeft = false;
    [HideInInspector]
    public bool isWallRight = false;
    [HideInInspector]
    public bool isFloor = false;    
    [HideInInspector]
    public bool isRoof = false;
    [HideInInspector]
    public bool isSlope = false;
    [HideInInspector]
    public float previousSlope = 0.0f;
    [HideInInspector]
    public float currentSlope = 0.0f;
    [HideInInspector]
    public Ground previousGroundScript;
    [HideInInspector]
    public Ground currentGroundScript;
    
    

    //bool jump_start = false;
    bool jumpQueued = false;
    float fJump_Counter = 0f; //forget the jump

    bool jump_held = false;
    bool jump_press;
    float jump_held_counter = 0f;
    float jump_ForcePressed = 0f; // the force at which the player hits the jump button

    [HideInInspector]
    public bool dashing = false;
    bool dash_delay_start = false;
    bool dash_ready = true;
    float dash_counter = 0f;
    float dash_delayCounter = 0f;

    [HideInInspector]
    public Vector3 Checkpoint;
    //bool respawn = false;


    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        FPS = Application.targetFrameRate;
        terminal_vel = -Mathf.Sqrt((2*MASS*GRAVITY)/(DENSITY*CS_AREA*DRAG));
        current_speed = new Vector3(0, -GRAVITY, 0);

        //MOVE_ACCEL_ACOEF = MOVE_SPEED / Mathf.Log10(MOVE_ACCEL_TIME); //Creates quadratic function to fufil time/speed acceleration requirements
    }

    // Update is called once per frame
    void Update()
    {
        //Checks
        friction();
        CheckGroundCollisions(); //Manages player movement
        

        //Movement
        HorizontalMovement(Input.GetAxisRaw("Horizontal"));
        VerticalMovement(Input.GetAxisRaw("Vertical"));
        DashMovement(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Dash"));


        //Timers
        timers();

        objectController();

    }

    private void objectController()
    {
        if (hasKey == true)
        {
            showObj(key);
        }
        else
        {
            hideObj(key);
        }

        if (flowerCount == 1)
        {
            showObj(flower1);
            hideObj(flower2);
            hideObj(flower3);
        }
        else if (flowerCount == 2)
        {
            showObj(flower1);
            showObj(flower2);
            hideObj(flower3);
        }
        else if (flowerCount == 3)
        {
            showObj(flower1);
            showObj(flower2);
            showObj(flower3);
        }
        else if (flowerCount == 0)
        {
            hideObj(flower1);
            hideObj(flower2);
            hideObj(flower3);
        }
    }

    private void showObj(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void hideObj(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void friction()
    {
        if (current_speed.x > 0)//FRICTION
        {
            current_speed -= new Vector3(friction_current / FPS, 0, 0);
            if (current_speed.x <= 0)
            {
                current_speed = new Vector3(0, current_speed.y, 0);
            }
        }
        else if (current_speed.x < 0)
        {
            current_speed += new Vector3(friction_current / FPS, 0, 0);
            if (current_speed.x >= 0)
            {
                current_speed = new Vector3(0, current_speed.y, 0);
            }
        }
    }

    private void timers()
    {
        //DELAY UNTIL NEXT DASH
        if (dash_delay_start && dash_delayCounter < DASH_DELAY)
        {
            dash_delayCounter += 1f / FPS;
        }
        else if (dash_delayCounter >= DASH_DELAY)
        {
            dash_delayCounter = 0f;
            dash_ready = true;
        }

        //LENGTH OF DASH
        if (dashing && dash_counter < DASH_LENGTH)
        {
            dash_counter += 1f / FPS;
        }
        else if (dash_counter >= DASH_LENGTH)
        {
            dash_counter = 0f;
            dashing = false;
            dash_delay_start = true;
            speed_dashx = 0f;
            speed_dashy = 0f;
        }

        //HOLDING THE JUMP BUTTON COUNTER
        if (jump_held)
        {
            jump_held_counter += 1f / FPS;
            jump_ForcePressed = jump_held_counter;
        }
        else
        {
            jump_held_counter = 0;
        }

        if (jumpQueued && fJump_Counter < ForgetJump) //forget jump
        {
            fJump_Counter += 1f / FPS;
        }
        else
        {
            fJump_Counter = 0f;
            jumpQueued = false;
        }

        if (frame < 1f)
        {
            frame += 1f / FPS; //Counts each frame until enough are counted that take up 1 second
            //Debug.Log(current_speed.x / (FPS * MOVE_ACCEL_TIME) + " | ");

        }
        else //Executes every Second
        {
            frame = 0.0f;

            //Debug.Log(current_speed + " | " + terminal_vel);
        }
    }

    private void CheckGroundCollisions()
    {
        if (!isFloor)
        {

            if (gameObject.transform.eulerAngles.z > 0 && gameObject.transform.eulerAngles.z < 180)
            {
                gameObject.transform.Rotate(new Vector3(0, 0, (-ROTATE_SPEED) * Time.deltaTime * ROTATE_TIME));
            } else if (gameObject.transform.eulerAngles.z >= 180)
            {
                gameObject.transform.Rotate(new Vector3(0, 0, ROTATE_SPEED * Time.deltaTime * ROTATE_TIME));
            }
            Quaternion pRotation = gameObject.transform.rotation;
            pRotation.eulerAngles = new Vector3(15, 0, 0);
            cam.transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y + cam.GetComponent<FollowPlayer>().height, cam.transform.position.z), pRotation);

            if (isRoof)
            {
                //Debug.Log("FALL: " + current_speed.y);
                transform.position += new Vector3((current_speed.x + speed_x + speed_dashx) * Time.deltaTime, current_speed.y * Time.deltaTime, 0); //MOVES THE PLAYER TO EQUATE TO 1 SECOND 
            } 
            else
            {
                //Debug.Log((current_speed.x + " " + speed_x + " " + speed_dashx) + " " + isWallRight);
                transform.position += new Vector3((current_speed.x + speed_x + speed_dashx) * Time.deltaTime, (current_speed.y + speed_y + speed_dashy) * Time.deltaTime, 0); //MOVES THE PLAYER TO EQUATE TO 1 SECOND 
            }
            
            if (current_speed.y > terminal_vel && (currentGroundScript == null || !currentGroundScript.top))
            {
                if (!isFloor)
                {
                    current_speed -= new Vector3(0, GRAVITY * Time.deltaTime, 0); //INCREASES GRAVITY IN SMALL SECTIONS TO EQUATE TO 1 SECOND [FALLING]
                }
                
            }


        }
        else //player is on the ground
        {
            float totalXMove = 0;
            jump_press = false;

            if (PFC.current_slope > 0 && CAN_SLIDE)
            {
                float RadAngle = (90 - transform.eulerAngles.z) * Mathf.PI / 180;
                float slide = 0;

                slide = SLIDE_SPEED / Mathf.Cos(RadAngle);
                totalXMove = (current_speed.x + speed_x + speed_dashx) - slide;
            }
            else if (PFC.current_slope < 0 && CAN_SLIDE) 
            {
                float RadAngle = Mathf.Abs((270f - transform.eulerAngles.z)) * Mathf.PI / 180;
                float slide = 0;

                slide = SLIDE_SPEED / Mathf.Cos(RadAngle);
                totalXMove = (current_speed.x + speed_x + speed_dashx) + slide;
            } 
            else
            {
                totalXMove = (current_speed.x + speed_x + speed_dashx);
            }

            if (isFloor && !isRoof)
            {
                current_speed = new Vector3(current_speed.x, 0, 0); //Stop player from falling once they hit the ground
            }
            else if (isRoof)
            {
                Debug.Log("ROOF");
                current_speed = new Vector3(current_speed.x, current_speed.y, 0);
            }
            speed_y = 0;
            if (jumpQueued) //IF JUMP QUEUED AND ON GROUND, THEN JUMP
            {
                jumpQueued = false;

                float slopeY = (currentGroundScript.slope) * ((transform.position.x) - currentGroundScript.trans.position.x) + (currentGroundScript.verticalWidthAP);

                isFloor = false;
                
                
                speed_y = JUMP_SPEED;
                if (current_speed.y > JUMP_SPEED)
                {
                    speed_y = JUMP_SPEED;
                }

            }

            transform.position += transform.TransformDirection((totalXMove) / FPS, 0,0);
            
        }
    }

    private void DashMovement(float LR, float UD, float dash)
    {
        if (dash > 0 && !dashing && dash_ready && flowerCount >= 1)
        {
            float diag = DASH_SPEED / 2;
            dashing = true;
            dash_ready = false;
            flowerCount--;
            if (UD > 0 && !isRoof)
            {
                speed_dashy = DASH_SPEED;
            }
            else if (UD < 0)
            {
                speed_dashy = -DASH_SPEED;
            }
            if (LR > 0 && !isWallRight)
            {
                speed_dashx = DASH_SPEED;
            }
            else if (LR < 0 && !isWallLeft)
            {
                speed_dashx = -DASH_SPEED;
            }

            if (UD > 0 && LR > 0 && !isWallRight && !isRoof)
            {
                //Debug.Log("TopRight");
                speed_dashy = diag;
                speed_dashx = diag;
            } 
            else if (UD > 0 && LR > 0 && isWallRight && !isRoof)
            {
                speed_dashy = diag;
            }
            
            
            if (UD > 0 && LR < 0 && !isWallLeft && !isRoof)
            {
                //Debug.Log("TopLeft");
                speed_dashy = diag;
                speed_dashx = -diag;
            }
            else if (UD > 0 && LR < 0 && isWallLeft && !isRoof)
            {
                speed_dashy = diag;
            }

            if (UD < 0 && LR < 0 && !isWallLeft && !isRoof && !isSlope)
            {
                //Debug.Log("BottomLeft");
                speed_dashy = -diag;
                speed_dashx = -diag;
            }
            else if (UD < 0 && LR < 0 && isWallLeft && !isRoof && !isSlope) 
            {
                speed_dashy = -diag;
            }
            
            if (UD < 0 && LR > 0 && !isWallRight && !isRoof && !isSlope)
            {
                //Debug.Log("BottomRight");
                speed_dashy = -diag;
                speed_dashx = diag;
            } 
            else if (UD < 0 && LR > 0 && isWallRight && !isRoof && !isSlope) 
            {
                speed_dashy = -diag;
            }
            current_speed = new Vector3(0, 0, 0);
            speed_x = 0;

            if (isRoof)
            {
                speed_dashy = 0;
                speed_y = 0;
            }
            
        }
    }

    private void VerticalMovement(float dir)
    {
        if (!dashing)
        {
            if (dir > 0 && !jump_held && !isRoof)
            {
                jump_held = true;
                jumpQueued = true;
            }
            if (dir == 0 && jump_held)
            {
                jump_held = false;
                speed_y /= JUMP_CANCEL;
            }
            if (dir < 0)
            {
                speed_y = 0;
            }
        }
       
    }

    private void HorizontalMovement(float dir)
    {
        if (!dashing)
        {
            speed_x = 0;
            if (dir > 0 && !isWallRight)
            {
                if (prevDir <= 0)
                {
                    fMove_Counter = 0;
                }

                fMove_Counter += 1f / (FPS * MOVE_ACCEL_TIME);
                MOVE_ACCEL_ACOEF = Mathf.Pow(MOVE_SPEED, (1f / MOVE_ACCEL_TIME));
                current_speed = new Vector3(Mathf.Pow(MOVE_ACCEL_ACOEF, fMove_Counter) + MOVE_ACCEL_X, current_speed.y, 0);

                if (current_speed.x >= MOVE_SPEED)
                {
                    current_speed = new Vector3(MOVE_SPEED, current_speed.y, 0);
                    MOVE_ACCEL_ACOEF = 0f;
                }


            }
            else if (dir < 0 && !isWallLeft)
            {
                if (prevDir >= 0)
                {
                    fMove_Counter = 0;
                }

                fMove_Counter += 1f / (FPS * MOVE_ACCEL_TIME);
                MOVE_ACCEL_ACOEF = Mathf.Pow(MOVE_SPEED, (1f / MOVE_ACCEL_TIME));
                current_speed = new Vector3(-(Mathf.Pow(MOVE_ACCEL_ACOEF, fMove_Counter) + MOVE_ACCEL_X), current_speed.y, 0);

                if (current_speed.x <= -MOVE_SPEED)
                {
                    current_speed = new Vector3(-MOVE_SPEED, current_speed.y, 0);
                    MOVE_ACCEL_ACOEF = 0f;
                }

            }
            else
            {
                //current_speed = new Vector3(current_speed.x, current_speed.y, 0);
                //fMove_Counter = 0f;
            }
            prevDir = dir;
        }
    }


    public void CollisionDetected(bool left, bool right, bool floor, bool roof, float friction)
    {
        if (left || right)
        {
            speed_dashx = 0;
            speed_x = 0;
            current_speed = new Vector3(0, current_speed.y, 0);
        }
        
        
        isWallLeft = left;
        isWallRight = right;
        isCollidingGround = floor;
        isFloor = floor;
        isRoof = roof;
        friction_current = friction;

        if (isRoof || isSlope)
        {
            speed_dashy = 0;
            speed_y = 0;
            current_speed = new Vector3(0, -2.45f, 0);
        }
        //Debug.Log("Collide");

    }

    public void Respawn()
    {
        Debug.Log("Respawn");
        speed_dashx = 0;
        speed_dashy = 0;
        speed_x = 0;
        speed_y = 0;
        current_speed = (Vector3.zero);
        transform.position = Checkpoint;
    }

}
