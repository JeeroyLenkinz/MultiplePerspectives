using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject enemyType1;
    public int totalEnemyTypes;
    public float spawnTimeMin;
    public float spawnTimeMax;

    private bool isSpawning;
    private float spawnTimeWait;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		if (!isSpawning) //Check to see if spawning is already underway, if not start spawning an enemy
        {
            StartCoroutine("SpawnEnemy");
        }
	}

    IEnumerator SpawnEnemy()
    {
        int spawnedEnemyType = Random.Range(1, totalEnemyTypes); //Spawn a random type of enemy from those provided
        spawnTimeWait = Random.Range(spawnTimeMin, spawnTimeMax); //Select a random wait time between spawns (between the max and min times)

        switch (spawnedEnemyType) //Switch case to spawn the correct enemy prefab based on the randomly chosen type
        {
            case 1:
                Instantiate(enemyType1);
                break;
            default: //Enemy type 1 will be the default enemy type
                Instantiate(enemyType1);
                break;
        }

        isSpawning = true;
        yield return new WaitForSeconds(spawnTimeWait); //Wait for the given wait time before spawning again

        isSpawning = false;
        yield return null;
    }

    public void DestroyEnemy(GameObject enemy) //Called by EnemyController to destroy their gameObject
    {
        Destroy(enemy);
    }
}
