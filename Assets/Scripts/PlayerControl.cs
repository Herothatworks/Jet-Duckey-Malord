using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    //Movement based mechanics
    public float PlayerMoveSpeed;
    public float JumpHeight;
    public float PlayerRunSpeed;

    //This controls the player's limits in movement
    private Rigidbody playerRig;
    public MoveLimits playerBoundary;

    //Controls for the player's animations
    private Animator playerAnime;

    //Key Commands, for simplicity. Easy to change instead of hunting stuff down in the code.
    public static KeyCode JetPunch = KeyCode.O;
    public static KeyCode JetKick = KeyCode.P;
    public static KeyCode JetGrab = KeyCode.Semicolon;
    public static KeyCode JetBlock = KeyCode.L;
    public static KeyCode JetAbilityUse = KeyCode.K;
    public static KeyCode JetAbilitySwap = KeyCode.I;
    public static KeyCode JetLeft = KeyCode.A;
    public static KeyCode JetRight = KeyCode.D;
    public static KeyCode JetUp = KeyCode.W;
    public static KeyCode JetDown = KeyCode.S;
    public static KeyCode JetJump = KeyCode.Space;

    //Combo information
    public float DashLength;
    private ComboMoves DashLeft = new ComboMoves(new List<KeyCode>() { JetLeft, JetLeft }, 0.3f);
    private ComboMoves DashRight = new ComboMoves(new List<KeyCode>() { JetRight, JetRight }, 0.3f);

    //Punch attack stuff.
    public float punchCooldown;
    public float timeAllowedBetweenPunches;
    public float singlePunchCooldown;

    private float punchTimer;
    private float punchBetweenCounter;
    private float singlePunchCounter;

    private bool cooldownPunch;
    private bool justPunched;
    private bool queuePunch;

    private float punchCount = 0f;

    // Start is called before the first frame update
    void Start()
    {
        playerRig = GetComponent<Rigidbody>();
        playerAnime = GetComponent<Animator>();
    }

    //Checks to make sure the player can jump, so we don't have a flying character - yet
    bool GroundCheck()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.5f))
        {
            if (hit.transform.CompareTag("Ground"))
                return true;
        }

        return false;
    }


    // Update is called once per frame
    void Update()
    {
        //Get the move controls
        float HorizontalMovement = Input.GetAxisRaw("Horizontal");
        float VerticalMovement = Input.GetAxisRaw("Vertical");

        //This is for running mechanics
        float movingpace = 0f;

        //Add Jump stuff
        if(GroundCheck() && Input.GetKeyDown(JetJump))
        {
            playerRig.AddForce(0, 1 * JumpHeight, 0, ForceMode.Impulse);
            playerAnime.SetTrigger("Jump");
        }

        //Keeps players within the street to avoid running too far off camera up or down.
        

        //Sets the direction the player is facing, left or right
        if(HorizontalMovement < 0f)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        else if (HorizontalMovement > 0f)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }

        //Sets the player to running (or not running)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if ((transform.position.x > playerBoundary.LeftBound && HorizontalMovement < 0) || (transform.position.x < playerBoundary.RightBound && HorizontalMovement > 0))
            {
                transform.position += transform.right * Time.deltaTime * PlayerRunSpeed * HorizontalMovement;
            }
            if ((transform.position.z > playerBoundary.Lower && VerticalMovement < 0) || (transform.position.z < playerBoundary.Upper && VerticalMovement > 0))
            {
                transform.position += transform.forward * Time.deltaTime * PlayerRunSpeed * VerticalMovement;
            }
            movingpace = 1f;
        }
        else
        {
            if ((transform.position.x > playerBoundary.LeftBound && HorizontalMovement < 0) || (transform.position.x < playerBoundary.RightBound && HorizontalMovement > 0))
            {
                transform.position += transform.right * Time.deltaTime * PlayerMoveSpeed * HorizontalMovement;
            }
            if ((transform.position.z > playerBoundary.Lower && VerticalMovement < 0) || (transform.position.z < playerBoundary.Upper && VerticalMovement > 0))
            {
                transform.position += transform.forward * Time.deltaTime * PlayerMoveSpeed * VerticalMovement;
            }
            movingpace = 0.5f;
        }

        if(HorizontalMovement == 0 && VerticalMovement ==0)
        {
            movingpace = 0f;
        }

        //Lets the animator know if we are moving or not.
        playerAnime.SetFloat("Speed", movingpace);

        //Punch Attack Setup
        if(Input.GetKeyDown(JetPunch) && !cooldownPunch && !queuePunch)
        {
            playerAnime.SetFloat("Punch", punchCount);
            playerAnime.SetTrigger("Punching");
            punchCount += 0.5f;
            justPunched = true;
            punchBetweenCounter = 0f; //Rest every time a punch goes through.
            queuePunch = true;
            if(punchCount > 1)
            {
                cooldownPunch = true;
                punchCount = 0f;
            }
        }

        if(queuePunch)
        {
            singlePunchCounter += Time.deltaTime;
            if(singlePunchCounter >= singlePunchCooldown)
            {
                singlePunchCounter = 0f;
                queuePunch = false;
            }
        }

        if(justPunched)
        {
            punchBetweenCounter += Time.deltaTime;
            if(punchBetweenCounter > timeAllowedBetweenPunches)
            {                
                punchCount = 0f;
                punchBetweenCounter = 0f;
                justPunched = false;
            }
        }

        if (cooldownPunch)
        {
            punchTimer += Time.deltaTime;
            if (punchTimer < punchCooldown)
            {
                punchTimer = 0f;
                cooldownPunch = false;
            }
        }

        //Combos for the game
        if (DashLeft.CheckCombo() && transform.position.x > playerBoundary.LeftBound + DashLength)
        {
            transform.position -= transform.right * DashLength;
        }
        if(DashRight.CheckCombo() && transform.position.x < playerBoundary.RightBound - DashLength)
        {
            transform.position += transform.right * DashLength;
        }
    }
}
