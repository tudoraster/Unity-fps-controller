using System.Collections;
using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Refrences
    public CharacterController controller;

    //Movement variables
    float moveSpeed;
    public float walkSpeed = 7.5f;
    public float sprintSpeed = 30f;
    public float crouchSpeed = 2.5f;
    public float slideSpeed = 35f;
    private float slidingDuration = 1.5f;
    private float airSpeed = 5f;
    public float jumpHeight = 10f;
    public float gravity = -9.81f;

    Vector3 velocity;

    //IsGround variables
    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;

    //CanUnCrouch variables
    public Transform upCheck;
    public LayerMask upMask;

    //Crouching variables
    Vector3 originalHeight;
    Vector3 crouchingHeight = new Vector3(0, 0.75f, 0);

    void Start()
    {
        originalHeight = transform.localScale;
    }

    void Update()
    {
        Movement();
        StartCoroutine(SlideCrouchSprint());
    }

    //Checking if the player is currently sliding
    private bool IsSliding()
    {
        if (IsSprinting() && IsCrouching())
        {
            return true;
        }
        else { return false; }
    }

    //Checking if character is crouching
    private bool IsCrouching()
    {
        if (Input.GetKey(KeyCode.LeftControl)) { return true; }
        else { return false; }
    }

    //Checking if character is sprinting
    private bool IsSprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift)) { return true; }
        else { return false; }
    }

    //Crouching Sliding Sliding mechanics in one
    IEnumerator SlideCrouchSprint()
    {
        moveSpeed = slideSpeed;

        if (IsSliding())
        {
            transform.localScale = crouchingHeight;

            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            if (!IsGrounded())
            {
                controller.Move(transform.forward * airSpeed * Time.deltaTime);
            }

            yield return new WaitForSeconds(slidingDuration);

            if (IsCrouching())
            {
                moveSpeed = crouchSpeed;
            }
            else if (IsSprinting())
            {
                moveSpeed = sprintSpeed;
            }
            else
            {
                moveSpeed = crouchSpeed;
            }
        }
        else if (IsSprinting())
        {
            moveSpeed = sprintSpeed;

            transform.localScale = originalHeight;

            Sprint();
        }
        else if (IsCrouching())
        {
            Crouch();
        }
        else
        {
            moveSpeed = walkSpeed;

            transform.localScale = originalHeight;
        }
    }

    //Crouching mechanic
    private void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = crouchSpeed;
            transform.localScale = crouchingHeight;
        }
        else
        {
            transform.localScale = originalHeight;
            moveSpeed = crouchSpeed;
        }
    }

    //Sprinting mechanic
    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
        {
            moveSpeed = sprintSpeed;
        }
        else { moveSpeed = walkSpeed; }
    }

    //Check if isGrounded
    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    //Movement
    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        velocity.y += gravity * Time.deltaTime;

        //Checking if player is grounded so the velocity resets to 0
        if (IsGrounded() && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        controller.Move(move * moveSpeed * Time.deltaTime);

        //Jumping mechanic
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        controller.Move(velocity * Time.deltaTime);
    }
}
