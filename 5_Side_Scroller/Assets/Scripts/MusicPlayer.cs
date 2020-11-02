using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    bool toggle = true;
    private void Awake()
    {
        int numMusicPlayers = FindObjectsOfType<MusicPlayer>().Length;
        if (numMusicPlayers > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    public void Toggle()
    {
        if (toggle)
        {
            gameObject.SetActive(false);
            toggle = false;
        }
        else 
        {
            gameObject.SetActive(true);
            toggle = true;
        }
        print("toggle");
    }
}
