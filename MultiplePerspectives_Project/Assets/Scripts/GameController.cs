﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject[] enemyTypeArray;
    public float[] enemyProbabilityArray;

    public int maxBaseHealth;
    public float spawnTimeMin;
    public float spawnTimeMax;
    public int[] levelRequirements;
    public int[] enemyScoreValues;
    public float restartWaitTime;
    public float comboResetTime;
    public int comboBaseRequirement;
    public float specialEnemySpawnDelay;

    private bool isSpawning;
    private bool waitingToRestart;
    private bool comboTimerActive;
    private bool specialEnemySpawning;
    private float spawnTimeWait;
    private int spawnedEnemyType;

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
    [HideInInspector]
    public int score;
    [HideInInspector]
    public int comboMultiplier;

    // Use this for initialization
    void Start () {
        killCount = 0;
        score = 0;
        comboMultiplier = 1;
        baseHealth = maxBaseHealth;
        gameOver = false;
        youWin = false;
        waitingToRestart = false;
        comboTimerActive = false;
        specialEnemySpawning = false;
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
            else
            {
                specialEnemySpawning = true;
            }
        }
        
        if (!comboTimerActive)
        {
            comboTimerActive = true;
            StartCoroutine("ComboTimer");
        }
	}

    IEnumerator SpawnEnemy()
    {
        float probability = Random.Range(1.0f, 100.0f);
        float minRange = 0;
        float maxRange = 0;

        if (specialEnemySpawning)
        {
            spawnedEnemyType = 5;
            spawnTimeWait = specialEnemySpawnDelay;
            specialEnemySpawning = false;
        }
        else
        {
            for (int j = 0; j < enemyTypeArray.Length; j++)
            {
                maxRange += enemyProbabilityArray[j];

                if (probability >= minRange && probability < maxRange)
                {
                    spawnedEnemyType = j;
                }

                minRange += enemyProbabilityArray[j];
            }
            spawnTimeWait = Random.Range(spawnTimeMin, spawnTimeMax); //Select a random wait time between spawns (between the max and min times)
        }
        
        Instantiate(enemyTypeArray[spawnedEnemyType]);

        isSpawning = true;
        yield return new WaitForSeconds(spawnTimeWait); //Wait for the given wait time before spawning again

        isSpawning = false;
        yield return null;
    }

    IEnumerator ComboTimer()
    {
        int timerStartScore = score;
        yield return new WaitForSeconds(comboResetTime);

        if (timerStartScore == score)
        {
            comboMultiplier = 1;
        }
        if (score - timerStartScore >= comboBaseRequirement*comboMultiplier)
        {
            comboMultiplier++;
        }
        comboTimerActive = false;
        yield return null;
    }

    public void DestroyObject(GameObject objectToDestroy) //Called by EnemyController to destroy their gameObject
    {
        Destroy(objectToDestroy);
    }

    public void KillCountTracker(int enemyType)
    {
        if (!gameOver && !youWin)
        {
            killCount++;
            score += enemyScoreValues[enemyType] * comboMultiplier;
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
            SceneManager.LoadScene(0);
        }
        yield return null;
    }

}
