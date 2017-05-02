using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public Vector3 gravity;
    public Vector3 velocity = Vector3.zero;
    public Vector3 jumpVelocity;
    public bool didJump = false;
    public float maxSpeed = 5f;
    public Vector3 jump;
    public float jumpForce = 2.0f;
    Rigidbody rb;
    int rotate = 0;

    Vector3 myView;
    public GameObject explosion;
    public bool death;
    public bool floor;
    public bool once;

    public bool belowDieOnce;
    public bool upperDieOnce;

    Vector3 upperBound;
    Vector3 lowerBound;

    GameManager gameManagerRef;

    public bool jumped = false;
    TreadmillScript treadMillRef;
    bool setOnce;

    public int count = 0;


    public ParticleSystem orb;

    int shipScrollCounter;
    int shipMod;
    public RaycastHit hit;
    public Ray ray;
    public GameObject rightArrowObj;
    public GameObject leftArrowObj;
    //Rendererj
    //StayScript stayRef;

    Color startGreyColor;

    AudioSource source;
    public AudioClip jumpSound;
    public AudioClip changeShipSound;


    // Use this for initialization
    void Start()
    {
        gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
        treadMillRef = GameObject.Find("Treadmill").GetComponent<TreadmillScript>();
        //stayRef = GameObject.Find("StayObj").GetComponent<StayScript>();
        rb = GetComponent<Rigidbody>();
        startGreyColor = GetComponent<Renderer>().materials[0].color;
        source = GetComponent<AudioSource>();
        StartCoroutine(ShipUpandDown());
        //orb.Play();

        if (PlayerPrefs.GetInt("HighScore") < 10)
        {

            //shipScrollCounter = 0;
            shipMod = 0;
            rightArrowObj.SetActive(false);
            leftArrowObj.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("HighScore") >= 10 && PlayerPrefs.GetInt("HighScore") < 20)
        {
            //myMats[0] = prestiges[0];

            //shipScrollCounter = 1;
            shipMod = 1;
            //GetComponent<Renderer>().material.color = Color.cyan;
        }
        else if (PlayerPrefs.GetInt("HighScore") >= 20 && PlayerPrefs.GetInt("HighScore") < 30)
        {

            //shipScrollCounter = 2;
            shipMod = 2;
        }
        else if (PlayerPrefs.GetInt("HighScore") >= 30 && PlayerPrefs.GetInt("HighScore") < 40)
        {

            //shipScrollCounter = 3;
            shipMod = 3;
        }
        else if (PlayerPrefs.GetInt("HighScore") >= 40 && PlayerPrefs.GetInt("HighScore") < 50)
        {

            //shipScrollCounter = 4;
            shipMod = 4;
        }
        else if (PlayerPrefs.GetInt("HighScore") >= 50 && PlayerPrefs.GetInt("HighScore") < 60)
        {

            //shipScrollCounter = 5;
            shipMod = 5;
        }
        else if (PlayerPrefs.GetInt("HighScore") >= 60)
        {

            //shipScrollCounter = 5;
            shipMod = 6;
        }

        shipScrollCounter = PlayerPrefs.GetInt("CurrentShip");

    }

    // Update is called once per frame
    void Update()
    {
        //ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (shipScrollCounter == 0)
        {
            GetComponent<Renderer>().materials[2].color = Color.blue;
            GetComponent<Renderer>().materials[0].color = startGreyColor;
        }
        else if(shipScrollCounter == 1)
        {
            GetComponent<Renderer>().materials[2].color = Color.green;
            GetComponent<Renderer>().materials[0].color = startGreyColor;
        }
        else if (shipScrollCounter == 2)
        {
            GetComponent<Renderer>().materials[2].color = Color.red;
            GetComponent<Renderer>().materials[0].color = startGreyColor;
        }
        else if(shipScrollCounter == 3)
        {
            GetComponent<Renderer>().materials[2].color = Color.yellow;
            GetComponent<Renderer>().materials[0].color = startGreyColor;
        }
        else if(shipScrollCounter == 4)
        {
            GetComponent<Renderer>().materials[2].color = Color.cyan;
            GetComponent<Renderer>().materials[0].color = startGreyColor;
        }
        else if (shipScrollCounter == 5)
        {
            GetComponent<Renderer>().materials[0].color = Color.cyan;
            GetComponent<Renderer>().materials[2].color = Color.cyan;
        }
        else if (shipScrollCounter == 6)
        {
            GetComponent<Renderer>().materials[0].color = Color.black;
            GetComponent<Renderer>().materials[2].color = Color.cyan;
        }
       
        


        //transform.Rotate(0, 0, 20 * Time.deltaTime);

        controls();

        myView = Camera.main.WorldToViewportPoint(this.transform.position);

        if(myView.y > 1)
        {
            //this.transform.position = new Vector3(0.06f, 5.96f, 0);
            death = true;
            gameManagerRef.setDeath(death);

            if (!upperDieOnce)
            {
                Instantiate(explosion, this.transform.position, Quaternion.identity);
                upperDieOnce = true;
            }
        }

        if (myView.y < 0)
        {
            //this.transform.position = new Vector3(0.06f, -3.56f, 0);
            
            if (!belowDieOnce)
            {
                
                Instantiate(explosion, this.transform.position, Quaternion.identity);
                belowDieOnce = true;
            }

            death = true;
            gameManagerRef.setDeath(death);
        }

        if (death)
        {
            if (!floor)
            {
                Vector3 destination = new Vector3(0, 0, 70);
                transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles,
                                                     destination,
                                                     Time.deltaTime * 2);

                this.gameObject.GetComponent<Rigidbody>().useGravity = true;
                    
            }
            
        }



        




    }


    private void FixedUpdate()
    {

        if (jumped)
        {

            velocity += 2 * gravity * Time.deltaTime;

            if (didJump == true)
            {

                velocity = new Vector3(0, 0, 0);

                velocity.y += jumpVelocity.y;

                source.PlayOneShot(jumpSound, 0.7f);

                didJump = false;
            }

            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
            transform.position += velocity * Time.deltaTime;
        }
        else
        {

            if (count % 2 == 0)
            {
                transform.Translate(new Vector3(0.0f, -0.025f, 0.0f), Space.World);
            }
            else
            {
                transform.Translate(new Vector3(0.0f, 0.025f, 0.0f), Space.World);
            }



        }


        //if (velocity.y > 0)
        //{
        //    orb.Play();
        //    //orb.emission.enabled = true;
        //}
        //else
        //{
        //    orb.Stop();
        //}


    }

    IEnumerator ShipUpandDown()
    {
        while (true)
        {
            if (!jumped)
            {
                //spawnEnemy();
                count++;
            }
            yield return new WaitForSeconds(0.6f);
        }
    }


    private void controls()
    {
        if (!death)
        {
            //if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            //{
            //    //orb.Play();
            //    didJump = true;

            //    if (!setOnce)
            //    {
            //        treadMillRef.setJumped(true);
            //        gameManagerRef.setJumped(true);
            //        jumped = true;
            //        setOnce = true;
            //    }
            //    //rb.AddForce(jump * jumpForce, ForceMode.Impulse);

            //    // GetComponent<Rigidbody2D>().AddForce(Vector2.up * maxSpeed / 3);
            //}


            if (Input.GetMouseButtonDown(0))
            {
                bool rightArrow  = false;
                bool leftArrow = false;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                   
                    if (hit.collider.gameObject.name == "right_arrow")
                    {
                        //gameManagerRef.onArrowSelect();
                        shipScrollCounter++;
                        //shipScrollCounter = shipScrollCounter % shipMod;


                        if (shipScrollCounter > shipMod)
                        {
                            shipScrollCounter = 0;
                        }

                        source.PlayOneShot(changeShipSound, 1);

                        rightArrow = true;

                        PlayerPrefs.SetInt("CurrentShip", shipScrollCounter);
                        //stayRef.setChosenShip(shipScrollCounter);

                    }
                    else if(hit.collider.gameObject.name == "left_arrow")
                    {
                        shipScrollCounter--;
                        //shipScrollCounter = shipScrollCounter % shipMod;
                        source.PlayOneShot(changeShipSound, 1);

                        if (shipScrollCounter < 0)
                        {
                            shipScrollCounter = shipMod;
                        }



                        rightArrow = true;
                        PlayerPrefs.SetInt("CurrentShip", shipScrollCounter);
                        //stayRef.setChosenShip(shipScrollCounter);
                    }
                    
                }


                if (!rightArrow && !leftArrow)
                {
                    Debug.Log("here");
                    didJump = true;
                    treadMillRef.setJumped(true);
                    gameManagerRef.setJumped(true);
                    jumped = true;
                    setOnce = true;
                    //rightArrow.SetActive(false);
                    rightArrowObj.SetActive(false);
                    leftArrowObj.SetActive(false);
                }
                


            }

        }
    }


    void OnCollisionEnter(Collision col)
    {
        //Debug.Log("hit");
        if (col.gameObject.tag == "Enemy")
        {
            if (!once)
            {
                death = true;
                
                Debug.Log("hit");
                Instantiate(explosion, col.transform.position, Quaternion.identity);
                gameManagerRef.setDeath(death);
                once = true;
            }
            //this.gameObject.SetActive(false);
            //exp.SetActive(false);
        }


        if(col.gameObject.tag == "Floor")
        {
            if (death)
            {
                floor = true;
            }
        }

    }


    public void setShipCounter(int inCount)
    {
        //Debug.Log("set to " + shipScrollCounter);
        //shipScrollCounter = inCount;
    }

    


}
