using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{    
    [SerializeField] GameObject deathFX; // death effect for enemy ships
    [SerializeField] Transform parent; // placeholder to destroy deathFX when done
    [SerializeField] int scorePerHit = 12;
    ScoreBoard scoreboard; // class variable

    // Start is called before the first frame update
    void Start()
    {
        AddNonTriggerBoxCollider();
    }

    private void AddNonTriggerBoxCollider()
    {
        Collider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        GameObject fx = Instantiate(deathFX, transform.position, Quaternion.identity);
        fx.transform.parent = parent;
        ScoreBoard scoreBoard = FindObjectOfType<ScoreBoard>();
        scoreBoard.ScoreHit(scorePerHit);

        print("Particles collided with enemy" + gameObject.name);
        Destroy(gameObject);
    }
}
