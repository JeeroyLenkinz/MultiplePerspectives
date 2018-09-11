using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private Rigidbody rb;
    private GameController gameController;

    private bool isDead;
    private int trackNumber;

    public string enemyType;
    public int enemyHealth;
    public float enemySpeed;

    public float cylinderRadius;
    public float cylinderHeight;
    
    // Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();

        isDead = false;
        trackNumber = Random.Range(1, 5); //Randomly choose which track to spawn on

        switch (trackNumber) //Below comments are based on a top down view of the cylinder (the player starts at the Bottom)
        {
            case 1: //Bottom
                rb.position = new Vector3(0, cylinderHeight, -cylinderRadius);
                break;
            case 2: //Right
                rb.position = new Vector3(cylinderRadius, cylinderHeight, 0);
                break;
            case 3: //Top
                rb.position = new Vector3(0, cylinderHeight, cylinderRadius);
                break;
            case 4: //Left
                rb.position = new Vector3(-cylinderRadius, cylinderHeight, 0);
                break;
            case 5: //Center
                rb.position = new Vector3(0, cylinderHeight, 0);
                break;
            default: //Default to center in case of error
                rb.position = new Vector3(0, cylinderHeight, 0);
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (rb.position.y <= 0)
        {
            isDead = true;
        }
        if (isDead)
        {
            gameController.DestroyEnemy(rb.gameObject);
        }
        rb.position = new Vector3(rb.position.x, rb.position.y - enemySpeed, rb.position.z); //Slowly move the enemy down the cylinder based on the speed
	}
}
