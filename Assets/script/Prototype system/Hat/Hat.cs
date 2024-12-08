using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour
{
    [SerializeField] GameObject hat;
    [SerializeField] GameObject gunSpawm;
    [SerializeField] float throwSpeed;
    [SerializeField] bool HatOut;

    GameObject hatcurr = null;
    PlayerStateMachine psm;

    private void Awake()
    {
        psm = GetComponent<PlayerStateMachine>();
    }

    public void ThrowStatic()
    {
        if (!HatOut)
        {
            hatcurr = Instantiate(hat, gunSpawm.transform.position, Quaternion.identity);

            Rigidbody rb = hatcurr.GetComponent<Rigidbody>();

            rb.drag = 5f;
            rb.AddForce(Camera.main.transform.forward * throwSpeed, ForceMode.VelocityChange);

            HatOut = true;
        }
        else {
            Destroy(hatcurr);
            hatcurr = null;
            HatOut=false;
        }


    }
    
    public void ThrowDynamic()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == hatcurr)
        {
            psm._getPCC.JumpForce(psm._jumpSpeed);
        }
    }
}
