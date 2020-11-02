using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float jumpForce = 500f;
    [SerializeField] float speed = 10f;
    [SerializeField] float levelLoadDelay = 3f;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;
    MusicPlayer music;
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
        music = FindObjectOfType<MusicPlayer>();
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
            MusicToggle();
        }
    }    
    private void MusicToggle()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            music.Toggle();
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
            case "Bad":
                print("Enemy Hit");
                StartDeathSequence();
                break;
            case "Spike":
                print("Spike Hit");
                StartDeathSequence();
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
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void LoadFirstLevel()
    {
        state = State.Alive;
        SceneManager.LoadScene(1);
    }
    void LoadNextLevel()
    {
        state = State.Alive;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    
}