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
    private float slidingDuration = 0.75f;
    public float jumpHeight = 10f;
    public float gravity = -9.81f;

    Vector3 velocity;

    //IsGround variables
    public Transform groundCheck;
    private float groundDistance = 0.4f;
    public LayerMask groundMask;

    //Crouching variables
    Vector3 originalHeight;
    Vector3 crouchingHeight = new Vector3(1f, 0.75f, 1f);

    //Input
    private float x;
    private float z;

    private void Start()
    {
        originalHeight = transform.localScale;
    }

    private void Update()
    {
        Movement();
        Mechanics();
    }

    //Check if character is sliding
    private bool IsSliding()
    {
        if (Input.GetKey(KeyCode.LeftShift) && IsGrounded())
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                return true;
            }
            else { return false; }
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

    //All mechanics into one method
    private void Mechanics()
    {
        transform.localScale = originalHeight;
        moveSpeed = walkSpeed;

        if (IsSprinting() && !IsCrouching())
        {
            Sprint();
            StopAllCoroutines();
        }
        else if(IsCrouching() && !IsSprinting())
        {
            Crouch();
            StopAllCoroutines();
        }
        else if (IsSliding())
        {
            Slide();
            StartCoroutine(StopSlide());
        }
        else
        {
            transform.localScale = originalHeight;
            moveSpeed = walkSpeed;
            StopAllCoroutines();
        }
    }

    //Stopping the slide after a specific number of seconds
    IEnumerator StopSlide()
    {
        if (IsSliding())
        {
            yield return new WaitForSeconds(slidingDuration);

            transform.localScale = crouchingHeight;
            moveSpeed = crouchSpeed;
        }
    }

    //Sliding mechanic
    private void Slide()
    {
        if (IsSliding())
        {
            moveSpeed = slideSpeed;
            transform.localScale = crouchingHeight;
        }
    }

    //Crouching mechanic
    private void Crouch()
    {
        if (IsCrouching())
        {
            transform.localScale = crouchingHeight;
            moveSpeed = crouchSpeed;
        }
    }

    //Sprinting mechanic
    private void Sprint()
    {
        if (IsSprinting())
        {
            transform.localScale = originalHeight;
            moveSpeed = sprintSpeed;
        }
    }

    //Check if isGrounded
    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    //Movement
    private void Movement()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

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
