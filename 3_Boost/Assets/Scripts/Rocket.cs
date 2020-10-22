using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    // lighting needs improvement
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float levelLoadDelay = 3f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

    // Member Variables
    Rigidbody rigidBody;  // rocket's Rigid Body
    AudioSource audioSource;

    enum State { Placeholder, Alive, Dying, Transcending }
    State state = State.Alive;
    bool collisionsDisabled = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();  // Find the Rigid Body component (reflection)
        audioSource = GetComponent<AudioSource>();  // Find the AudioSource component (reflection)
    }

    // Update is called once per frame
    void Update()
    {
        // todo stop sound on death
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        //if (Debug.isDebugBuild)
        {
            RespondToDebugInput();
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

    private void StartDeathSequence()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        mainEngineParticles.Stop();
        state = State.Dying;
        Invoke("LoadFirstLevel", levelLoadDelay);  // VS doesn't check
    }

    private void StartSuccessSequence()
    {
        
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        mainEngineParticles.Stop();
        state = State.Transcending;
        Invoke("LoadNextLevel", levelLoadDelay);  // todo parameterize time
    }

    private void LoadFirstLevel()
    {
        state = State.Alive;
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        state = State.Alive;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        print("Scene Count " + SceneManager.sceneCountInBuildSettings);
        print("Current Scene Index " + currentSceneIndex);
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        print("NextSceneIndex " + nextSceneIndex);
        SceneManager.LoadScene(nextSceneIndex);  // todo allow for more than two levels
    }


    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true;  // take manual control of rotation
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // resume physics control of rotation
    }

    private void RespondToDebugInput()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        }
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Play();
        }
    }

    private void ApplyThrust()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);  //  add thrust
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!mainEngineParticles.isPlaying)
        {
            mainEngineParticles.Play();
        }
    }
}

