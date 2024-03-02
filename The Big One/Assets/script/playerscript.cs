using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerScript : MonoBehaviour
{
    [Header("General Settings")]
    public float gravity;
    public float walkingSpeed;
    public float jumpSpeed;
    public float slideSpeed;


    [Header("Dash Settings")]
    public float dashSpeed;
    public float dashtime;


    [Header("Camera Settings")]
    public Camera playerCamera;
    public float lookSpeed;
    public float lookXLimit;


    [HideInInspector]
    public CharacterController characterController;




    private float rotationX = 0;
    private Vector3 slidedir = Vector3.zero;
    private Vector3 moveDirection = Vector3.zero;
    private Controller control;
    private InputAction move;
    private InputAction direction;
    private InputAction dash;
    private InputAction jump;
    private InputAction slide;

    private PlayerState PlayerState;


    #region input
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        control = new();

        move = control.player.movement;
        dash = control.player.dash;
        jump = control.player.jump;
        slide = control.player.slide;
        direction = control.player.camera;

    }

    private void OnEnable()
    {
        move.Enable();
        dash.Enable();
        jump.Enable();
        slide.Enable();
        direction.Enable();

    }

    private void OnDisable()
    {
        move.Disable();
        dash.Disable();
        jump.Disable();
        slide.Disable();
        direction.Disable();
    }
    #endregion

    void Start()
    {
        lookSpeed *= 0.5f;
        PlayerState = new PlayerState(this);

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //Dashing player
        StartCoroutine(Dashing());
        //Debug.Log(PlayerState.canJump());

        //moving the player
        PlayerMovement();
        sliding();

        // Applying gravity
        Artificialgravity();

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        cameraMovement();
    }
    void cameraMovement()
    {

        if (PlayerState.canMove)
            rotationX += -LookDir().y * lookSpeed;

        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);



        transform.rotation *= Quaternion.Euler(0, LookDir().x * lookSpeed, 0);

    }

    void PlayerMovement()
    {
        //float movementDirectionY = moveDirection.y;
        if (PlayerState.canMove)
        {
            moveDirection.x = MovementVector().x * walkingSpeed;
            moveDirection.z = MovementVector().z * walkingSpeed;
        }

        if (jump.WasPressedThisFrame() && PlayerState.canJump())
            moveDirection.y = jumpSpeed;
        //moveDirection.y = (jump.WasPressedThisFrame() && PlayerState.canJump()) ? jumpSpeed : movementDirectionY;
    }

    Vector2 MoveDir()
    {
        return move.ReadValue<Vector2>();
    }

    Vector2 LookDir()
    {
        return direction.ReadValue<Vector2>();
    }

    Vector3 MovementVector()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 outp = forward * MoveDir().y + right * MoveDir().x;
        return outp;

    }

    void sliding()
    {
        if (PlayerState.canSlide() && slide.WasPerformedThisFrame())
        {
            PlayerState.Sliding();
            transform.localScale = new Vector3(1, 0.5f, 1);
            slidedir = ((MoveDir().magnitude == 0) ? transform.forward : MovementVector());
        }
        else if (slide.WasReleasedThisFrame())
        {
            PlayerState.Default();
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (PlayerState.isSliding)
        {

            moveDirection.x = slidedir.x * slideSpeed;
            moveDirection.z = slidedir.z * slideSpeed;

            if (jump.WasPressedThisFrame() && PlayerState.canJump())
                moveDirection.y = jumpSpeed;
        }


    }
    void Artificialgravity()
    {
        if (!characterController.isGrounded && PlayerState.canMove)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        else
        {
            moveDirection.y = Math.Clamp(moveDirection.y, -2, int.MaxValue);
        }
    }

    IEnumerator Dashing()
    {
        if (PlayerState.canDash() && dash.WasPerformedThisFrame())
        {
            PlayerState.Dashing();

            //moveDirection = MovementVector( ) * dashSpeed;
            moveDirection = ((MoveDir().magnitude == 0) ? transform.forward : MovementVector()) * dashSpeed;
            yield return new WaitForSecondsRealtime(dashtime);
            PlayerState.Default();
        }
    }
}