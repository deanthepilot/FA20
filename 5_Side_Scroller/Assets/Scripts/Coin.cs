using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int scorePerCoin = 10;
    [SerializeField] AudioClip coinCollect;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {            
            transform.GetComponent<CapsuleCollider>().enabled = false;
            transform.GetComponent<Renderer>().enabled = false;            
            ScoreBoard scoreBoard = FindObjectOfType<ScoreBoard>();
            scoreBoard.ScoreHit(scorePerCoin);
            audioSource.Stop();
            audioSource.PlayOneShot(coinCollect);
        }
    }
}
