using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PlayerStateMachine : MonoBehaviour
{
    StateFactory stateFactory;
    BaseState currentState;
    public Text speed;
    public float slideNormalizingTime;
    public float walkNormalizingTime;
    [Header("General Settings")]
    [SerializeField] float gravity;
    [SerializeField] float walkingSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float slideSpeed;
    [SerializeField] float forceAppliedInAir;


    [Header("Dash Settings")]
    [SerializeField] float dashSpeed;
    [SerializeField] float dashtime;

    [Header("Wall Settings")]
    [SerializeField] float wallSlideSpeed;
    [SerializeField] float minWallRunSpeed;
    [SerializeField] float wallRunTime;
    [SerializeField] float wallRunDecay;
    [SerializeField] float maxWallMovingAngle;
    [SerializeField] float minWallMovingAngle;
    [SerializeField] float maxWallLookingAngle;
    [SerializeField] float minWallLookingAngle;

    [Header("Camera Settings")]
    [SerializeField] float lookSpeed;
    [SerializeField] Camera playerCamera;
    [SerializeField] float lookXLimit;

    [Header("Grapple Settings")]
    [SerializeField] GameObject debugGrapplePoint;
    [SerializeField] LineRenderer lineRenderer;


    [HideInInspector]
    [SerializeField] CharacterController characterController;




    public event Action<ControllerColliderHit> Collide;
    private float rotationX = 0;
    private Vector3 slidedir = Vector3.zero;
    private Vector3 moveDirection = Vector3.zero;
    private Controller control;
    private InputAction move;
    private InputAction direction;
    private InputAction dash;
    private InputAction jump;
    private InputAction slide;
    private InputAction grapple;
    private InputAction grappleHold;
    private ControllerColliderHit collision;
    

    //private PlayerState PlayerState;


    #region input
    private void Awake()
    {
        stateFactory = new StateFactory(this);
        characterController = GetComponent<CharacterController>();

        control = new();

        move = control.player.movement;
        dash = control.player.dash;
        jump = control.player.jump;
        slide = control.player.slide;
        direction = control.player.camera;
        grapple = control.player.Grapple;
        grappleHold = control.player.GrappleHold;

    }

    private void OnEnable()
    {
        move.Enable();
        dash.Enable();
        jump.Enable();
        slide.Enable();
        direction.Enable();
        grapple.Enable();
        grappleHold.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        dash.Disable();
        jump.Disable();
        slide.Disable();
        direction.Disable();
        grapple.Disable();
        grappleHold.Disable();
    }
    #endregion

    void Start()
    {
        InputSystem.settings.SetInternalFeatureFlag("DISABLE_SHORTCUT_SUPPORT", true);
        currentState = stateFactory.Idle();
        currentState.EnterState();
        lookSpeed *= 0.5f;

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        //Debug.Log(currentState + " " + moveDirection);
        //moving the player
        //PlayerMovement();
        //Debug.Log(collision);
        // Applying gravity
        Artificialgravity();
        currentState.UpdateState();
        //Debug.Log(currentState);
        // Move the controller

        characterController.Move(moveDirection * Time.deltaTime);
        //displays the current speed
        
        speed.text = Math.Round(characterController.velocity.magnitude).ToString();


        // Player and Camera rotation
        cameraMovement();
    }

    private void LateUpdate()
    {
        currentState.LateUpdateState();
    }

    public void cameraMovement()
    {
        rotationX += -LookDir().y * lookSpeed;

        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        transform.rotation *= Quaternion.Euler(0, LookDir().x * lookSpeed, 0);

    }

    //void PlayerMovement()
    //{
    //    //float movementDirectionY = moveDirection.y;

    //        moveDirection.x = MovementVector().x * walkingSpeed;
    //        moveDirection.z = MovementVector().z * walkingSpeed;


    //    if (jump.WasPressedThisFrame() && PlayerState.canJump())
    //        moveDirection.y = jumpSpeed;
    //    //moveDirection.y = (jump.WasPressedThisFrame() && PlayerState.canJump()) ? jumpSpeed : movementDirectionY;
    //}

    public Vector2 MoveDir()
    {
        return move.ReadValue<Vector2>();
    }

    public Vector2 LookDir()
    {
        return direction.ReadValue<Vector2>();
    }

    public Vector3 MovementVector()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 outp = forward * MoveDir().y + right * MoveDir().x;
        return outp;

    }
    public void Artificialgravity()
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        //this.collision = hit
        Collide?.Invoke(hit);
    }


    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject);
        
    }



    #region getters and setters (DO NOT OPEN IF NOT NECESSARY, BRAINROT GURANTEED)
    public float _gravity {  get => gravity; set => gravity = value; }
    public float _walkingSpeed { get => walkingSpeed; set => walkingSpeed = value; }
    public float _jumpSpeed { get => jumpSpeed;  set => jumpSpeed = value; }
    public float _slideSpeed { get => slideSpeed; set => slideSpeed = value; }
    public float _wallSlideSpeed { get => wallSlideSpeed; set => wallSlideSpeed = value; }
    public float _dashSpeed { get => dashSpeed; set => dashSpeed = value; }
    public float _dashtime { get => dashtime; set => dashtime = value; }
    public float _lookSpeed { get => lookSpeed; set => lookSpeed = value; }
    public float _forceAppliedInAir { get => forceAppliedInAir; set => forceAppliedInAir = value; }

    public Vector3 _moveDirection { get { return moveDirection; } set { moveDirection = value; } }
    public float _moveDirectionX { get => moveDirection.x; set => moveDirection.x = value; }
    public float _moveDirectionY { get => moveDirection.y; set => moveDirection.y = value; }
    public float _moveDirectionZ { get => moveDirection.z; set => moveDirection.z = value; }
    public CharacterController _characterController { get => characterController; set => characterController = value; }
    public InputAction _move { get => move; set => move = value; }
    public InputAction _direction{get=> direction; set => direction = value;}
    public InputAction _dash{get=> dash; set => dash = value;}
    public InputAction _jump{get=> jump; set => jump = value;}
    public InputAction _slide{get=> slide; set => slide = value;}
    public BaseState _currentState { get => currentState; set => currentState = value; }
    public float _wallRunTime { get => wallRunTime; set => wallRunTime = value; }
    public float _wallRunDecay { get => wallRunDecay; set => wallRunDecay = value; }
    public float _maxWallMovingAngle{ get => maxWallMovingAngle; set => maxWallMovingAngle = value; }
    public float _minWallMovingAngle{get=> minWallMovingAngle; set => minWallMovingAngle = value; }
    public float _maxWalllookingAngle{get=> maxWallLookingAngle; set => maxWallLookingAngle = value; }

    public float _minWalllookingAngle{get=> minWallLookingAngle; set => minWallLookingAngle = value; }
    public float _minWallSlideSpeed { get => minWallRunSpeed; }
    public float _magnitude { get => characterController.velocity.magnitude; }     //this is to return the current velocity of the player
    public float _slideNormalizingTime { get => slideNormalizingTime; }
    public Camera _playerCamera { get { return playerCamera; } set { playerCamera = value; } }
    public GameObject _debugGrapplePoint { get { return debugGrapplePoint; } set { debugGrapplePoint = value; } }
    public LineRenderer _lineRenderer { get { return lineRenderer; } set { lineRenderer = value; } }
    public InputAction _grapple { get { return grapple; } set { grapple = value; } }
    public InputAction _grappleHold { get { return grappleHold; } set { grappleHold = value; } }

    //public ControllerColliderHit _collision { get { return collision; } set {  collision = value; } }
    #endregion
}

