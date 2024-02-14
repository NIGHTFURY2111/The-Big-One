public class PlayerState {

    public bool canMove = true;
    public bool isMoving = false;
    public bool isDashing = false;
    public bool isjumping = false;
    public void Walking()
    {
        this.isMoving = true;
        this.isDashing = false;
        this.isjumping = false;
        this.canMove = true;
    }
    public void Dashing() 
    {
        this.canMove = false;
        this.isDashing = true;
    }
    public void Default()
    {
        isMoving = false;
        isDashing = false;
        isjumping = false;

        canMove = true;
    }
    public bool canDash()
    {
        return !this.isDashing && this.canMove;
    }
}