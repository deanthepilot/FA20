using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 1000f;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        float jumpFrame = jumpForce * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {            
            rb.AddForce(Vector3.up * jumpFrame);
        }
    }
}
