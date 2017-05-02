using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

    public int score;
    public int highScore;
    public int oldHighScore;

    public Texture aTexture;

    public GUIStyle TextStyle;
    public GUIStyle TextStyle2;
    public bool death;
    public bool jumped;

    public Texture topLeftCamDot;
    public Texture TopLeftCamNoDot;
    public Texture TopRightCam;
    public Texture BotRightCam;
    public Texture BotLeftCam;

    TreadmillScript treadMill;

    public Texture retryBtnTexture;

    public GUIStyle RetryStyle;
    public GUIStyle rightArrowStyle;

    public bool gotHighScore = false;

    //bool recDotSwitch;

    public int counter = 0;
    bool deathText;
    PlayerScript player;
    int shipCounter = 0;
    int shipMod = 0;

    bool addOnce = false;

    AudioSource source;
    public AudioClip camCorder;
    //public AudioClip themeSong;
    public AudioClip RetrySound;
    public AudioClip NightSounds;
    public AudioClip BeepSound;

    public float wait;
    public float wait2;
    bool playOnce = false;
    bool playBeepOnce = false;

    StayScript stayRef;


    //ADS
    public string gameId = "1396504";
    public bool enableTestMode = true;

    bool incOnce = false;

    // Use this for initialization
    void Start () {
        
        treadMill = GameObject.Find("Treadmill").GetComponent<TreadmillScript>();
        StartCoroutine(RecSwitch());
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        stayRef = GameObject.Find("StayObj").GetComponent<StayScript>();

        source = GetComponent<AudioSource>();
        //PlayerPrefs.SetInt("HighScore", 60);
        source.PlayOneShot(camCorder, 0.7f);
        //source.PlayOneShot(themeSong, 0.35f);
        source.PlayOneShot(NightSounds, 0.55f);


        //wait = themeSong.length;
        wait2 = NightSounds.length;
        incOnce = false;
    }
	
	// Update is called once per frame
	void Update () {

        TextStyle.fontSize = (int)(90.0f * (float)(Screen.width) / 1920.0f); //scale size font

        TextStyle2.fontSize = (int)(60.0f * (float)(Screen.width) / 1920.0f); //scale size font


        if (death)
        {
            highScore = PlayerPrefs.GetInt("HighScore");
            oldHighScore = PlayerPrefs.GetInt("LastHighScore");

            if (score > highScore)
            {
                PlayerPrefs.SetInt("HighScore", score);
                highScore = PlayerPrefs.GetInt("HighScore");
                gotHighScore = true;








            }
        }
            
        if(score > 35)
        {
            if (!addOnce)
            {
                treadMill.increaseDifficulty();
                addOnce = true;
            }
        }


        //wait -= Time.deltaTime;

        //if(wait < 0)
        //{
        //    source.PlayOneShot(themeSong, 0.35f);
        //    wait = themeSong.length;
        //}

        wait2 -= Time.deltaTime;

        if (wait2 < 0)
        {
            source.PlayOneShot(NightSounds, 0.5f);
            wait2 = NightSounds.length;
        }


        if (jumped)
        {
            if (!playBeepOnce)
            {
                source.PlayOneShot(BeepSound, 1);
                playBeepOnce = true; 
            }

        }



        //if (Advertisement.isSupported)
        //{ // If runtime platform is supported...
        //    Advertisement.Initialize(gameId, enableTestMode); // ...initialize.
        //}


        //topLeftCamDot.transform.position = new Vector3(Scree, 10, 0);
        //TopRightCam.transform.position = new Vector3(10, Screen.height, 0);
        //BotRightCam.transform.position = new Vector3(Screen.width / 10, Screen.height/10, 0);
        //BotLeftCam.transform.position = new Vector3(Screen.width, Screen.height/10, 0);

    }

    void OnGUI()
    {
        if(score >= 20)
        {
            GUI.Label(new Rect(Screen.width / 1.18f, Screen.height / 23, Screen.width / 20, Screen.width / 20), score.ToString(), TextStyle);
        }
        else
        {
            GUI.Label(new Rect(Screen.width / 1.16f, Screen.height / 23, Screen.width / 20, Screen.width / 20), score.ToString(), TextStyle);
        }

       

        //GUI.Label(new Rect(Screen.width / 3.4f, Screen.height / 8f, Screen.width / 20, Screen.width / 20), "UFO Avade", TextStyle);

        if (!jumped)
        {
            if (counter % 2 == 0)
            {
                GUI.Label(new Rect(Screen.width / 2.55f, Screen.height / 1.6f, Screen.width / 20, Screen.width / 20), "Tap to Fly", TextStyle2);
            }

            GUI.Label(new Rect(Screen.width / 3.3f, Screen.height / 5f, Screen.width / 25, Screen.width / 25), "UFO: Evade", TextStyle);

            //if (GUI.Button(new Rect(Screen.width / 1.3f, Screen.height / 2.3f, Screen.width / 13f, Screen.height / 16), "", rightArrowStyle))
            //{
            //    //SceneManager.LoadScene(0);
                
            //}
        }




        if (counter % 2 == 0 || jumped && !death)
        {
            
            GUI.DrawTexture(new Rect(Screen.width / 22, Screen.height / 40, Screen.width / 6, Screen.width / 9), topLeftCamDot);
        }
        else
        {
            GUI.DrawTexture(new Rect(Screen.width / 22, Screen.height / 40, Screen.width / 6, Screen.width / 9), TopLeftCamNoDot);
        }


        
        GUI.DrawTexture(new Rect(Screen.width / 1.25f, Screen.height / 40,              Screen.width / 6, Screen.width / 9), TopRightCam);

        GUI.DrawTexture(new Rect(Screen.width / 1.25f, Screen.height / 1.12f,              Screen.width / 6, Screen.width / 8), BotRightCam);

        GUI.DrawTexture(new Rect(Screen.width / 22, Screen.height / 1.12f,         Screen.width / 6, Screen.width / 8), BotLeftCam);



        if (death)
        {
            StartCoroutine(deathWait());

            if (deathText)
            {

                if (!incOnce)
                {
                    stayRef.increasedGamesPlayed();
                    incOnce = true;
                }



                GUI.Label(new Rect(Screen.width / 3f, Screen.height / 4f, Screen.width / 20, Screen.width / 20), "You Died.", TextStyle);

                GUI.Label(new Rect(Screen.width / 3f, Screen.height / 2.6f, Screen.width / 20, Screen.width / 20), "Score:", TextStyle);
                GUI.Label(new Rect(Screen.width / 2.8f, Screen.height / 2f, Screen.width / 20, Screen.width / 20), "Best:", TextStyle);

                if (!playOnce)
                {
                    source.PlayOneShot(RetrySound, 1f);
                    playOnce = true;
                }
                
                if (gotHighScore)
                {
                    
                    if (counter % 2 == 0)
                    {
                        GUI.Label(new Rect(Screen.width / 1.65f, Screen.height / 2.6f, Screen.width / 20, Screen.width / 20), score.ToString(), TextStyle);
                        GUI.Label(new Rect(Screen.width / 1.7f, Screen.height / 2f, Screen.width / 20, Screen.width / 20), highScore.ToString(), TextStyle);
                    }
                }
                else
                {

                    if (counter % 2 == 0)
                    {
                        GUI.Label(new Rect(Screen.width / 1.65f, Screen.height / 2.6f, Screen.width / 20, Screen.width / 20), score.ToString(), TextStyle);
                    }

                    GUI.Label(new Rect(Screen.width / 1.7f, Screen.height / 2f, Screen.width / 20, Screen.width / 20), highScore.ToString(), TextStyle);

                }



                if (oldHighScore < 10 && highScore >= 10 && gotHighScore)
                {
                    GUI.Label(new Rect(Screen.width / 9f, Screen.height / 1.23f, Screen.width / 20, Screen.width / 20), "You unlocked a new spaceship!", TextStyle2);
                }
                else if (oldHighScore < 20 && highScore >= 20 && gotHighScore)
                {
                    GUI.Label(new Rect(Screen.width / 9f, Screen.height / 1.23f, Screen.width / 20, Screen.width / 20), "You unlocked a new spaceship!", TextStyle2);
                }
                else if (oldHighScore < 30 && highScore >= 30 && gotHighScore)
                {
                    GUI.Label(new Rect(Screen.width / 9f, Screen.height / 1.23f, Screen.width / 20, Screen.width / 20), "You unlocked a new spaceship!", TextStyle2);
                }
                else if (oldHighScore < 40 && highScore >= 40 && gotHighScore)
                {
                    GUI.Label(new Rect(Screen.width / 9f, Screen.height / 1.23f, Screen.width / 20, Screen.width / 20), "You unlocked a new spaceship!", TextStyle2);
                }
                else if (oldHighScore < 50 && highScore >= 50 && gotHighScore)
                {
                    GUI.Label(new Rect(Screen.width / 9f, Screen.height / 1.23f, Screen.width / 20, Screen.width / 20), "You unlocked a new spaceship!", TextStyle2);
                }
                else if (oldHighScore < 60 && highScore >= 60 && gotHighScore)
                {
                    GUI.Label(new Rect(Screen.width / 9f, Screen.height / 1.23f, Screen.width / 20, Screen.width / 20), "You unlocked a new spaceship!", TextStyle2);
                }




                if (GUI.Button(new Rect(Screen.width / 3f, Screen.height / 1.55f, Screen.width / 2.88f, Screen.height / 10), "", RetryStyle))
                {
                    PlayerPrefs.SetInt("LastHighScore", highScore);
                    
                    SceneManager.LoadScene(0);
                    
                }

                
                


            }
        }


      


    }

    IEnumerator RecSwitch()
    {
        while (true)
        {
            counter++;
            yield return new WaitForSeconds(.4f);
        }
    }


    IEnumerator deathWait()
    {
        //while (!jumped)
        //{
           // counter++;
            yield return new WaitForSeconds(2.2f);
            deathText = true;
        //}
    }



    public void setDeath(bool inBool)
    {
        death = inBool;
        treadMill.setDeath(death);
        //stayRef.setDeath(true);

        
    }

    public void setJumped(bool inBool)
    {
        jumped = inBool;
    }



    public void increaseScore()
    {
        if (!death)
        {
            score++;
        }
    }


  


}
