using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public float PlayerMoveSpeed;
    public float JumpHeight;
    public float PlayerRunSpeed;

    private Rigidbody playerRig;
    public MoveLimits playerBoundary;
    private Animator playerAnime;

    public float DashLength;
    private ComboMoves DashRight = new ComboMoves(new string[] { "right", "right" }, 0.3f);
    private ComboMoves DashLeft = new ComboMoves(new string[] { "left", "left" }, 0.3f);

    // Start is called before the first frame update
    void Start()
    {
        playerRig = GetComponent<Rigidbody>();
        playerAnime = GetComponent<Animator>();
    }

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
        float HorizontalMovement = Input.GetAxisRaw("Horizontal");
        float VerticalMovement = Input.GetAxisRaw("Vertical");

        //Add Jump stuff
        if(GroundCheck() && Input.GetButtonDown("Jump"))
        {
            playerRig.AddForce(0, 1 * JumpHeight, 0, ForceMode.Impulse);
            playerAnime.SetTrigger("Jump");
        }

        //Keeps players within the street to avoid running too far off camera up or down.
        if ((transform.position.z > playerBoundary.Lower && VerticalMovement < 0) || (transform.position.z < playerBoundary.Upper && VerticalMovement > 0))
        {
            transform.position += transform.forward * Time.deltaTime * PlayerMoveSpeed * VerticalMovement;
        }

        if(HorizontalMovement < 0f)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        else if (HorizontalMovement > 0f)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }

        if(HorizontalMovement != 0f || VerticalMovement != 0f)
        {
            playerAnime.SetBool("Moving", true);
        }
        else
        {
            playerAnime.SetBool("Moving", false);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += transform.right * Time.deltaTime * PlayerRunSpeed * HorizontalMovement;
        }
        else
        {
            transform.position += transform.right * Time.deltaTime * PlayerMoveSpeed * HorizontalMovement;
        }

        //Combos for the game
        if(DashLeft.CheckCombo())
        {
            transform.position -= transform.right * DashLength;
        }
        if(DashRight.CheckCombo())
        {
            transform.position += transform.right * DashLength;
        }
    }
}
