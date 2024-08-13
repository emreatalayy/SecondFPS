using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    public float speed = 5.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 3.0f;

    private bool crouching = false;
    private bool sprinting = false;
    private bool lerpCrouch = false;
    private float crouchTimer = 0f;
    public float crouchScale = 0.5f;  // Çömelme sırasında oyuncunun boyutu
    private Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalScale = transform.localScale;  // Oyuncunun başlangıç boyutunu sakla
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            if (crouching)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, originalScale * crouchScale, p);
            }
            else
            {
                transform.localScale = Vector3.Lerp(transform.localScale, originalScale, p);
            }

            if (p >= 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;

        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void StartCrouch()
    {
        crouching = true;
        crouchTimer = 0f;
        lerpCrouch = true;
    }

    public void StopCrouch()
    {
        crouching = false;
        crouchTimer = 0f;
        lerpCrouch = true;
    }

    public void Sprint()
    {
        sprinting = !sprinting;
        if (sprinting)
        {
            speed = 8;
        }
        else
        {
            speed = 5;
        }
    }
}
