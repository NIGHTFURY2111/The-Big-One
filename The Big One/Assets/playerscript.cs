using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class SC_FPSController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed;
    public float lookXLimit = 45.0f;

    private bool DashPressed;
    private bool JumpPressed;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;
    
    private Controller control;
    private InputAction move;
    private InputAction direction;
    private InputAction dash;
    private InputAction jump;

    #region input
    private void Awake()
    {
        control = new Controller();
        move = control.player.movement;
        direction = control.player.camera;
        dash = control.player.dash;
        jump = control.player.jump;
        
    }

    private void OnEnable()
    {
        move.Enable();
        direction.Enable();
        dash.Enable();
        jump.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        direction.Disable();
        dash.Disable();
        jump.Disable();
    }
    #endregion

    void Start()
        {
            lookSpeed *=0.5f ;
            characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        DashPressed = dash.WasPressedThisFrame();
        JumpPressed = jump.WasPressedThisFrame();


        //moving the player
        PlayerMovement();
        // Applying gravity
        Artificialgravity();

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        cameraMovement();
    }
    void cameraMovement()
    {
        { 
        if (canMove)
            rotationX += -LookDir().y * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, LookDir().x * lookSpeed, 0);
        }
    }

    void PlayerMovement()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);


        // Press Left Shift to run


        float curSpeedX = canMove ? walkingSpeed * MoveDir().y : 0;
        float curSpeedY = canMove ? walkingSpeed * MoveDir().x : 0;

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);


        moveDirection.y = (JumpPressed && canMove && characterController.isGrounded) ? jumpSpeed : movementDirectionY;
    }

    Vector2 MoveDir()
    {
        return move.ReadValue<Vector2>();
    }
    Vector2 LookDir()
    {
        return direction.ReadValue<Vector2>();
    }
    void Artificialgravity()
    {
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        else
        {
            moveDirection.y = Math.Clamp(moveDirection.y, -2, int.MaxValue);
        }
    }
}