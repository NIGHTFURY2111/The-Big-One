using UnityEngine;

public class PlayerState:MonoBehaviour 
{
    public bool  isMoving, isDashing, isjumping, canMove;
    private PlayerScript player;

    void Start()
    {
        player = GetComponent<PlayerScript>();
        Default();
    }

    public void Walking()
    {
        this.isMoving = true;
        this.isDashing = false;
        this.isjumping = false;
        this.canMove = true;
    }
    public void Default()
    {
        isMoving = false;
        isDashing = false;
        isjumping = false;

        canMove = true;
    }

    public void Dashing() 
    {
        this.canMove = false;
        this.isDashing = true;
    }
    public bool canDash()
    {
        return !this.isDashing && this.canMove;
    }


    public bool canJump()
    {
        return this.canMove && player.characterController.isGrounded;
    }
}