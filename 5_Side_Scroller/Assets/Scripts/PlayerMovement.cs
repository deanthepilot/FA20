using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float jumpForce = 1500f;
    [SerializeField] float speed = 10f;
    [SerializeField] float levelLoadDelay = 3f;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;
    AudioSource audioSource;
    Rigidbody rb;
    bool isGrounded;
    bool collisionsDisabled = false;
    enum State { Placeholder, Alive, Dying, Transcending }
    State state = State.Alive;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
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
        if (state == State.Alive)
        {
            PlayerJump();
            PlayerMove();
        }
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
    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionsDisabled) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing, someone I like
                print("Friendly");
                break;
            case "Finish":
                print("Finish");
                StartSuccessSequence();
                break;
            default:
                print("Death");
                StartDeathSequence();
                break;
        }
    }

    void StartDeathSequence()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        state = State.Dying;
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    void StartSuccessSequence()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        state = State.Transcending;
    }

    void LoadFirstLevel()
    {
        state = State.Alive;
        SceneManager.LoadScene(0);
    }
}