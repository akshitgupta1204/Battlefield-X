using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{

    //public float moveSpeed;
    public float jumpForce;

    public float jumpTime;
    private float jumpTimeCounter;

    private Rigidbody2D myRigidbody;
    public bool grounded;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    public float groundCheckRadius;
    private Animator myAnimator;
    //private Collider2D myCollider;
    // Use this for initialization

    private bool stoppedJumping;
    private bool canDoubleJump;
    private int count;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        //myCollider = GetComponent<Collider2D>();
        myAnimator = GetComponent<Animator>();
        jumpTimeCounter = jumpTime;
        stoppedJumping = true;
        count = 0;

    }



    // Update is called once per frame
    void Update()
    {
        //grounded = Physics2D.IsTouchingLayers(myCollider, whatIsGround);
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        //myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = new Vector2(PlayerStats.instance.movementSpeed, myRigidbody.velocity.y);

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            if (grounded)
            {
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
                stoppedJumping = false;
            }

            if(!grounded && canDoubleJump)
            {
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
                jumpTimeCounter = jumpTime;
                stoppedJumping = false;
                canDoubleJump = false;
            }

        }

        if (CrossPlatformInputManager.GetButton("Jump") && !stoppedJumping)
        {

            if (jumpTimeCounter > 0)
            {

                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
        }

        if (CrossPlatformInputManager.GetButtonUp("Jump"))
        {

            jumpTimeCounter = 0;
            stoppedJumping = true;
        }

        if (grounded)
        {
            jumpTimeCounter = jumpTime;
            canDoubleJump = true;
            
        }

        

        

        myAnimator.SetFloat("Speed", myRigidbody.velocity.x);
        myAnimator.SetBool("Ground", grounded);



    }
}
