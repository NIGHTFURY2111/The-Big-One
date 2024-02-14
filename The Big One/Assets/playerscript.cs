using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class SC_FPSController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float dashSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed;
    public float lookXLimit = 45.0f;
    public float dashtime;

    private bool DashPressed;
    private bool JumpPressed;
  

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
   
    
    private Controller control;
    private InputAction move;
    private InputAction direction;
    private InputAction dash;
    private InputAction jump;

    private PlayerState PlayerState;
    

    #region input
    private void Awake()
    {
        control = new();
        PlayerState = new();
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
        StartCoroutine(Dashing());
        Debug.Log(moveDirection);

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
        if (PlayerState.canMove)
            rotationX += -LookDir().y * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, LookDir().x * lookSpeed, 0);
        }
    }

    void PlayerMovement()
    {

        float movementDirectionY = moveDirection.y;
        if(PlayerState.canMove)
        {
            moveDirection = MovementVector() * walkingSpeed;
        }
       

        moveDirection.y = (JumpPressed && PlayerState.canMove && characterController.isGrounded) ? jumpSpeed : movementDirectionY;
    }

    Vector2 MoveDir()
    {
        return move.ReadValue<Vector2>();
    }

    Vector2 LookDir()
    {
        return direction.ReadValue<Vector2>();
    }

    Vector3 MovementVector ()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 outp = forward * MoveDir().y + right * MoveDir().x;
        return outp;
        //return canMove? outp:Vector3.zero;
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
        if(PlayerState.canDash() && dash.WasPerformedThisFrame())
        {
            PlayerState.Dashing();

            //moveDirection = MovementVector( ) * dashSpeed;
            moveDirection = ((MoveDir().magnitude==0) ? transform.forward: MovementVector())  * dashSpeed;
            yield return new WaitForSecondsRealtime(dashtime);
            PlayerState.Default();
        }
       
    }
}