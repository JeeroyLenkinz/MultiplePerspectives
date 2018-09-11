using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWall : MonoBehaviour {

    private GameController gameController;

    // Use this for initialization
    void Start () {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            gameController.DestroyObject(other.gameObject);
        }
    }

}
