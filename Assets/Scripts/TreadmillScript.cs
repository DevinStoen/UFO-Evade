using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreadmillScript : MonoBehaviour {

    public List<Transform> spawns;
    //public List<Transform> rightSpawn;
    public GameObject[] treadmillObsticals;
    public List<GameObject> obsticals;

    public GameObject obj;
    public int randomEnemy;
    public int randomSpawn;
    public int pooledAmount = 12;

    public int enemyCount;

    public int enemiesAvoided = 0;

    GameManager gameManagerRef;

    public bool dead;

    public bool jumped = false;

    public int difficulty = 7;

    // Use this for initialization
    void Start () {

        gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
        //treadmillObsticals = new List<GameObject>();
        spawns = new List<Transform>();
        obsticals = new List<GameObject>();

        for (int i = 0; i < treadmillObsticals.Length; i++)
        {
            //may not need this, drag and drop ??
            //Debug.Log("obstical" + i);
            //load road levels
            obj = (GameObject) Instantiate(treadmillObsticals[i]);

            obj.SetActive(false);
            //spawns.Add(obj.transform);
            obsticals.Add(obj);
            //levels are ready
        }

        foreach (Transform child in transform)
        {
            //child is your child transform
            //child.GetComponent<RoadSpawn>().setTreadmillRef(this);



            if (child.tag == "EnemySpawnR")
            {
                spawns.Add(child);
            }
            else if (child.tag == "EnemySpawnL")
            {
                spawns.Add(child);
            }


            foreach (Transform chil in transform)
            {
                if (!chil.Equals(child))
                {
                    if(child.GetComponent<SpawnScript>().getLane() == chil.GetComponent<SpawnScript>().getLane())
                    {
                        child.GetComponent<SpawnScript>().setCorSpawn(chil.GetComponent<SpawnScript>());
                    }
                }
            }

        }

        //for(int i = 0; i < 1; i++)
        //{
        //    spawnEnemy();
        //}


        StartCoroutine(TacTiming());

    }
	
	// Update is called once per frame
	void Update () {

    }

    IEnumerator TacTiming()
    {
        while (true)
        {
            if (enemyCount < difficulty && jumped && !dead)
            {
                spawnEnemy();

            }
            yield return new WaitForSeconds(0.3f);
        }
    }


    public void spawnEnemy()
    {

        randomEnemy = Random.Range(0, obsticals.Count);

        randomSpawn = Random.Range(0, spawns.Count);

        while (obsticals[randomEnemy].activeInHierarchy)
        {
            randomEnemy = UnityEngine.Random.Range(0, obsticals.Count);
        }

        obsticals[randomEnemy].GetComponent<EnemyScript>().setActive(true);
        


        while (spawns[randomSpawn].GetComponent<SpawnScript>().getUsed())
        {
            randomSpawn = UnityEngine.Random.Range(0, spawns.Count);
        }


        obsticals[randomEnemy].transform.position = new Vector3 (spawns[randomSpawn].transform.position.x, spawns[randomSpawn].transform.position.y, -1f);

        obsticals[randomEnemy].transform.localEulerAngles = new Vector3(0, spawns[randomSpawn].GetComponent<SpawnScript>().getRotation(), 0);

        obsticals[randomEnemy].GetComponent<EnemyScript>().setDir(spawns[randomSpawn].GetComponent<SpawnScript>().getDir());

        obsticals[randomEnemy].GetComponent<EnemyScript>().setSpawnRef(spawns[randomSpawn].GetComponent<SpawnScript>());

        spawns[randomSpawn].GetComponent<SpawnScript>().setUsed(true);


        spawns[randomSpawn].GetComponent<SpawnScript>().setUsed(true);
        spawns[randomSpawn].GetComponent<SpawnScript>().getCorSpawn().setUsed(true);


        enemyCount++;
        //obsticals[randomEnemy].
    }



    public void turnOffEnemy(GameObject inObject)
    {
        enemyCount = enemyCount - 1;

        inObject.SetActive(false);

        enemiesAvoided++;

        if(enemiesAvoided != 0 && enemiesAvoided % 4 == 0)
        {
            gameManagerRef.increaseScore();
        }

    }

    public int getEnemiesAvoided()
    {
        return enemiesAvoided;
    }


    public void setDeath(bool inBool)
    {
        dead = inBool;
    }

    public void setJumped(bool inBool)
    {
        jumped = inBool;
    }


    public void increaseDifficulty()
    {
        difficulty++;
    }
        //return obsticals[randomEnemy];
}
