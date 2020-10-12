using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f; // default period is 2 seconds
    [Range(0, 1)] // You can have more than one attribute on a variable
    float movementFactor;
    Vector3 destinationVector;

    Vector3 startingPosition;
    float movementStepsize = 0.2f;
    bool stepUp = true;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        return;
        // set movement factor
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2f;
        float rawSineWave = Mathf.Sin(cycles * tau);
        movementFactor = rawSineWave/2f + 0.5f;
        destinationVector = startingPosition + movementVector * movementFactor;
        transform.position = destinationVector;
    }
}
