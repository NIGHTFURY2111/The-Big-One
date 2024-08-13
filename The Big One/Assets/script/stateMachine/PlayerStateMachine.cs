using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    StateFactory stateFactory;
    BaseState currentState;
<<<<<<<< Updated upstream:The Big One/Assets/script/stateMachine/PlayerStateMachine.cs
========
    public Text speed;
    public float slideNormalizingTime;
    public float walkNormalizingTime;
>>>>>>>> Stashed changes:Assets/script/stateMachine/PlayerStateMachine.cs

    [Header("General Settings")]
    [SerializeField] float gravity;
    [SerializeField] float maxGravity;
    [SerializeField] float walkingSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float jumptime;
    [SerializeField] float slideSpeed;
<<<<<<<< Updated upstream:The Big One/Assets/script/stateMachine/PlayerStateMachine.cs
========
    [SerializeField] float forceAppliedInAir;
    [SerializeField] LayerMask Ground;
>>>>>>>> Stashed changes:Assets/script/stateMachine/PlayerStateMachine.cs


    [Header("Dash Settings")]
    [SerializeField] float dashSpeed;
    [SerializeField] float dashtime;

    [Header("Wall Settings")]
    [SerializeField] float wallSlideSpeed;
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
    PlayerCharacterController PCC;
    Rigidbody rb;
    Collider col;

    float tgtSpeed;





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
<<<<<<<< Updated upstream:The Big One/Assets/script/stateMachine/PlayerStateMachine.cs
        
========

        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        PCC = new PlayerCharacterController(rb, col);
        PCC._setGroundLayer(Ground);
        


>>>>>>>> Stashed changes:Assets/script/stateMachine/PlayerStateMachine.cs
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

        PCC._gravity = gravity;

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {

        // Applying gravity
        //Artificialgravity();

        currentState.UpdateState();
        //Debug.Log(currentState);
        // Move the controller
<<<<<<<< Updated upstream:The Big One/Assets/script/stateMachine/PlayerStateMachine.cs
        characterController.Move(moveDirection * Time.deltaTime);
========
        
        //displays the current speed
        speed.text = Math.Round(PCC._velocityMagnitude).ToString();

        //Debug.Log(PCC._velocityMagnitude);
>>>>>>>> Stashed changes:Assets/script/stateMachine/PlayerStateMachine.cs

        // Player and Camera rotation
        cameraMovement();
    }

    private void FixedUpdate()
    {
        currentState.FixedState();
        Artificialgravity();
        PCC._TGTvelocityDirection = moveDirection;
        PCC.accelrationCheck(tgtSpeed);
       

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
        if (!PCC.isGrounded())
        {
            PCC.ApplyGravity();
        }

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
<<<<<<<< Updated upstream:The Big One/Assets/script/stateMachine/PlayerStateMachine.cs
        //Debug.Log(collision);
        //this.collision = hit;
        Collide?.Invoke(hit);
        
        
========
        //this.collision = hit
        Collide?.Invoke(hit);
    }


    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject);
>>>>>>>> Stashed changes:Assets/script/stateMachine/PlayerStateMachine.cs
    }

    #region getters and setters (DO NOT OPEN IF NOT NECESSARY, BRAINROT GURANTEED)
<<<<<<<< Updated upstream:The Big One/Assets/script/stateMachine/PlayerStateMachine.cs
    public float _gravity {  get {return gravity; }set { gravity = value; } }
    public float _walkingSpeed { get { return walkingSpeed; } set { walkingSpeed = value; } }
    public float _jumpSpeed { get { return jumpSpeed; } set { jumpSpeed = value; } }
    public float _slideSpeed { get { return slideSpeed; } set { slideSpeed = value; } }
    public float _wallSlideSpeed { get { return wallSlideSpeed; } set { wallSlideSpeed = value; } }
    public float _dashSpeed { get { return dashSpeed; } set { dashSpeed = value; } }
    public float _dashtime { get { return dashtime; } set { dashtime = value; } }
    public float _lookSpeed { get { return lookSpeed; } set { lookSpeed= value; } }
========
    public float _gravity {  get => gravity; set => gravity = value; }
    public float _walkingSpeed { get => walkingSpeed; set => walkingSpeed = value; }
    public float _jumpSpeed { get => jumpSpeed;  set => jumpSpeed = value; }
    public float _jumptime { get => jumptime; set => jumptime = value; }
    public float _slideSpeed { get => slideSpeed; set => slideSpeed = value; }
    public float _wallSlideSpeed { get => wallSlideSpeed; set => wallSlideSpeed = value; }
    public float _dashSpeed { get => dashSpeed; set => dashSpeed = value; }
    public float _dashtime { get => dashtime; set => dashtime = value; }
    public float _lookSpeed { get => lookSpeed; set => lookSpeed = value; }
    public float _forceAppliedInAir { get => forceAppliedInAir; set => forceAppliedInAir = value; }

>>>>>>>> Stashed changes:Assets/script/stateMachine/PlayerStateMachine.cs
    public Vector3 _moveDirection { get { return moveDirection; } set { moveDirection = value; } }
    public float _moveDirectionX { get { return moveDirection.x; } set { moveDirection.x = value; } }
    public float _moveDirectionY { get { return moveDirection.y; } set { moveDirection.y = value; } }
    public float _moveDirectionZ { get { return moveDirection.z; } set { moveDirection.z = value; } }
    public CharacterController _characterController { get { return characterController; } set { characterController = value; } }
    public InputAction _move { get { return move; } set { move= value; } }
    public InputAction _direction{get{return direction;} set{direction = value;}}
    public InputAction _dash{get{return dash;} set{ dash= value;}}
    public InputAction _jump{get{return jump;} set{ jump = value;}}
    public InputAction _slide{get{return slide;} set{slide = value;}}
    public InputAction _grapple { get { return grapple; } set { grapple = value; } }
    public InputAction _grappleHold { get { return grappleHold; } set { grappleHold = value; } }    
    public BaseState _currentState { get { return currentState; } set { currentState = value; } }
    public float _wallRunTime { get { return wallRunTime; } set { wallRunTime = value; } }
    public float _wallRunDecay { get { return wallRunDecay; } set { wallRunDecay = value; } }
    public float _maxWallMovingAngle{ get { return maxWallMovingAngle; } set { maxWallMovingAngle = value; } }
    public float _minWallMovingAngle{get{return minWallMovingAngle;} set { minWallMovingAngle = value; } }
    public float _maxWalllookingAngle{get{return maxWallLookingAngle;} set { maxWallLookingAngle = value; } }
    public float _minWalllookingAngle{get{return minWallLookingAngle;} set { minWallLookingAngle = value; } }
    public Camera _playerCamera { get { return playerCamera; } set { playerCamera = value; } }
    public GameObject _debugGrapplePoint { get { return debugGrapplePoint; } set { debugGrapplePoint = value; } }
    public LineRenderer _lineRenderer { get { return lineRenderer; } set { lineRenderer = value; } }
<<<<<<<< Updated upstream:The Big One/Assets/script/stateMachine/PlayerStateMachine.cs
========
    public InputAction _grapple { get { return grapple; } set { grapple = value; } }
    public InputAction _grappleHold { get { return grappleHold; } set { grappleHold = value; } }
    public PlayerCharacterController _getPCC { get { return PCC; } }
    public float _TGTSpeed { get { return tgtSpeed; } set { tgtSpeed = value; } }

>>>>>>>> Stashed changes:Assets/script/stateMachine/PlayerStateMachine.cs
    //public ControllerColliderHit _collision { get { return collision; } set {  collision = value; } }

    #endregion
}