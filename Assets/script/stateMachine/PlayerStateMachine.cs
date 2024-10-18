using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerStateMachine : MonoBehaviour
{
    StateFactory stateFactory;
    BaseState currentState;
    //public Text speed;
    public float slideNormalizingTime;
    public float walkNormalizingTime;

    [Header("General Settings")]
    [SerializeField] StateEnum enumum;
    [SerializeField] float gravity;
    [SerializeField] float maxGravity;
    [SerializeField] float walkingSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float jumptime;
    [SerializeField] float slideSpeed;
    [SerializeField] float forceAppliedInAir;
    [SerializeField] float maxVelocityAddedInAir;
    [SerializeField] float moveDragDebug;
    [SerializeField] float idleDragDebug;
    [SerializeField] LayerMask Ground;


    [Header("Dash Settings")]
    [SerializeField] float dashSpeed;
    [SerializeField] float dashtime;

    [Header("Wall Settings")]
    [SerializeField] float wallSlideSpeed;
    [SerializeField] float wallRunTime;
    [SerializeField] float wallRunDecay;
    [SerializeField] float wallRunRaycastAngle;
    [SerializeField] float maxWallRaycastDistance;
    [SerializeField] float minWallRunSpeedReq;
    [SerializeField] LayerMask Wall;

    [Header("Camera Settings")]
    [SerializeField] float lookSpeed;
    [SerializeField] Camera playerCamera;
    [SerializeField] float lookXLimit;

    [Header("Grapple Settings")]
    [SerializeField] GameObject debugGrapplePoint;
    [SerializeField] LineRenderer lineRenderer;

    [HideInInspector]
    //[SerializeField] CharacterController characterController;
    PlayerCharacterController PCC;
    Rigidbody rb;
    Collider col;


    float tgtSpeed;
    bool isGrounded;
    bool isWall;
    Vector3 wallNormal;
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

    public void OnMove(InputValue action)
    {
        Vector2 newee = action.Get<Vector2>();
    }

    #region input
    private void Awake()
    {
        stateFactory = new StateFactory(this);
        //characterController = GetComponent<CharacterController>();


        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        PCC = new PlayerCharacterController(rb, col);
        PCC._setGroundLayer(Ground);
        PCC._setWallLayer(Wall);



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
        //grapple.Enable();
        //grappleHold.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        dash.Disable();
        jump.Disable();
        slide.Disable();
        direction.Disable();
        //grapple.Disable();
        //grappleHold.Disable();
    }
    #endregion

    void Start()
    {
        StateEnum temp = StateEnum.move | StateEnum.dash;

        if (enumum.HasFlag(temp))
        {
            Debug.Log(true);
        }
        temp = temp | StateEnum.fall;



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

        isGrounded = _getPCC.isGrounded();
        //Debug.Log(PCC._velocityMagnitude);
        //Debug.Log(currentState);
        //characterController.Move(moveDirection * Time.deltaTime);

        // Move the controller
        currentState.UpdateState();

        // Player and Camera rotation
        cameraMovement();

        //displays the current speed
        //speed.text = Math.Round(PCC._currentVelocityMagnitude).ToString();



    }

    private void FixedUpdate()
    {
        currentState.FixedState();

        // Applying gravity
        Artificialgravity();

        PCC._TGTvelocityDirection = moveDirection;
        PCC.AccelrationCheck(tgtSpeed);
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
        if (!isGrounded && !isWall)
            PCC.ApplyGravity();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log(collision);
        //this.collision = hit;
        Collide?.Invoke(hit);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            // gameobject.layer takes the specific layer number of the desired layer to be checked, in this case wall is at 7th position in the editor, so it is 7
            // alternative is wall.layer == 1 << collision.gameobject.layer , which essentially is just reverse bit shifting by 7 to check if it is the wall layer
            if (collision.gameObject.layer == 7) 
            {
                wallNormal = collision.contacts[0].normal;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject);
    }

    #region getters and setters (DO NOT OPEN IF NOT NECESSARY, BRAINROT GURANTEED)
    public float _gravity { get => gravity; set => gravity = value; }
    public float _walkingSpeed { get => walkingSpeed; set => walkingSpeed = value; }
    public float _jumpSpeed { get => jumpSpeed; set => jumpSpeed = value; }
    public float _jumptime { get => jumptime; set => jumptime = value; }
    public float _slideSpeed { get => slideSpeed; set => slideSpeed = value; }
    public float _wallSlideSpeed { get => wallSlideSpeed; set => wallSlideSpeed = value; }
    public float _dashSpeed { get => dashSpeed; set => dashSpeed = value; }
    public float _dashtime { get => dashtime; set => dashtime = value; }
    //public float _lookSpeed { get => lookSpeed; set => lookSpeed = value; }
    public float _forceAppliedInAir { get => forceAppliedInAir; set => forceAppliedInAir = value; }

    public float _moveDirectionX { get { return moveDirection.x; } set { moveDirection.x = value; } }
    public float _moveDirectionY { get { return moveDirection.y; } set { moveDirection.y = value; } }
    public float _moveDirectionZ { get { return moveDirection.z; } set { moveDirection.z = value; } }
    public Vector3 _moveDirection { get { return moveDirection; } set { moveDirection = value; } }

    //public CharacterController _characterController { get { return characterController; } set { characterController = value; } }
    public InputAction _move { get { return move; } set { move = value; } }
    //public InputAction _direction{get{return direction;} set{direction = value;}}
    public InputAction _dash { get { return dash; } set { dash = value; } }
    public InputAction _jump { get { return jump; } set { jump = value; } }
    public InputAction _slide { get { return slide; } set { slide = value; } }
    public BaseState _currentState { get { return currentState; } set { currentState = value; } }
    public float _wallRunTime { get { return wallRunTime; } set { wallRunTime = value; } }
    public float _wallRunDecay { get { return wallRunDecay; } set { wallRunDecay = value; } }
    public Camera _playerCamera { get { return playerCamera; } set { playerCamera = value; } }
    public GameObject _debugGrapplePoint { get { return debugGrapplePoint; } set { debugGrapplePoint = value; } }
    public LineRenderer _lineRenderer { get { return lineRenderer; } set { lineRenderer = value; } }
    public InputAction _grapple { get { return grapple; } set { grapple = value; } }
    public InputAction _grappleHold { get { return grappleHold; } set { grappleHold = value; } }
    public PlayerCharacterController _getPCC { get { return PCC; } }
    public float _TGTSpeed { get { return tgtSpeed; } set { tgtSpeed = value; } }

    public bool _isGrounded { get => isGrounded; }

    public float _getMoveDragDebug { get { return moveDragDebug; } }
    public float _getIdleDragDebug { get { return idleDragDebug; } }

    public float _getMaxVelocityInAir { get { return maxVelocityAddedInAir; } }

    public float _getWallRunAngle { get => wallRunRaycastAngle; }

    public float _getWallRunRaycastDistance { get => maxWallRaycastDistance; }

    public bool _getisWall { get { return isWall; } set { isWall = value; } }

    public float _getWallRunReqSpeed { get { return minWallRunSpeedReq; } }

    public Vector3 _getWallNormal { get { return wallNormal; } }

    //public ControllerColliderHit _collision { get { return collision; } set {  collision = value; } }

    #endregion
}

