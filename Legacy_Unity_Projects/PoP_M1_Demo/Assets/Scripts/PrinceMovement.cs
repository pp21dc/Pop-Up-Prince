using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceMovement : MonoBehaviour
{

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

    [Header("Dash Settings")]
    public float DASH_SPEED = 20f;
    [Tooltip("Time in seconds for the length of the players dash")]
    public float DASH_LENGTH = 2f;
    [Tooltip("Time in seconds for the players dash to recover")]
    public float DASH_DELAY = 5f;
    [Tooltip("Helps the player dash upwards")]
    public float DASH_UPWARDRESISTANCE = 1.75f;

    [Header("Prince Attributes [DO NOT ADJUST]")]
    public float MASS = 75f;
    public float GRAVITY = 9.8f;
    public float CS_AREA = 0.18f;
    public float DRAG = 0.7f;
    public float DENSITY = 1.2f;

    public Transform pR; //this is the players current rotation


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
    string isCollidingWall;

    bool jump_start = false;
    bool jumpQueued = false;
    float fJump_Counter = 0f; //forget the jump

    bool jump_held = false;
    float jump_held_counter = 0f;
    float jump_ForcePressed = 0f; // the force at which the player hits the jump button

    bool dashing = false;
    bool dash_delay_start = false;
    bool dash_ready = true;
    float dash_counter = 0f;
    float dash_delayCounter = 0f;


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
        CheckGroundCollisions();

        //Movement
        HorizontalMovement(Input.GetAxisRaw("Horizontal"));
        VerticalMovement(Input.GetAxisRaw("Vertical"));
        DashMovement(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Dash"));


        //Timers
        timers();

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
        if (!isCollidingGround)
        {
            //pR.SetPositionAndRotation(pR.position, new Quaternion(0, 0, 0, 1));
            if (pR.eulerAngles.z > 0 && pR.eulerAngles.z < 180)
            {
                pR.Rotate(new Vector3(0, 0, -1f / FPS*ROTATE_TIME));
            } else if (pR.eulerAngles.z >= 180)
            {
                pR.Rotate(new Vector3(0, 0, 1f / FPS * ROTATE_TIME));
            }
            
            
            transform.position += new Vector3((current_speed.x + speed_x + speed_dashx) / FPS, (current_speed.y + speed_y + speed_dashy) / FPS, 0); //MOVES THE PLAYER TO EQUATE TO 1 SECOND 
            if (current_speed.y > terminal_vel)
            {
                current_speed -= new Vector3(0, GRAVITY / FPS, 0); //INCREASES GRAVITY IN SMALL SECTIONS TO EQUATE TO 1 SECOND [FALLING]
            }


        }
        else //player is on the ground
        {
            float totalXMove = (current_speed.x + speed_x + speed_dashx);
            float totalYMove = 0;
            float angledXMove;
            float angledYMove;
            if (pR.eulerAngles.z > 90)
            {
                //Debug.Log(Mathf.Abs(pR.eulerAngles.z - 360f));
                angledXMove = totalXMove * Mathf.Cos(Mathf.Abs(pR.eulerAngles.z - 360f));
                angledYMove = totalXMove * Mathf.Sin(Mathf.Abs(pR.eulerAngles.z - 360f));
            }
            else
            {
                angledXMove = totalXMove * Mathf.Cos(Mathf.Abs(pR.eulerAngles.z));
                angledYMove = totalXMove * Mathf.Sin(Mathf.Abs(pR.eulerAngles.z));
            }
            

            //Debug.Log(angledXMove + " | " + angledYMove);
            transform.position += new Vector3(angledXMove / (FPS), angledYMove/FPS, 0); //MOVES THE PLAYER TO EQUATE TO 1 SECOND 
            

            current_speed = new Vector3(current_speed.x, 0, 0); //Stop player from falling once they hit the ground
            speed_y = 0;
            if (jumpQueued) //IF JUMP QUEUED AND ON GROUND, THEN JUMP
            {
                jumpQueued = false;
                isCollidingGround = false;
                speed_y = JUMP_SPEED;
                if (current_speed.y > JUMP_SPEED)
                {
                    speed_y = JUMP_SPEED;
                }

            }
        }
    }

    private void DashMovement(float LR, float UD, float dash)
    {
        if (dash > 0 && !dashing && dash_ready)
        {
            float diag = DASH_SPEED / 2;
            dashing = true;
            dash_ready = false;
            if (UD > 0)
            {
                speed_dashy = DASH_SPEED;
            }
            else if (UD < 0)
            {
                speed_dashy = -DASH_SPEED;
            }
            if (LR > 0)
            {
                speed_dashx = DASH_SPEED;
            }
            else if (LR < 0)
            {
                speed_dashx = -DASH_SPEED;
            }

            if (UD > 0 && LR > 0)
            {
                //Debug.Log("TopRight");
                speed_dashy = diag;
                speed_dashx = diag;
            } 
            else if (UD > 0 && LR < 0)
            {
                //Debug.Log("TopLeft");
                speed_dashy = diag;
                speed_dashx = -diag;
            }
            else if (UD < 0 && LR < 0)
            {
                //Debug.Log("BottomLeft");
                speed_dashy = -diag;
                speed_dashx = -diag;
            }
            else if (UD < 0 && LR > 0)
            {
                //Debug.Log("BottomRight");
                speed_dashy = -diag;
                speed_dashx = diag;
            }
            current_speed = new Vector3(0, 0, 0);
            
        }
    }

    private void VerticalMovement(float dir)
    {
        if (!dashing)
        {
            if (dir > 0 && !jump_held)
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
            if (dir > 0)
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
            else if (dir < 0)
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

    public void CollisionDetected(bool colliding, float friction)
    {
        isCollidingGround = colliding;
        friction_current = friction;
        
    }


}
