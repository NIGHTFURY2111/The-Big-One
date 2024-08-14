using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacterController : AbstractCharacterController
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Collider playerCharacterController;

    float TGTvelocityMagnitude;
    Vector3 TGTvelocityDirection;
    float acceleration;
    float maxVelocity;
    float drag;
    float angularDrag;
    float lVelocity;
    float gravity;

    bool useGravity;

    float playerHeight = 2f;
    LayerMask Ground;
    bool isgrounded;


    public PlayerCharacterController(Rigidbody rb,Collider col)
    {
        this.rb = rb;
        this.playerCharacterController = col;
        rb.drag = 10f;
    }

    public void accelrationCheck(float TgtMagnitude)
    {
        if (TgtMagnitude == 0)
        {
            acceleration = 0;
        }
    }

    public void setmaxlinvel(float TgtMagnitude) 
    {
        if (TgtMagnitude != 0)
        {
            rb.maxLinearVelocity = TgtMagnitude;
        }
    }

    public void calculateAccelration(float TgtMagnitude)
    {
        acceleration += (TgtMagnitude - _velocityMagnitude);
    }

    public override void move()
    {
        rb.AddForce(TGTvelocityDirection.normalized * acceleration, ForceMode.VelocityChange);
        //rb.velocity = TGTvelocityDirection.normalized*TGTvelocityMagnitude;
    }

    public void ApplyGravity()
    {
        rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
    }
        
    public void dashMove()
    {
        rb.AddForce(TGTvelocityDirection.normalized * acceleration, ForceMode.VelocityChange);
    }


    public void airMove(float airMoveForce)
    {
        rb.AddForce(TGTvelocityDirection.normalized * _velocityMagnitude * airMoveForce, ForceMode.Acceleration);
    }   

    public void jumpForce(float jumpForce)
    {
        float upForce = Mathf.Clamp(jumpForce - rb.velocity.y, 0, Mathf.Infinity);  
        rb.AddForce(new Vector3(0, upForce, 0), ForceMode.VelocityChange);
        //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    public override bool isGrounded()
    {
        isgrounded = Physics.Raycast(rb.transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);
        return isgrounded;
    }

    public float setCurrentVelocity(float stateVel)
    {
        return (_velocityMagnitude <= stateVel ? stateVel : _velocityMagnitude + stateVel); 
    }

    public void setvelocityMagnitudeasZero()
    { 
        rb.velocity = Vector3.zero; 
    }

    public float _velocityMagnitude { get { return rb.velocity.magnitude; } }
    
    public Vector3 _getvelocityVector { get { return rb.velocity; } }
    public Vector3 _setvelocityVector { set { rb.velocity = value; } }
    public bool _useGravity { get { return useGravity; } }
    public void _setGroundLayer(LayerMask lm) {  Ground = lm;  }



    #region Character Movement Variables
    public float _TGTvelocityMagnitude { get { return rb.velocity.magnitude; } set { TGTvelocityMagnitude = value; } }
    public Vector3 _TGTvelocityDirection { get { return rb.velocity; } set { TGTvelocityDirection = value; } }
    public float _acceleration { get { return rb.velocity.magnitude; } set { TGTvelocityMagnitude = value; } }
    public float _drag { get { return rb.drag; } set { rb.drag = value; } }
    public float _angulardrag { get { return rb.angularDrag; } set { rb.angularDrag = value; } }
    public float _gravity { get { return gravity; } set { gravity = value; } }

    
    #endregion
}