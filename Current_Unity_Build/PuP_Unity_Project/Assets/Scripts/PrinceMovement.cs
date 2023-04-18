using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceMovement : MonoBehaviour
{
    // if the player has a key or not
    
    [Header("Collectables")]
    public static bool hasKey = false;
    public  bool hasKey2 = false;
    [HideInInspector]
    public float flowerCount = 0;
    public GameObject key;
    public GameObject flower1;
    public GameObject flower2;
    public GameObject flower3;
    public GameObject manager;
    public GameObject dashEffect;
    public GameManager GM;

    [Header("Jump Settings")]
    public bool stick_jumpControl = false;
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
    public BezierFollow BzF;
    public ParticleSystem PS;
    bool PS_lock = false;
    public GameObject ink_effect;
    public float ink_riseHeight;
    public float ink_riseSpeed;
    public Vector3 ink_startPos;
    bool ink_rise;
    bool ink_top;


    [Header("Audio/SFX")]
    public AudioSource AS;
    public AudioClip AS_walking;
    public AudioClip AS_jumping;
    public AudioClip AS_landing;
    public AudioClip AS_dash;
    public AudioClip AS_collect;
    

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
    public FollowPlayer FP;


    float speed_x = 0f;
    [HideInInspector]
    public float speed_y = 0f;
    [HideInInspector]
    public float speed_dashx = 0f;
    float speed_dashy = 0f;

    public float heightFactor;
    public float scrollFactor;

    float FPS;
    float frame = 0f;
    float fMove_Counter = 0f;

    float prevDir = 0f;


    float terminal_vel; // this is the max speed the player can fall at
    [HideInInspector]
    public Vector3 current_speed;// current speed of the player
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
    [HideInInspector]
    public bool grabbed = false;
    [HideInInspector]
    public bool onCurve = false;
    
    

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
    public bool respawn = false;
    float respawn_counter = 0f;
    float respawn_length = 5f;

    [HideInInspector]
    public Vector3 Checkpoint;
    [HideInInspector]
    public GrabScript current_grab;
    //bool respawn = false;


    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate = 60;
        //FPS = Application.targetFrameRate;
        terminal_vel = -Mathf.Sqrt((2*MASS*GRAVITY)/(DENSITY*CS_AREA*DRAG));
        current_speed = new Vector3(0, -GRAVITY, 0);
        GM = manager.GetComponent<GameManager>();
        FP = cam.GetComponent<FollowPlayer>();
        Checkpoint = transform.position;
        

        //MOVE_ACCEL_ACOEF = MOVE_SPEED / Mathf.Log10(MOVE_ACCEL_TIME); //Creates quadratic function to fufil time/speed acceleration requirements
    }

    // Update is called once per frame
    void Update()
    {
        particleEffects();

        if (ink_rise)
        {
            RiseInk();
        }

        if (current_speed == new Vector3(0, 0, 0) && dashing == false)
        {
            AS.Stop();
        }

        if (transform.position.y - 12 >= -11 && transform.position.y < 20)
        {
            heightFactor = transform.position.y / -2;
        }

        //Debug.Log(transform.position.y);
        
        
        hasKey2 = hasKey;
        if (!grabbed) { 
            //Checks
            friction();
            CheckGroundCollisions(); //Manages player movement
            

            //Movement
            HorizontalMovement(Input.GetAxisRaw("Horizontal"));
            if (stick_jumpControl) {
                VerticalMovement(Input.GetAxisRaw("Vertical"), 0);
            }
            else
            {
                VerticalMovement(Input.GetAxisRaw("Jump"), Input.GetAxisRaw("Vertical"));
            }
            //DashMovement(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Dash"));
        }

        DashMovement(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Dash"));
        

        //Timers
        timers();

        objectController();
        //Debug.Log(current_speed.y);
    }

    public void RiseInk()
    {
        ink_startPos = new Vector3(ink_effect.transform.position.x, transform.position.y - 1, ink_effect.transform.position.z);
        if (ink_effect.transform.position.y < transform.position.y + ink_riseHeight && ! ink_top)
        {
            Vector3 ink_target = new Vector3(ink_effect.transform.position.x, transform.position.y + ink_riseHeight, ink_effect.transform.position.z);
            ink_effect.transform.position = Vector3.MoveTowards(ink_effect.transform.position, ink_target, ink_riseSpeed * Time.deltaTime);
        }
        else
        {
            if (!ink_top)
            {
                FullRespawn();
            }
            
            ink_top = true;
            //Vector3 ink_target = new Vector3(ink_effect.transform.position.x, ink_startPos.y, ink_effect.transform.position.z);
            ink_effect.transform.position = Vector3.MoveTowards(ink_effect.transform.position, ink_startPos, ink_riseSpeed * Time.deltaTime);
            if (ink_effect.transform.position.y <= transform.position.y)
            {
                ink_rise = false;
                ink_top = false;
            }
        }
    }

    public void playSound(AudioClip AC)
    {
        AS.Stop();
        AS.clip = AC;
        AS.Play();
    }

    public void particleEffects()
    {
        if (PS != null)
        {
            if (!PS_lock && (isFloor || onCurve))
            {
                PS.Play();
                PS_lock = true;
                if (true)
                {
                    //Debug.Log("SOUND: LANDING");
                    playSound(AS_landing);
                }
            }
            else if (PS_lock && !isFloor)
            {
                PS_lock = false;
            }
        }
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
            current_speed -= new Vector3(friction_current * Time.deltaTime, 0, 0);
            if (current_speed.x <= 0)
            {
                current_speed = new Vector3(0, current_speed.y, 0);
            }
        }
        else if (current_speed.x < 0)
        {
            current_speed += new Vector3(friction_current * Time.deltaTime, 0, 0);
            if (current_speed.x >= 0)
            {
                current_speed = new Vector3(0, current_speed.y, 0);
            }
        }
    }

    private void timers()
    {
        if (respawn && respawn_counter < respawn_length)
        {
            respawn_counter += 1f * Time.deltaTime;
        }
        else if (respawn_counter > respawn_length)
        {
            respawn_counter = 0;
            respawn = false;
        }

        //DELAY UNTIL NEXT DASH
        if (dash_delay_start && dash_delayCounter < DASH_DELAY)
        {
            dash_delayCounter += 1f * Time.deltaTime;
        }
        else if (dash_delayCounter >= DASH_DELAY)
        {
            dash_delayCounter = 0f;
            dash_ready = true;
            dash_delay_start = false;
        }

        //LENGTH OF DASH
        if (dashing && dash_counter < DASH_LENGTH)
        {
            dash_counter += 1f * Time.deltaTime;
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
            jump_held_counter += 1f * Time.deltaTime;
            jump_ForcePressed = jump_held_counter;
        }
        else
        {
            jump_held_counter = 0;
        }

        if (jumpQueued && fJump_Counter < ForgetJump) //forget jump
        {
            fJump_Counter += 1f * Time.deltaTime;
        }
        else
        {
            fJump_Counter = 0f;
            jumpQueued = false;
        }

        if (frame < 1f)
        {
            frame += 1f * Time.deltaTime; //Counts each frame until enough are counted that take up 1 second
            //Debug.Log(current_speed.x / (FPS * MOVE_ACCEL_TIME) + " | ");

        }
        else //Executes every Second
        {
            frame = 0.0f;

            //Debug.Log(current_speed + " | " + terminal_vel);
        }
    }
    private float CalculateTotalXMove()
    {
        float totalXMove = 0;
        if ((isWallLeft || isWallRight) && current_speed.x == 0 && speed_dashx == 0) //Player is not moving next to a wall... duh
        {
            //Debug.Log(801);
            totalXMove = 0;
        }
        else if (isWallLeft || isWallRight) //hitting a wall and moving
        {
            if (isWallLeft && current_speed.x >= 0 && speed_dashx >= 0)//leaving right wall
            {
                //Debug.Log(901);
                totalXMove = (current_speed.x + speed_x + speed_dashx);
            }
            else if (isWallRight && current_speed.x <= 0 && speed_dashx <= 0)//leaving left wall
            {
                //Debug.Log(902);
                totalXMove = (current_speed.x + speed_x + speed_dashx);
            }
        }
        else if (!isWallLeft && !isWallRight)//not touching anywalls
        {
            //Debug.Log(802);
            totalXMove = (current_speed.x + speed_x + speed_dashx);
        }
        else
        {
            //Debug.Log(803);
            totalXMove = 0;
        }
        return totalXMove;
    }

    private void CheckGroundCollisions()
    {
        cam.transform.SetPositionAndRotation(new Vector3(transform.position.x + FP.lead, transform.position.y + FP.height, transform.position.z + FP.zoom + heightFactor), cam.transform.rotation);
        if (!isFloor && !grabbed && !onCurve)
        {

            if (gameObject.transform.eulerAngles.z > 0 && gameObject.transform.eulerAngles.z < 180)
            {
                gameObject.transform.Rotate(new Vector3(0, 0, (-ROTATE_SPEED) * Time.deltaTime * ROTATE_TIME));
            } else if (gameObject.transform.eulerAngles.z >= 180)
            {
                gameObject.transform.Rotate(new Vector3(0, 0, ROTATE_SPEED * Time.deltaTime * ROTATE_TIME));
            }
            Quaternion pRotation = gameObject.transform.rotation;
            pRotation.eulerAngles = new Vector3(FP.rotation.x, FP.rotation.y, FP.rotation.z);
            cam.transform.SetPositionAndRotation(new Vector3(transform.position.x + FP.lead, transform.position.y + FP.height, transform.position.z + FP.zoom + heightFactor), pRotation);

            if (isRoof)
            {
                //Debug.Log("FALL: " + current_speed.y);
                transform.position += new Vector3((current_speed.x + speed_x + speed_dashx) * Time.deltaTime, current_speed.y * Time.deltaTime, 0); //MOVES THE PLAYER TO EQUATE TO 1 SECOND 
            }
            else
            {
                float totalXMove = CalculateTotalXMove();
                
                transform.position += new Vector3(totalXMove * Time.deltaTime, (current_speed.y + speed_y + speed_dashy) * Time.deltaTime, 0); //MOVES THE PLAYER TO EQUATE TO 1 SECOND

            }

            if (current_speed.y > terminal_vel && (currentGroundScript == null || !isFloor))
            {
                if (!isFloor & !dashing)
                {
                    current_speed -= new Vector3(0, GRAVITY * Time.deltaTime, 0); //INCREASES GRAVITY IN SMALL SECTIONS TO EQUATE TO 1 SECOND [FALLING]
                }

            }


        }
        else if (!grabbed) //player is on the ground
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
                totalXMove = CalculateTotalXMove();
            }

            if ((isFloor || onCurve) && !isRoof)
            {
                current_speed = new Vector3(current_speed.x, 0, 0); //Stop player from falling once they hit the ground
                speed_y = 0;
            }
            else if (isRoof)
            {
                Debug.Log("ROOF");
                current_speed = new Vector3(current_speed.x, current_speed.y, 0);
            }
            //speed_y = 0;
            if (jumpQueued && (isFloor || onCurve)) //IF JUMP QUEUED AND ON GROUND, THEN JUMP
            {
                jumpQueued = false;

                //float slopeY = (currentGroundScript.slope) * ((transform.position.x) - currentGroundScript.trans.position.x) + (currentGroundScript.verticalWidthAP);

                speed_y = JUMP_SPEED;
                if (current_speed.y > JUMP_SPEED)
                {
                    speed_y = JUMP_SPEED;

                }

            }
            //Debug.Log(isFloor);
            if (!onCurve || speed_y > 0)
            {
                if (isFloor)
                {
                    transform.position += new Vector3(0, (speed_y) * Time.deltaTime, 0); //MOVES THE PLAYER TO EQUATE TO 1 SECOND 
                }
                else
                {
                    transform.position += new Vector3(0, (speed_y + speed_dashy) * Time.deltaTime, 0); //MOVES THE PLAYER TO EQUATE TO 1 SECOND 
                }
                
            }
            if (!onCurve)
            {
                //Debug.Log("SPEED: " + totalXMove);
                //Debug.Log(isWallLeft + " : " + isWallRight);
                transform.position += transform.TransformDirection((totalXMove) * Time.deltaTime, 0, 0);
            }

        }
        else if (grabbed && dashing && !isWallLeft && !isWallLeft && !isFloor && !isRoof) 
        {
           
            transform.position += new Vector3((speed_dashx) *Time.deltaTime, (speed_dashy) * Time.deltaTime, 0); //MOVES THE PLAYER TO EQUATE TO 1 SECOND 
        }
       
    }

    private void DashMovement(float LR, float UD, float dash)
    {
        if (dash > 0 && !dashing && dash_ready && flowerCount >= 1)
        {
            float diag = DASH_SPEED / 1.5f;
            dashing = true;
            dash_ready = false;
            flowerCount--;

            speed_dashy = UD * DASH_SPEED;
            speed_dashx = LR * DASH_SPEED;
            if (current_speed.y < 0)
            {
                current_speed.y = current_speed.y/5;
            }
            
            //Debug.Log(UD + " :?: " + LR);

            dashEffect.GetComponent<ParticleSystem>().Play();
            if (true)
            {
                //Debug.Log("SOUND: DASHING");
                playSound(AS_dash);
            }
            
        }
    }

    private void VerticalMovement(float dir, float stickDir)
    {
        if (!dashing && !grabbed)
        {

            if (dir > 0 && !jump_held && !isRoof)
            {
                playSound(AS_jumping);
                //Debug.Log("SOUND: JUMPING");
                jump_held = true;
                jumpQueued = true;
            }
            if (dir == 0 && jump_held)
            {
                jump_held = false;
                speed_y /= JUMP_CANCEL;
            }
            if (dir > 0 && jump_held && isFloor)
            {
                jump_held = false;
            }
            if (dir < 0 || stickDir < 0)
            {
                speed_y = 0;
            }
        }
       
    }

    private void HorizontalMovement(float dir)
    {
        if (!dashing && !grabbed)
        {
            if (!AS.isPlaying && isFloor && dir != 0)
            {
                //Debug.Log("SOUND: WALKING");
                playSound(AS_walking);
            }

            speed_x = 0;
            if (dir > 0 && !isWallRight)
            {
                if (prevDir <= 0)
                {
                    fMove_Counter = 0;
                }

                /*fMove_Counter += 1f / (Time.deltaTime * MOVE_ACCEL_TIME);
                MOVE_ACCEL_ACOEF = Mathf.Pow(MOVE_SPEED, (1f / MOVE_ACCEL_TIME));
                current_speed = new Vector3(Mathf.Pow(MOVE_ACCEL_ACOEF, fMove_Counter), current_speed.y, 0);*/

                fMove_Counter += 1f * Time.deltaTime * MOVE_ACCEL_TIME;
                MOVE_ACCEL_ACOEF = Mathf.Pow(MOVE_SPEED, fMove_Counter);
                current_speed = new Vector3(MOVE_ACCEL_ACOEF, current_speed.y, 0);

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

                /* fMove_Counter += 1f / (Time.deltaTime * MOVE_ACCEL_TIME);
                 MOVE_ACCEL_ACOEF = Mathf.Pow(MOVE_SPEED, (1f / MOVE_ACCEL_TIME));
                 current_speed = new Vector3(-(Mathf.Pow(MOVE_ACCEL_ACOEF, fMove_Counter) + MOVE_ACCEL_X), current_speed.y, 0);*/

                fMove_Counter += 1f * Time.deltaTime * MOVE_ACCEL_TIME;
                MOVE_ACCEL_ACOEF = Mathf.Pow(MOVE_SPEED, fMove_Counter);
                current_speed = new Vector3(-MOVE_ACCEL_ACOEF, current_speed.y, 0);

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
            Debug.Log("ROOF DETECTED");
            speed_dashy = 0;
            speed_y = 0;
            if (isRoof)
            {
                current_speed = new Vector3(0, -2.45f, 0);
            }
        }

        if (isFloor)
        {
            speed_y = 0;
        }

        Debug.Log("Collide");

    }

    public void Respawn()
    {
        respawn = true;
        ink_rise = true;
        ink_top = false;
    }

    public void FullRespawn()
    {
        transform.position = Checkpoint;
        if (current_grab != null)
        {
            current_grab.grabbed = false;
        }
        grabbed = false;

        speed_dashx = 0;
        speed_dashy = 0;
        speed_x = 0;
        speed_y = 0;
        current_speed = (Vector3.zero);

        GM.resetFlowers();
        GM.resetKeys();


        CollisionDetected(false, false, false, false, 30);
        isWallLeft = false;
        isFloor = false;

        PFC.contacts = 0;
        //ink_rise = false;
        Debug.Log("Respawn");
    }
        

}
