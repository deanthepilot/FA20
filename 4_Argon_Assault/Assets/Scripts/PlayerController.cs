using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    [Header("General")]
    [Tooltip("In ms^-1")] [SerializeField] float xSpeed = 4f;  // in meters per second
    [Tooltip("In ms^-1")] [SerializeField] float ySpeed = 4f;  // in meters per second
    [Tooltip("In meters")] [SerializeField] const float xRange = 6.5f;
    [Tooltip("In meters")] [SerializeField] const float yMin = -4f;
    [Tooltip("In meters")] [SerializeField] const float yMax = 4f;

    [Header("Screen-position Based")]
    [SerializeField] float positionPitchFactor = -5.5f;
    [SerializeField] float controlPitchFactor = -5.5f;
    [Header("Control-Throw Based")]
    [SerializeField] float positionYawFactor = 5.5f;
    [SerializeField] float controlYawFactor = 5.5f;
    [SerializeField] float controlRollFactor = -8f;
    float xThrow, yThrow;

    bool isControlEnabled = true;

    [SerializeField] GameObject[] guns;
    // Start is called before the first frame update
    void Start()
    {
        
    }

   // Update is called once per frame
    void Update()
    {
        if (isControlEnabled)
        {
            ProcessTranslation();
            ProcessRotation();
            ProcessFiring();
        }
    }

    void ProcessFiring()
    {
        bool enableGun = CrossPlatformInputManager.GetAxis("Fire 1") > Mathf.Epsilon;
        {
            //print("Number of guns is " + guns.Length);
            foreach(GameObject gun in guns)
            {
                gun.SetActive(enableGun);
            }
        };
    }

    void OnPlayerDeath()
    {
        isControlEnabled = false;
    }

    private void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;
        float pitch = pitchDueToPosition + pitchDueToControlThrow;

        float yawDueToPosition = transform.localPosition.x * positionYawFactor;
        float yawDueToControlThrow = xThrow * controlYawFactor;
        float yaw = yawDueToPosition + yawDueToControlThrow;

        float roll = xThrow * controlRollFactor;
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void ProcessTranslation()
    {
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        float xOffset = xThrow * xSpeed * Time.deltaTime;
        float rawXPosition = transform.localPosition.x + xOffset;
        float clampedXPosition = Mathf.Clamp(rawXPosition, -xRange, xRange);

        yThrow = CrossPlatformInputManager.GetAxis("Vertical");
        float yOffset = yThrow * ySpeed * Time.deltaTime;
        float rawYPosition = transform.localPosition.y + yOffset;
        float clampedYPosition = Mathf.Clamp(rawYPosition, yMin, yMax);

        transform.localPosition = new Vector3(
            clampedXPosition,
            clampedYPosition,
            transform.localPosition.z);
    }
}
