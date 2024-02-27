using UnityEngine;

public class PlayerState 
{
    public bool isMoving, isDashing, isjumping, canMove, isSliding;
    private PlayerScript player;

    public PlayerState(PlayerScript player)
    {
        this.player = player;
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
        isSliding = false;

        canMove = true;
    }

    public void Dashing() 
    {
        this.canMove = false;
        this.isDashing = true;
    }

    public void Sliding() 
    {
        //this.canMove =false;
        this.isSliding = true;

    }

    public bool canDash()
    {
        return !this.isDashing && this.canMove;
    }

    public bool canJump()
    {
        return this.canMove && player.characterController.isGrounded;
    }
    public bool canSlide()
    {
        return player.characterController.isGrounded;
        //return true;
    }
}