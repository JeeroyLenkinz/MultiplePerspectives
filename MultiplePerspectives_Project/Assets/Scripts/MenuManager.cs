using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    private Vector3 position1;
    private Vector3 position2;
    private int currentPosition;

    public RectTransform arrowRt;
    public RectTransform newGameRt;
    public RectTransform exitGameRt;


	// Use this for initialization
	void Start () {
        position1 = new Vector3 (arrowRt.position.x, newGameRt.position.y, arrowRt.position.z);
        position2 = new Vector3(arrowRt.position.x, exitGameRt.position.y, arrowRt.position.z);
        arrowRt.position = position1;
        currentPosition = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxisRaw("Vertical") < 1)
        {
            arrowRt.position = position2;
            currentPosition = 2;
        }
        else if (Input.GetAxisRaw("Vertical") > 1)
        {
            arrowRt.position = position1;
            currentPosition = 1;
        }

        if (Input.GetKeyDown("space") || Input.GetKeyDown("enter") || Input.GetKeyDown("joystick button 0"))
        {
            if (currentPosition == 1)
            {
                NewGameButton();
            }
            else
            {
                ExitGameButton();
            }
        }
	}

    public void NewGameButton ()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }

}
