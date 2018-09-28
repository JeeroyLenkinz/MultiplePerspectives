using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private Rigidbody rb;
    private GameController gameController;

    private bool isDead;
    private bool isRotating;
    private int trackNumber;
    private int randomRotation;
    private bool canSetZero;
    private float totalAmountRotated;
    private float direction;
    private int randomDirectionNumber;

    public int enemyType;
    public int enemyHealth;
    public float enemySpeed;
    public int enemyDamage;
    public bool spawnInCenter;
    public bool rotateOnHit;
    public float rotateSpeed;

    public float cylinderRadius;
    public float cylinderHeight;

    
    // Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();

        isDead = false;
        isRotating = false;
        totalAmountRotated = 0.0f;
        trackNumber = Random.Range(1, 6); //Randomly choose which track to spawn on (max is exclusive for ints, hence using 6)

        if (spawnInCenter) //Enemies that must spawn in the center
        {
            trackNumber = 5;
        }

        switch (trackNumber) //Below comments are based on a top down view of the cylinder (the player starts at the Bottom)
        {
            case 1: //Bottom
                rb.position = new Vector3(0, cylinderHeight, -cylinderRadius);
                transform.Rotate(Vector3.up * 180);
                break;
            case 2: //Right
                rb.position = new Vector3(cylinderRadius, cylinderHeight, 0);
                transform.Rotate(Vector3.up * 90);
                break;
            case 3: //Top
                rb.position = new Vector3(0, cylinderHeight, cylinderRadius);

                break;
            case 4: //Left
                rb.position = new Vector3(-cylinderRadius, cylinderHeight, 0);
                transform.Rotate(Vector3.up * -90);
                break;
            case 5: //Center
                rb.position = new Vector3(0, cylinderHeight, 0);
                randomRotation = Random.Range(0, 4);
                rb.transform.Rotate(Vector3.up * 90 * randomRotation); //Give it a random rotation to decide which is the vulnerable side
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
            gameController.DamageBase(enemyDamage);
        }
        if (isDead)
        {
            gameController.DestroyObject(rb.gameObject);
        }
        rb.position = new Vector3(rb.position.x, rb.position.y - enemySpeed, rb.position.z); //Slowly move the enemy down the cylinder based on the speed

        if (isRotating)
        {
            RotateEnemy();
        }
	}

    private void RotateEnemy()
    {
        canSetZero = false;
        float rotateAmount = Time.deltaTime * rotateSpeed * direction; //Amount to be rotated on each call of Update, takes direction into account

        if (Mathf.Abs((90.0f*randomRotation) - Mathf.Abs(totalAmountRotated)) <= Mathf.Abs(rotateAmount)) //Check if the amount you are about to rotate puts you past 90 degrees of total rotation
        {
            rotateAmount = ((90.0f*randomRotation) - Mathf.Abs(totalAmountRotated))*direction; //Set the amount to be rotated to the remainder before reaching 90 degrees
            isRotating = false;
            canSetZero = true; //totalAmountRotated needs to be incremented after the RotateAround so we need to know to set it to zero afterwards
        }

        rb.transform.Rotate(Vector3.up * rotateAmount);
        totalAmountRotated += rotateAmount; //Update how many degrees we've now rotated in total

        if (canSetZero)
        {
            totalAmountRotated = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile" && !isRotating)
        {
            enemyHealth--;
            if (enemyHealth > 0 && rotateOnHit && !isRotating)
            {
                randomRotation = Random.Range(1, 4);
                randomDirectionNumber = Random.Range(1, 3);
                if (randomDirectionNumber == 1)
                {
                    direction = 1;
                }
                else
                {
                    direction = -1;
                }
                isRotating = true;
            }

            if (enemyHealth <= 0)
            {
                isDead = true;
                gameController.KillCountTracker(enemyType);
            }

            gameController.DestroyObject(other.gameObject);
        }
    }

}
