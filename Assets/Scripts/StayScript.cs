using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class StayScript : MonoBehaviour {


    public static StayScript instance;

    public int gamesPlayed;
    int gameTracker = 0;

    AudioSource source;
    public AudioClip themeSong;

    public float wait;

    bool death = false;

    bool addOnce = false;

    void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            instance = this;
            gamesPlayed = 0;
        }
    }

    // Use this for initialization
    void Start () {
        
        source = GetComponent<AudioSource>();
        wait = themeSong.length;
        source.PlayOneShot(themeSong, 0.35f);




        Advertisement.Initialize("1396504", false);


    }
	
	// Update is called once per frame
	void Update () {

        if (gamesPlayed > gameTracker && gamesPlayed % 2 == 0 && gamesPlayed != 0)
        {

            if (Advertisement.IsReady())
            {
                //Debug.Log("here");
                Advertisement.Show();

            }

            gameTracker = gamesPlayed;

        }
        
        wait -= Time.deltaTime;

        if (wait < 0)
        {
            source.PlayOneShot(themeSong, 0.35f);
            wait = themeSong.length;
        }
    }

    public void increasedGamesPlayed()
    {

        gamesPlayed++;

    }

    public int getGamesPlayed()
    {
        return gamesPlayed;
    }

    //public void setDeath(bool inBool)
    //{
    //    death = inBool;
    //    addOnce = false;
    //}

}
