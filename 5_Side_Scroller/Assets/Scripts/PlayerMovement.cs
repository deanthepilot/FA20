using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 1500f;
    Rigidbody rb;
    public bool isGrounded;
    public float speed = 10;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void OnCollisionStay()
    {
        isGrounded = true;
    }

    void OnCollisionExit()
    {
        isGrounded = false;
    }
    void Update()
    {
        PlayerJump();
        PlayerMove();
    }

    void PlayerMove()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.transform.Translate(Vector3.left * Time.deltaTime * speed, Space.Self); //LEFT
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.transform.Translate(Vector3.right * Time.deltaTime * speed, Space.Self); //RIGHT
        }
    }

    void PlayerJump()
    {
        float jumpFrame = jumpForce * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpFrame, ForceMode.Impulse);
            isGrounded = false;
        }
    }
}
