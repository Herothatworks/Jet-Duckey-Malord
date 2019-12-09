using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    //Movement based mechanics
    public float PlayerMoveSpeed;
    public float JumpHeight;
    public float PlayerRunSpeed;
    private bool facingRight = true;

    //Player Health Stuff
    public Image healthBar;
    public Image powerBar;
    public float PlayerPower;
    private float currentPower;
    public float PlayerHealth;
    private float currentHealth;

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
    public float PunchDamage;
    public float KickDamage;
    private Transform punchBox;
    public GameObject punchForce;

    public float punchCooldown;
    public float kickCooldown;
    public float timeAllowedBetweenPunches;
    public float singlePunchCooldown;

    private float punchTimer;    
    private float punchBetweenCounter;
    private float singlePunchCounter;

    private bool cooldownPunch;
    private bool justPunched;
    private bool justKicked;
    private bool queuePunch;

    private float punchCount = 0f;

    //Damage Taking stuff
    private bool isInvulnerable = false;
    private float invulnTimer = 0f;
    public float DamageInvulnerabilityTimer;

    //Death Stuff
    private bool deathStart = false;
    private float deathDelay = 0.5f;
    private float deathCount = 0f;

    public bool MetalOn = false;
    public float MetalDamageTaken;
    public float MetalDamageDone;


    // Start is called before the first frame update
    void Awake()
    {
        playerRig = GetComponent<Rigidbody>();
        playerAnime = GetComponent<Animator>();
        playerBoundary.LeftBound = GameObject.FindGameObjectWithTag("LeftLimit").transform.position.x;
        playerBoundary.RightBound = GameObject.FindGameObjectWithTag("RightLimit").transform.position.x;

        foreach(Transform checkBoxes in transform)
        {
            if (checkBoxes.CompareTag("PunchArea"))
            {
                punchBox = checkBoxes;
            }
        }

        currentHealth = PlayerHealth;
        currentPower = PlayerPower;
        deathStart = false;
        healthBar.fillAmount = 1f;
        powerBar.fillAmount = 1f;
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

    public void TakeDamage(float Health)
    {
        if(MetalOn)
        {
            Health = Health * MetalDamageTaken;
        }

        if (!isInvulnerable)
        {
            currentHealth -= Health;
            isInvulnerable = true;
            playerAnime.SetTrigger("TakeDamage");
        }

        if (currentHealth > PlayerHealth)
        {
            currentHealth = PlayerHealth; //Don't go over max health
        }

        if (currentHealth <= 0)
        {
            deathStart = true;
        }

        healthBar.fillAmount = currentHealth / PlayerHealth;

    }

    public void DrainPower(float Power)
    {
        currentPower -= Power;

        if(currentPower > PlayerPower)
        {
            currentPower = PlayerPower;
        }

        if(currentPower <= 0)
        {
            currentPower = 0;
            MetalOn = false;
        }

        powerBar.fillAmount = currentPower / PlayerPower;
    }

    public void RestorePower(float Power)
    {
        DrainPower(-Power);
    }

    public void RestoreHealth(float Health)
    {
        currentHealth += Health;

        if (currentHealth > PlayerHealth)
        {
            currentHealth = PlayerHealth; //Don't go over max health
        }

        healthBar.fillAmount = currentHealth / PlayerHealth;
    }

    //Instantiate a punch instead of raycast
    void MakeAttack(float Damage)
    {
        if(MetalOn)
        {
            Damage = Damage * MetalDamageDone;
        }

        GameObject newPunch = Instantiate(punchForce, punchBox.position, transform.rotation, transform) as GameObject;
        newPunch.GetComponent<PunchAttack>().Damage = Damage;
        newPunch.GetComponent<PunchAttack>().hurtEnemy = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!deathStart)
        {
            //Get the move controls
            float HorizontalMovement = Input.GetAxisRaw("Horizontal");
            float VerticalMovement = Input.GetAxisRaw("Vertical");

            //Please keep this here.
            Debug.DrawRay(transform.position, transform.TransformDirection(facingRight ? Vector3.right : -Vector3.right), Color.red, .3f);

            //This is for running mechanics
            float movingpace = 0f;

            //Add Jump stuff
            if (GroundCheck() && Input.GetButtonDown("Jump"))
            {
                playerRig.AddForce(0, 1 * JumpHeight, 0, ForceMode.Impulse);
                playerAnime.SetTrigger("Jump");
            }

            //Invulnerability Timer
            if (isInvulnerable)
            {
                invulnTimer += Time.deltaTime;
                if (invulnTimer >= DamageInvulnerabilityTimer)
                {
                    invulnTimer = 0f;
                    isInvulnerable = false;
                }
            }


            //Sets the direction the player is facing, left or right
            if (HorizontalMovement < 0f)
            {
                if (facingRight)
                {
                    transform.Rotate(0, 180f, 0);
                    facingRight = false;
                }
            }
            else if (HorizontalMovement > 0f)
            {
                if (!facingRight)
                {
                    transform.Rotate(0, -180f, 0);
                    facingRight = true;
                }
            }

            //Have a way to quit
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("MainMenu");
            }

            if(Input.GetKeyDown(JetAbilityUse))
            {
                MetalOn = !MetalOn;
            }

            if(MetalOn)
            {
                if (currentPower <= 0)
                    MetalOn = false;
            }

            playerAnime.SetBool("IsMetal", MetalOn);

            //Sets the player to running (or not running)
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if ((transform.position.x > playerBoundary.LeftBound && HorizontalMovement < 0) || (transform.position.x < playerBoundary.RightBound && HorizontalMovement > 0))
                {
                    transform.position += transform.right * Time.deltaTime * PlayerRunSpeed * Mathf.Abs(HorizontalMovement);
                }
                if ((transform.position.z > playerBoundary.Lower && VerticalMovement < 0) || (transform.position.z < playerBoundary.Upper && VerticalMovement > 0))
                {
                    if (!facingRight)
                    {
                        VerticalMovement *= -1;
                    }
                    transform.position += transform.forward * Time.deltaTime * PlayerRunSpeed * VerticalMovement;
                }
                movingpace = 1f;
            }
            else
            {
                if ((transform.position.x > playerBoundary.LeftBound && HorizontalMovement < 0) || (transform.position.x < playerBoundary.RightBound && HorizontalMovement > 0))
                {
                    transform.position += transform.right * Time.deltaTime * PlayerMoveSpeed * Mathf.Abs(HorizontalMovement);
                }
                if ((transform.position.z > playerBoundary.Lower && VerticalMovement < 0) || (transform.position.z < playerBoundary.Upper && VerticalMovement > 0))
                {
                    if (!facingRight)
                    {
                        VerticalMovement *= -1;
                    }
                    transform.position += transform.forward * Time.deltaTime * PlayerMoveSpeed * VerticalMovement;
                }
                movingpace = 0.5f;
            }

            if (HorizontalMovement == 0 && VerticalMovement == 0)
            {
                movingpace = 0f;
            }

            //Lets the animator know if we are moving or not.
            playerAnime.SetFloat("Speed", movingpace);

            //Punch Attack Setup
            if (Input.GetKeyDown(JetPunch) && !cooldownPunch && !queuePunch && !justKicked)
            {
                playerAnime.SetFloat("Punch", punchCount);
                playerAnime.SetTrigger("Punching");
                punchCount += 0.5f;

                MakeAttack(PunchDamage);

                if(MetalOn)
                {
                    DrainPower(1);
                }

                justPunched = true;
                punchBetweenCounter = 0f; //Rest every time a punch goes through.
                punchTimer = 0f;
                queuePunch = true;
                if (punchCount > 1)
                {
                    cooldownPunch = true;
                    punchCount = 0f;
                }
            }

            if (Input.GetKeyDown(JetKick) && !cooldownPunch && !queuePunch && !justKicked)
            {
                playerAnime.SetTrigger("Kick");
                justKicked = true;
                MakeAttack(KickDamage);
                if (MetalOn)
                {
                    DrainPower(2);
                }
            }

            if (justKicked)
            {
                singlePunchCounter += Time.deltaTime;
                if (singlePunchCounter >= kickCooldown)
                {
                    singlePunchCounter = 0f;
                    justKicked = false;
                }
            }


            if (queuePunch)
            {
                singlePunchCounter += Time.deltaTime;
                if (singlePunchCounter >= singlePunchCooldown)
                {
                    singlePunchCounter = 0f;
                    queuePunch = false;
                }
            }

            if (justPunched)
            {
                punchBetweenCounter += Time.deltaTime;
                if (punchBetweenCounter > timeAllowedBetweenPunches)
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
                if(facingRight)
                    transform.position -= transform.right * DashLength;
                else
                    transform.position += transform.right * DashLength;
            }
            if (DashRight.CheckCombo() && transform.position.x < playerBoundary.RightBound - DashLength)
            {
                if(facingRight)
                    transform.position += transform.right * DashLength;
                else
                    transform.position -= transform.right * DashLength;
            }
        }
        else
        {
            deathCount += Time.deltaTime;
            if (deathCount >= deathDelay)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
