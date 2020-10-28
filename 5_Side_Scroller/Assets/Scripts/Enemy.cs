using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject enemy;
    enum State { Placeholder, Alive, Dying, Transcending }
    State state = State.Alive;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.Find("Enemy");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == enemy)
        {
            state = State.Alive;
            print("collision");
            Destroy(enemy);
        }
        else { return; }
    }
}
