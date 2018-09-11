using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateCanvas : MonoBehaviour {

    private GameController gameController;
    private bool isWaiting;
    private int previousLevel;

    public TextMeshProUGUI baseHealthText;
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI announcementText;
    public TextMeshProUGUI levelText;
    public float announcementTextTime;

    // Use this for initialization
    void Start () {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();

        isWaiting = false;
        previousLevel = 1;
        announcementText.text = "BEGIN";
        StartCoroutine("WipeText");
    }
	
	// Update is called once per frame
	void Update () {
        baseHealthText.text = "Base Health: " + gameController.baseHealth;
        killCountText.text = "Kill Count: " + gameController.killCount;
        levelText.text = "Level " + gameController.currentLevel + ": " + gameController.levelProgress + " Enemies Left";

        if (previousLevel != gameController.currentLevel)
        {
            previousLevel = gameController.currentLevel;
            announcementText.text = "LEVEL " + previousLevel;
            StartCoroutine("WipeText");
        }

        if (gameController.gameOver)
        {
            announcementText.text = "GAME OVER";
        }
        if (gameController.youWin)
        {
            announcementText.text = "YOU WIN";
        }
	}

    IEnumerator WipeText()
    {
        if (!isWaiting)
        {
            isWaiting = true;
            yield return new WaitForSeconds(announcementTextTime);
            announcementText.text = "";
            isWaiting = false;
        }
        yield return null;
    }
}
