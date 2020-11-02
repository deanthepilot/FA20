using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    [SerializeField] public int scorePerKill = 10;
    enum State { Placeholder, Alive, Dying, Transcending }
    State state = State.Alive;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            state = State.Alive;
            ScoreBoard scoreBoard = FindObjectOfType<ScoreBoard>();
            scoreBoard.ScoreHit(scorePerKill);
            print("collision");             
            Destroy(enemy);            
        }
    }    
}
