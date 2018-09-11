using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody rb;
    private int trackDirection;
    private float totalAmountRotated;
    private float currentDashMultiplier;
    private bool isRotating;
    private bool isShooting;
    private bool isDashing;
    private bool canSetZero;

    public Rigidbody projectileRb;

    public float playerSpeed;
    public float playerDashMultiplier;
    public float projectileSpeed;
    public float shootCooldown;
    public float dashCooldown;
    public float trackChangeSpeed;
    public float circleRadius;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        isRotating = false;
        isShooting = false;
        isDashing = false;
        totalAmountRotated = 0;
        currentDashMultiplier = 1;
	}
	
	// Update is called once per frame
	void Update () {
        Move();
        //Listen for key press of the track change buttons
        if ((Input.GetKeyDown("a") || Input.GetAxis("LeftTrigger") != 0) && !isRotating)
        {
            isRotating = true;
            rb.velocity = Vector3.zero;
            trackDirection = 1; //Move left (CW)
        }
        else if ((Input.GetKeyDown("d") || Input.GetAxis("RightTrigger") != 0) && !isRotating) {
            isRotating = true;
            rb.velocity = Vector3.zero;
            trackDirection = -1; //Move right (CCW)
        }
        if (isRotating)
        {
            TrackChange(trackDirection); //if isRotating has been set to true run the TrackChange function every frame until finished
        }

        //Check for shooting
        if (!isShooting)
        {
            StartCoroutine("Shoot");
        }

        //Check for dashing
        if (!isDashing && !isRotating)
        {
            StartCoroutine("Dash");
        }
	}

    private void Move()
    {
        if (!isRotating)
        {
            float moveVertical = Input.GetAxis("Vertical");
            float movement = moveVertical * playerSpeed * currentDashMultiplier;

            rb.velocity = transform.TransformDirection(Vector3.up * movement);
            //rb.position = new Vector3(rb.position.x, rb.position.y + movement, rb.position.z); //X and Z positions untouched, can only move in the Y direction

            //Clamping the movement of the ship with ceiling and floor
            if (rb.position.y >= 20.0f)
            {
                rb.position = new Vector3(rb.position.x, 20.0f, rb.position.z);
            }
            else if (rb.position.y <= 0.5f)
            {
                rb.position = new Vector3(rb.position.x, 0.5f, rb.position.z);
            }
        }
    }

    private void TrackChange(int direction)
    {
        canSetZero = false;
        float rotateAmount = Time.deltaTime * trackChangeSpeed * direction; //Amount to be rotated on each call of Update, takes direction into account

        if (Mathf.Abs(90.0f - Mathf.Abs(totalAmountRotated)) <= Mathf.Abs(rotateAmount)) //Check if the amount you are about to rotate puts you past 90 degrees of total rotation
        {
            rotateAmount = (90.0f - Mathf.Abs(totalAmountRotated))*direction; //Set the amount to be rotated to the remainder before reaching 90 degrees
            isRotating = false;
            canSetZero = true; //totalAmountRotated needs to be incremented after the RotateAround so we need to know to set it to zero afterwards
        }

        rb.transform.RotateAround(Vector3.zero, Vector3.up, rotateAmount); //Rotate around the point 0,0,0 and Y axis by the rotateAmount degrees
        totalAmountRotated += rotateAmount; //Update how many degrees we've now rotated in total

        if (canSetZero)
        {
            totalAmountRotated = 0;
        }
    }

    IEnumerator Dash()
    {
        if (Input.GetKeyDown("e") || Input.GetKeyDown("joystick button 0"))
        {
            currentDashMultiplier = playerDashMultiplier;
            isDashing = true;

            yield return new WaitForSeconds(dashCooldown);
            currentDashMultiplier = 1;
            isDashing = false;
        }
        yield return null;
    }

    IEnumerator Shoot()
    {
        if (Input.GetKeyDown("space") || Input.GetKeyDown("joystick button 2"))
        {
            isShooting = true;
            Rigidbody clone;
            clone = Instantiate(projectileRb, transform.position, transform.rotation) as Rigidbody;
            clone.transform.Rotate(Vector3.forward * 90);
            clone.velocity = transform.TransformDirection(Vector3.left * projectileSpeed);

            yield return new WaitForSeconds(shootCooldown);
            isShooting = false;
        }
    yield return null;
    }

}
