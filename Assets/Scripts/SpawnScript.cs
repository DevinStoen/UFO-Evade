using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {


    public int rotation;
    public bool dir;
    public bool isUsed;

    public int laneID;

    public SpawnScript coorespondingSpawn;

    Vector3 startPosition;
    Vector3 myView;
    
	// Use this for initialization
	void Start () {

        myView = Camera.main.WorldToViewportPoint(this.transform.position);

        
    }
	
	// Update is called once per frame
	void Update () {

        //myView

    }

    public int getRotation()
    {
        return rotation;

    }

    public bool getDir()
    {
        return dir;
    }

    public void setUsed(bool inBool)
    {
        isUsed = inBool;
        //coorespondingSpawn.setUsed(inBool);
    }

    public void setLane(int inLane)
    {
        laneID = inLane;
    }

    public int getLane()
    {
        return laneID;
    }


    public bool getUsed()
    {
        
        return isUsed;
    }

    public SpawnScript getCorSpawn()
    {
        return coorespondingSpawn;
    }

    public void setCorSpawn(SpawnScript inSpawn)
    {
        coorespondingSpawn = inSpawn;
    }


}
