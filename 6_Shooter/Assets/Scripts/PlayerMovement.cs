using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //public GameObject cameraObject;
    public Camera cameraObject;
    public float acceleration = 3f;
    public float walkAccelerationRatio = 3f;

    public float maxWalkSpeed = 2f;
    public float deaccelerate = 20f;
    [HideInInspector]
    public Vector2 horizontalMovement;

    [HideInInspector]
    public float walkDeaccelerateX;
    [HideInInspector]
    public float walkDeaccelerateZ;

    [HideInInspector]
    public bool isGrounded = true;
    Rigidbody playerRigidBody;
    public float jumpVelocity = 10f;
    float maxSlope = 45;

    private void Awake()
    {
        // Getting the rigidBody component from player
        playerRigidBody = GetComponent<Rigidbody>();
        cameraObject = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Move();
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) // todo virtualize input
        {
            playerRigidBody.AddForce(0, jumpVelocity, 0);
        }
    }

    private void Move()
    {
        // Controlling the limit of player by  measuring the Vector 3 Magnitude and then
        // measuring and normalizing that vector

        horizontalMovement = new Vector2(playerRigidBody.velocity.x,
                                         playerRigidBody.velocity.z);

        if (horizontalMovement.magnitude > maxWalkSpeed)
        {
            horizontalMovement = horizontalMovement.normalized;
            horizontalMovement *= maxWalkSpeed;
        }
        // Controlling only the X and Z speed of the cube movement
        playerRigidBody.velocity = new Vector3(horizontalMovement.x,
                                               playerRigidBody.velocity.y,
                                               horizontalMovement.y);

        // rotating the player capsule according to the MouseLook current Y variable, 
        // so that player looks exactly where camera is looking
        transform.rotation = Quaternion.Euler(0, cameraObject.GetComponent<MouseLook>().currentY, 0);
        // Moving here 
        if (isGrounded) // complete control while player is on the ground
        {
            playerRigidBody.AddRelativeForce(Input.GetAxis("Horizontal") * acceleration,
                                             0,
                                             Input.GetAxis("Vertical") * acceleration);
        }
        else // Complete control while paler is in the air
        {
            playerRigidBody.AddRelativeForce(Input.GetAxis("Horizontal") * acceleration * walkAccelerationRatio,
                                             0,
                                             Input.GetAxis("Vertical") * acceleration * walkAccelerationRatio);
        }

        if (isGrounded) // This section of code adds friction to player's movement so that it does not 
                        // slip when no forece is applied
        {
            float xMove = Mathf.SmoothDamp(playerRigidBody.velocity.x,
                                            0,
                                            ref walkDeaccelerateX,
                                            deaccelerate);
            float zMove = Mathf.SmoothDamp(playerRigidBody.velocity.z,
                                            0,
                                            ref walkDeaccelerateZ,
                                            deaccelerate);
            playerRigidBody.velocity = new Vector3(xMove, playerRigidBody.velocity.y, zMove);
        }
    }
    // this buit-in method gets called whener Unity detects collision of two bodies
    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Angle(contact.normal, Vector3.up) < maxSlope)  // detecting
            {
                isGrounded = true;

            }
        }
    }
    // Making player state in the air
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name.Equals("Platform"))  // todo track name of 'Plane' or 
                                                        // identify some other way
        {
            {
                isGrounded = false;
            }
        }
    }
}