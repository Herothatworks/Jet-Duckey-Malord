using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public float PlayerMoveSpeed;
    public float JumpHeight;
    private Rigidbody playerRig;
    public MoveLimits playerBoundary;

    //public float PlayerRunSpeed;
    //private bool isRunning;


    // Start is called before the first frame update
    void Start()
    {
        playerRig = GetComponent<Rigidbody>();
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
            //playerRig.AddForce(transform.up * JumpHeight * Time.deltaTime);
            playerRig.AddForce(0, 1 * JumpHeight, 0, ForceMode.Impulse);
            //transform.position += transform.up * Time.deltaTime * JumpHeight;
        }

        //Keeps players within the street to avoid running too far off camera up or down.
        if ((transform.position.z > playerBoundary.Lower && VerticalMovement < 0) || (transform.position.z < playerBoundary.Upper && VerticalMovement > 0))
        {
            transform.position += transform.forward * Time.deltaTime * PlayerMoveSpeed * VerticalMovement;
        }

        transform.position += transform.right * Time.deltaTime * PlayerMoveSpeed * HorizontalMovement;        
    }
}
