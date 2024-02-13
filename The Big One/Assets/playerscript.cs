using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerscript : MonoBehaviour
{
    // Start is called before the first frame update
    
    private Controller control;
    private InputAction move;
    private InputAction direction;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private float lookingSpeed;
    [SerializeField] private Camera cam;

    #region input
    private void Awake()
    {
        control = new Controller();
        move = control.player.movememnt;
        direction = control.player.camera;
    }

    private void OnEnable()
    {
        move.Enable();
        direction.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        direction.Disable();
    }
    #endregion
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
        Looking();
        cam.transform.position = transform.position;
    }

    Vector2 MoveDir()
    {
        return move.ReadValue<Vector2>();
    }
    Vector2 LookDir()
    {
        return direction.ReadValue<Vector2>();
    }

    void Moving()
    {
        rb.velocity = new Vector3(MoveDir().x*speed,0,MoveDir().y*speed);
    }
    void Looking()
    {
        rb.rotation = Quaternion.Euler(0,Mathf.Atan2(LookDir().y,LookDir().x)*Mathf.Rad2Deg-45,0);
        cam.transform.rotation = Quaternion.Euler(Mathf.Atan2(LookDir().x, LookDir().y ) * Mathf.Rad2Deg-45, Mathf.Atan2(LookDir().y, LookDir().x) * Mathf.Rad2Deg-45, 0);
        
    }
    
}
