using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public float movementSpeed = 1;
    private float startRot;
    public Renderer levelRenderer;
    public bool seen;
    public bool dir;

    SpawnScript mySpawn;

    TreadmillScript treadScript;

    Vector3 myView;

    Rigidbody rb;

    AudioSource source;
    public AudioClip missileSound;

    // Use this for initialization
    void Start()
    {
        levelRenderer = this.GetComponentInChildren<MeshRenderer>();
        treadScript = GameObject.Find("Treadmill").GetComponent<TreadmillScript>();
        rb = this.GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
        
        if(source != null)
        {
            source.PlayOneShot(missileSound, 0.035f);
        }

    }

    // Update is called once per frame
    void Update()
    {

        myView = Camera.main.WorldToViewportPoint(this.transform.position);


        if (dir == true)
        {
            //transform.Translate(-Vector2.right * movementSpeed * Time.deltaTime);
            //transform.position = new Vector3(2 * Time.deltaTime, this.transform.position.y, this.transform.position.z);
            rb.AddForce(Vector3.left * movementSpeed);

            if (myView.x < 0)
            {
                //seen = false;
                //dir = false;
                mySpawn.setUsed(false);
                mySpawn.getCorSpawn().setUsed(false);
                treadScript.turnOffEnemy(this.gameObject);
                rb.velocity = Vector3.zero;
                //seen = true;
            }


        }
        else
        {
            //transform.Translate(Vector2.right * movementSpeed * Time.deltaTime);
            //transform.position = new Vector3(2 * Time.deltaTime, this.transform.position.y, this.transform.position.z);
            rb.AddForce(Vector3.right * movementSpeed);

            if (myView.x > 1)
            {
                //seen = true;

                //seen = false;
                //dir = false;
                mySpawn.setUsed(false);
                mySpawn.getCorSpawn().setUsed(false);
                treadScript.turnOffEnemy(this.gameObject);
                rb.velocity = Vector3.zero;

            }


        }

        //if (levelRenderer.isVisible)
        //{
        //    seen = true;
        //}

        //if (seen && !levelRenderer.isVisible)
        //{
        //    seen = false;
        //    dir = false;
        //    mySpawn.setUsed(false);
        //    mySpawn.getCorSpawn().setUsed(false);
        //    treadScript.turnOffEnemy(this.gameObject);

        //}
    }

    public void setDir(bool inBool)
    {
        dir = inBool;
    }

    public void setSpawnRef(SpawnScript inSpawn)
    {
        mySpawn = inSpawn;
    }

    public void setActive(bool inBool)
    {
        this.gameObject.SetActive(inBool);
        //this.gameObject.GetComponentInChildren<ParticleSystem>().emission = true;
    }




    void OnCollisionEnter(Collision col)
    {
        //Debug.Log("hit");
        if (col.gameObject.tag == "Player")
        {
            this.gameObject.SetActive(false);
            //this.gameObject.SetActive(false);
            //exp.SetActive(false);
        }


       

    }



}
