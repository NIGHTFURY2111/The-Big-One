using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    Vector3 pivot;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        pivot = transform.position;
        pivot.y = transform.position.y + transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(pivot, Vector3.forward, ((Mathf.Floor(Mathf.Sin(Time.time * 20f)) * 2 )+ 1));
       
    }
}
