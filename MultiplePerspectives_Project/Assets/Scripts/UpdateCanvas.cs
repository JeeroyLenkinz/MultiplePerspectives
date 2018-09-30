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
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiplierText;
    public TextMeshProUGUI subAnnouncementText;

    public float announcementTextTime;

    // Use this for initialization
    void Start () {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();

        isWaiting = false;
        previousLevel = 1;
        announcementText.text = "BEGIN";
        subAnnouncementText.text = "";
        StartCoroutine("WipeText");
    }
	
	// Update is called once per frame
	void Update () {
        baseHealthText.text = "City Health: " + gameController.baseHealth;
        killCountText.text = "Kill Count: " + gameController.killCount;
        if (gameController.enemiesToBeDestroyed > 1)
        {
            levelText.text = "Wave " + gameController.currentLevel + ": " + gameController.enemiesToBeDestroyed + " Enemies Left";
        }
        else
        {
            levelText.text = "Wave " + gameController.currentLevel + ": " + gameController.enemiesToBeDestroyed + " Enemy Left";
        }
        scoreText.text = "Score: " + gameController.score;
        multiplierText.text = "x" + gameController.comboMultiplier;

        if (previousLevel != gameController.currentLevel)
        {
            previousLevel = gameController.currentLevel;
            announcementText.text = "LEVEL " + previousLevel;
            StartCoroutine("WipeText");
        }

        if (gameController.gameOver)
        {
            announcementText.text = "GAME OVER";
            subAnnouncementText.text = scoreText.text;
            ClearUIText();
        }
        if (gameController.youWin)
        {
            announcementText.text = "YOU WIN";
            subAnnouncementText.text = scoreText.text;
            ClearUIText();
        }
	}

    void ClearUIText() //This is run when the game is over and you want to clear the rest of the HUD, leaving the announcement up
    {
        baseHealthText.text = "";
        killCountText.text = "";
        levelText.text = "";
        scoreText.text = "";
        multiplierText.text = "";
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
