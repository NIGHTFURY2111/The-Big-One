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

    Vector3 TGTvelocityDirection;
    LayerMask Ground;

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

    public override void Move()
    {
        rb.AddForce(TGTvelocityDirection.normalized * acceleration, ForceMode.VelocityChange);
        //rb.velocity = TGTvelocityDirection.normalized*TGTvelocityMagnitude;
    }

    public void AirMove(float airMoveForce)
    {
        rb.AddForce(TGTvelocityDirection.normalized * _currentVelocityMagnitude * airMoveForce, ForceMode.Acceleration);
    }   

    public void JumpForce(float jumpForce)
    {
        float upForce = Mathf.Clamp(jumpForce - rb.velocity.y, 0, Mathf.Infinity);  
        rb.AddForce(new Vector3(0, upForce, 0), ForceMode.VelocityChange);
        //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
    public Vector3 _getvelocityVector { get { return rb.velocity; } }
    public Vector3 _setvelocityVector { set { rb.velocity = value; } }
    public bool _useGravity { get { return useGravity; } }
    public void _setGroundLayer(LayerMask lm) {  Ground = lm;  }
    #endregion

    public float _TGTvelocityMagnitude { get { return rb.velocity.magnitude; } set { TGTvelocityMagnitude = value; } }
    public Vector3 _TGTvelocityDirection { get { return rb.velocity; } set { TGTvelocityDirection = value; } }
    public float _acceleration { get { return rb.velocity.magnitude; } set { TGTvelocityMagnitude = value; } }
    public float _drag { get { return rb.drag; } set { rb.drag = value; } }
    public float _angulardrag { get { return rb.angularDrag; } set { rb.angularDrag = value; } }
    public float _gravity { get { return gravity; } set { gravity = value; } }
    public float _TGTvelvocity{ get { return TGTvelocityMagnitude; } }
    #endregion
}