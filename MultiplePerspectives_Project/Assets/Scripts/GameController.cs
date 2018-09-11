﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject enemyType1;

    public int maxBaseHealth;
    public int totalEnemyTypes;
    public float spawnTimeMin;
    public float spawnTimeMax;
    public int[] levelRequirements;
    public float restartWaitTime;

    private bool isSpawning;
    private float spawnTimeWait;
    private bool waitingToRestart;

    [HideInInspector]
    public int killCount;
    [HideInInspector]
    public int baseHealth;
    [HideInInspector]
    public bool gameOver;
    [HideInInspector]
    public bool youWin;
    [HideInInspector]
    public int currentLevel;
    [HideInInspector]
    public int levelProgress;

    // Use this for initialization
    void Start () {
        killCount = 0;
        baseHealth = maxBaseHealth;
        gameOver = false;
        youWin = false;
        waitingToRestart = false;
        currentLevel = 1;
	}
	
	// Update is called once per frame
	void Update () {
        levelProgress = levelRequirements[currentLevel - 1] - killCount;
        if (!isSpawning) //Check to see if spawning is already underway, if not start spawning an enemy
        {
            StartCoroutine("SpawnEnemy");
        }
        if (killCount >= levelRequirements[currentLevel-1])
        {
            currentLevel++;
            if (currentLevel > levelRequirements.Length)
            {
                currentLevel = levelRequirements.Length;
                youWin = true;
                StartCoroutine("GameReset");
            }
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

    public void DestroyObject(GameObject objectToDestroy) //Called by EnemyController to destroy their gameObject
    {
        Destroy(objectToDestroy);
    }

    public void KillCountTracker()
    {
        if (!gameOver && !youWin)
        {
            killCount++;
        }
    }

    public void DamageBase(int damageAmount)
    {
        if (!gameOver && !youWin)
        {
            baseHealth -= damageAmount;
        }
        if (baseHealth <= 0)
        {
            baseHealth = 0;
            gameOver = true;
            StartCoroutine("GameReset");
        }
    }

    IEnumerator GameReset()
    {
        if ((youWin || gameOver) && !waitingToRestart)
        {
            waitingToRestart = true;
            yield return new WaitForSeconds(restartWaitTime);
            SceneManager.LoadScene("SampleScene");
        }
        yield return null;
    }

}
