using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacterController : AbstractCharacterController
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Collider playerCharacterController;

    //float maxVelocity;
    //float drag;
    //float angularDrag;
    //float lVelocity;

    float acceleration;
    float gravity;
    float playerHeight = 2f;
    float TGTvelocityMagnitude;
    bool useGravity;
    bool isgrounded;
    bool isWall;

    Vector3 TGTvelocityDirection;
    LayerMask Ground;
    LayerMask Wall;

    public PlayerCharacterController(Rigidbody rb,Collider col)
    {
        this.rb = rb;
        this.playerCharacterController = col;
        //rb.drag = 10f;
    }

    public void SetMaxlinVel(float TgtMagnitude) 
    {
        if (TgtMagnitude != 0)
        {
            rb.maxLinearVelocity = TgtMagnitude;
        }
    }

    public void AccelrationCheck(float TgtMagnitude)
    {
        if (TgtMagnitude == 0)
        {
            acceleration = 0;
        }
    }

    public void calculateAccelration(float TgtMagnitude)
    {
        TGTvelocityMagnitude = TgtMagnitude;
        //float factor = (TgtMagnitude / _currentVelocityMagnitude);
       

            acceleration += (TgtMagnitude - _currentVelocityMagnitude);
        
        //else
        //{

        //    acceleration *= factor;
        //    Debug.Log("1");
        //}
    }

    public override void Move(float OverrideMagnitude =1f)
    {
        //rb.AddForce(TGTvelocityDirection.normalized * TGTvelocityMagnitude, ForceMode.VelocityChange); 
        rb.velocity = new Vector3(TGTvelocityDirection.normalized.x, 0, TGTvelocityDirection.normalized.z) * TGTvelocityMagnitude * OverrideMagnitude;   
        //rb.velocity = TGTvelocityDirection.normalized*TGTvelocityMagnitude;
    }

    public void AirMoveForceLimit(float velocityAtInstance,float airMoveForceLimit)
    {
        Vector2 test1 = new Vector2(rb.velocity.x, rb.velocity.z);
        float test2 = Mathf.Clamp(test1.magnitude, 0, velocityAtInstance + airMoveForceLimit);
        //float test2 = test1.magnitude;
        //test2 *= test1.magnitude/(velocityAtInstance + airMoveForceLimit);
        rb.velocity = new Vector3(test1.normalized.x * test2, rb.velocity.y, test1.normalized.y * test2);
    }

  

    public void AirMove(float airMoveForce)
    {
        rb.AddForce(TGTvelocityDirection.normalized *  airMoveForce, ForceMode.Acceleration);
    }   

    public void JumpForce(float jumpForce)
    {
        float upForce = Mathf.Clamp(jumpForce - rb.velocity.y, 0, Mathf.Infinity);  
        rb.AddForce(new Vector3(0, upForce, 0), ForceMode.VelocityChange);
        //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void WallJumpForce(Vector3 Dir, float jumpForce)
    {
        //float upForce = Mathf.Clamp(jumpForce - rb.velocity.y, 0, Mathf.Infinity);
        Debug.Log(Dir * jumpForce);
        rb.AddForce(Dir*jumpForce, ForceMode.VelocityChange);
    }

    public float SetCurrentVelocity(float defaultStateVel)
    {
        return (_currentVelocityMagnitude <= defaultStateVel) ? defaultStateVel : _currentVelocityMagnitude + defaultStateVel; 
    }
    public void ApplyGravity()
    {
        rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
    }

    public override bool isGrounded()
    {
        isgrounded = Physics.Raycast(rb.transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);
        return isgrounded;
    }

    RaycastHit hit;

    public bool WallRunCheckRight(float angleToRotateBy,float rayCastDistance)
    {
        Vector3 rightVectorUpper = Quaternion.AngleAxis(angleToRotateBy, rb.transform.up) * rb.transform.right;
        Vector3 rightVectorLower = Quaternion.AngleAxis(-angleToRotateBy, rb.transform.up) * rb.transform.right;
        isWall = Physics.Raycast(rb.transform.position, rightVectorUpper, out hit, rayCastDistance,Wall) || Physics.Raycast(rb.transform.position, rightVectorLower, out hit, rayCastDistance, Wall);
        
        return isWall;
    }

    public bool WallRunCheckLeft(float angleToRotateBy, float rayCastDistance)
    {

        Vector3 leftVectorUpper = Quaternion.AngleAxis(angleToRotateBy+180, rb.transform.up) * rb.transform.right;
        Vector3 leftVectorLower = Quaternion.AngleAxis(-(angleToRotateBy+180), rb.transform.up) * rb.transform.right;
        isWall = Physics.Raycast(rb.transform.position, leftVectorUpper,out hit, rayCastDistance, Wall) || Physics.Raycast(rb.transform.position, leftVectorLower,out hit, rayCastDistance, Wall);  

        return isWall;
    }

    public void SetvelocityMagnitudeasZero()
    { 
        rb.velocity = Vector3.zero; 
    }

    public float GetCurrentHorizontal()
    {
        return new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
    }
    public float GetCurrentVertical()
    { 
        return rb.velocity.y;
    }


    #region Character Movement Variables
    #region script variables
    public float _currentVelocityMagnitude { get { return rb.velocity.magnitude; } }
    public float _currentHorizontalVelocityMagnitude { get => new Vector2(rb.velocity.x, rb.velocity.z).magnitude; }
    public Vector3 _getvelocityVector { get { return rb.velocity; } }
    public Vector3 _setvelocityVector { set { rb.velocity = value; } }
    public bool _useGravity { get { return useGravity; } }
    public void _setGroundLayer(LayerMask lm) {  Ground = lm;  }
    public void _setWallLayer(LayerMask lm) { Wall = lm; }
    #endregion

    public float _TGTvelocityMagnitude { get { return rb.velocity.magnitude; } set { TGTvelocityMagnitude = value; } }
    public Vector3 _TGTvelocityDirection { get { return rb.velocity; } set { TGTvelocityDirection = value; } }
    public float _acceleration { get { return rb.velocity.magnitude; } set { TGTvelocityMagnitude = value; } }
    public float _drag { get { return rb.drag; } set { rb.drag = value; } }
    public float _angulardrag { get { return rb.angularDrag; } set { rb.angularDrag = value; } }
    public float _gravity { get { return gravity; } set { gravity = value; } }
    public float _TGTvelvocity{ get { return TGTvelocityMagnitude; } }

    public Rigidbody _rb { get { return rb; } }

    
    #endregion
}