using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Health Things")]
    private float playerHealth = 1000f;
    private float presentHealth;

    public ProgressBar healthBar;


    [Header("Player Movement")]  //the header is used to indicate the variable use
    public float playerSpeed = 1.9f;
    public float currentPlayerSpeed = 0f;
    public float playerSprint = 3f;
    public float currentPlayerSprint = 0f;

    [Header("Player Animator and Gravity")]
    public CharacterController cC;
    public float gravity = -9.81f;
    public Animator animator;

    [Header("Player Camera")]
    public Transform playerCamera;

    [Header("Player Jumping & velocity")]
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;
    Vector3 velocity;
    public Transform surfaceCheck; //to check if there is a surface below the player
    bool onSurface; // to check if the player is on the surface
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;
    public float jumpRange = 1f;

    //MOBILE
    public bool mobileInputs;
    public FixedJoystick joystick;
    public FixedJoystick Sprintjoystick;

   
    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        presentHealth = playerHealth;
        healthBar.BarValue = 100; ;
    }


    // Update is called once per frame
    void Update()
    {

        if (currentPlayerSpeed > 0)
        {
            Sprintjoystick = null;
        }
        else
        {
            FixedJoystick sprintJS = GameObject.Find("PlayerSprintJoystick").GetComponent<FixedJoystick>();
            Sprintjoystick = sprintJS;
        } 
        onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);
        if (onSurface && velocity.y < 0)
        {
            velocity.y = -2f;//This ensures that the player is slightly pushed
                             //into the ground to prevent jittery behavior.
        }

        //gravity
        velocity.y += gravity * Time.deltaTime;//when the player is on air , exp he is faaling we made ,
                                               //the fall time dependant to keep it reel
        cC.Move(velocity * Time.deltaTime);

        playerMove();
        Jump();
        Sprint();

    }

    void playerMove()
    {
      if(mobileInputs == true) //if we're playing on mobile
        {
            float horizontalAxis =joystick.Horizontal;
            float verticalAxis = joystick.Vertical;

            // Move the player according to those inputs
            Vector3 direction = new Vector3(horizontalAxis, 0f, verticalAxis).normalized; //converting it to a unit vector, which has a magnitude of 1
                                                                                          //nkhdmou 3ala vecteur unitaire dima
            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Running", false); //when he's walking , no running 
                animator.SetBool("Idle", false);//when he's walking , no idle (we9if)
                animator.SetTrigger("Jump"); //when walking, he can also jump
                animator.SetBool("AimWalk", false);
                animator.SetBool("IdleAim", false);

                //tan(0) = x/y radian ->Deg
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                //smoothing the rotation
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                //returns a rotation ->rotate our player making him face the direction of movement.
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                //calculate the moveDirection based on the rotation and the forward direction
                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                cC.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);
                currentPlayerSpeed = playerSpeed;
            }




            else //player not walking -> idle
            {
                animator.SetBool("Idle", true);
                animator.SetTrigger("Jump");

                animator.SetBool("Walk", false);
                animator.SetBool("Running", false);
                animator.SetBool("AimWalk", false);
                currentPlayerSpeed = 0f;
            }
        }
        else //if we're playing in unity
        {
            float horizontalAxis = Input.GetAxisRaw("Horizontal");
            float verticalAxis = Input.GetAxisRaw("Vertical");

            // Move the player according to those inputs
            Vector3 direction = new Vector3(horizontalAxis, 0f, verticalAxis).normalized; //converting it to a unit vector, which has a magnitude of 1
                                                                                          //nkhdmou 3ala vecteur unitaire dima
            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Running", false); //when he's walking , no running 
                animator.SetBool("Idle", false);//when he's walking , no idle (we9if)
                animator.SetTrigger("Jump"); //when walking, he can also jump
                animator.SetBool("AimWalk", false);
                animator.SetBool("IdleAim", false);

                //tan(0) = x/y radian ->Deg
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                //smoothing the rotation
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                //returns a rotation ->rotate our player making him face the direction of movement.
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                //calculate the moveDirection based on the rotation and the forward direction
                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                cC.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);
                currentPlayerSpeed = playerSpeed;
            }
            else //player not walking -> idle
            {
                animator.SetBool("Idle", true);
                animator.SetTrigger("Jump");

                animator.SetBool("Walk", false);
                animator.SetBool("Running", false);
                animator.SetBool("AimWalk", false);
                currentPlayerSpeed = 0f;
            }
        }
     }

    




    void Jump()
    {
      if (mobileInputs == true)
        {
            if (CrossPlatformInputManager.GetButtonDown("Jump") && onSurface)
            {
                animator.SetBool("Walk", false);
                animator.SetTrigger("Jump");


                velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity); 
            }
            else
            {
                animator.ResetTrigger("Jump");
            }


        }
        else
        {
            if (Input.GetButtonDown("Jump") && onSurface) //GetButtonDown khtrna ninzlou 3al space to jump
            {
                animator.SetBool("Walk", false);
                animator.SetTrigger("Jump");


                velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity); //vertical motion fomula=
                                                                   //sqrt(height of the jump * gravity * 2
                                                                   //hatina -2 khtr gravity = -9.8 so -2 * -9.8 >0
            }
            else
            {
                animator.ResetTrigger("Jump");
            }
        }
    }

    void Sprint()
    {
        if (mobileInputs == true) //if we're playing on mobile
        {
            float horizontalAxis = Sprintjoystick.Horizontal;
            float verticalAxis = Sprintjoystick.Vertical;

            Vector3 direction = new Vector3(horizontalAxis, 0f, verticalAxis).normalized;
            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Running", true);
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", false);
                animator.SetBool("IdleAim", false);


                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                cC.Move(moveDirection.normalized * playerSprint * Time.deltaTime);
                currentPlayerSprint = playerSprint;
            }




            else 
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", false);
                currentPlayerSprint = 0f;

            }
        }
        else //if we're playing in unity
        {
            float horizontalAxis = Input.GetAxisRaw("Horizontal");
            float verticalAxis = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontalAxis, 0f, verticalAxis).normalized; 
            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Running", true);
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", false);
                animator.SetBool("IdleAim", false);


                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                cC.Move(moveDirection.normalized * playerSprint * Time.deltaTime);
                currentPlayerSprint = playerSprint;
            }
            else 
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", false);
                currentPlayerSprint = 0f;
            }
        }
    }


    //playerHitDamage
    public void playerHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        healthBar.UpdateHealth(presentHealth, playerHealth); // Update the health bar

        if (presentHealth <=0) {
            PlayerDie();
        }
    }

    private void PlayerDie()
    {
        Cursor.lockState = CursorLockMode.None;

        Destroy(gameObject);
    }
}

