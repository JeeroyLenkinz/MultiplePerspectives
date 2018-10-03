using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody rb;
    private int trackDirection;
    private float totalAmountRotated;
    private float currentDashMultiplier;
    private float bankRotationAmount;
    private bool isRotating;
    private bool isShooting;
    private bool isDashing;
    private bool canSetZero;
    private bool bankBackToZero;

    public Rigidbody projectileRb;
    public GameObject shipModel;

    public float playerSpeed;
    public float playerDashMultiplier;
    public float projectileSpeed;
    public float shootCooldown;
    public float dashCooldown;
    public float trackChangeSpeed;
    public float bankSpeed;
    public float returnBankSpeed;
    public float bankAngleFinal;
    public float circleRadius;

    public AudioSource shootSound;
    public AudioSource dashSound;
    public AudioSource trackChangeSound;




    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        isRotating = false;
        isShooting = false;
        isDashing = false;
        bankBackToZero = false;
        totalAmountRotated = 0;
        bankRotationAmount = 0;
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
            trackChangeSound.Play();
            bankBackToZero = false;
            shipModel.transform.localEulerAngles = new Vector3(0.0f, shipModel.transform.localEulerAngles.y, shipModel.transform.localEulerAngles.z);
        }
        else if ((Input.GetKeyDown("d") || Input.GetAxis("RightTrigger") != 0) && !isRotating) {
            isRotating = true;
            rb.velocity = Vector3.zero;
            trackDirection = -1; //Move right (CCW)
            trackChangeSound.Play();
            bankBackToZero = false;
            shipModel.transform.localEulerAngles = new Vector3(0.0f, shipModel.transform.localEulerAngles.y, shipModel.transform.localEulerAngles.z);
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

        if (bankBackToZero)
        {
            BankRotation(-trackDirection, 0.0f, returnBankSpeed);
        }

	}

    private void Move()
    {
        if (!isRotating)
        {
            float moveVertical = Input.GetAxis("Vertical");
            float movement = moveVertical * playerSpeed * currentDashMultiplier;

            rb.velocity = transform.TransformDirection(Vector3.up * movement);

            //Clamping the movement of the ship with ceiling and floor
            if (rb.position.y >= 20.0f)
            {
                rb.position = new Vector3(rb.position.x, 20.0f, rb.position.z);
            }
            else if (rb.position.y <= 1.0f)
            {
                rb.position = new Vector3(rb.position.x, 1.0f, rb.position.z);
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
            trackChangeSound.Stop();
        }

        rb.transform.RotateAround(Vector3.zero, Vector3.up, rotateAmount); //Rotate around the point 0,0,0 and Y axis by the rotateAmount degrees
        BankRotation(direction, bankAngleFinal, bankSpeed);
        totalAmountRotated += rotateAmount; //Update how many degrees we've now rotated in total

        if (canSetZero)
        {
            totalAmountRotated = 0;
            bankBackToZero = true;
        }
    }

    IEnumerator Dash()
    {
        if (Input.GetKeyDown("e") || Input.GetKeyDown("joystick button 0"))
        {
            currentDashMultiplier = playerDashMultiplier;
            isDashing = true;
            dashSound.Play();

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
            shootSound.Play();

            yield return new WaitForSeconds(shootCooldown);
            isShooting = false;
        }
    yield return null;
    }

    private void BankRotation(int direction, float target, float speed)
    {
        bankRotationAmount += Time.deltaTime * speed * -direction;
        if (!bankBackToZero)
        {
            if (Mathf.Abs(bankRotationAmount) >= target)
            {
                bankRotationAmount = target * -direction;
            }
        }
        else
        {
            if (bankRotationAmount >= target-5.0f && bankRotationAmount <= target+5.0f)
            {
                bankRotationAmount = target * -direction;
                shipModel.transform.localEulerAngles = Vector3.zero;
                bankRotationAmount = 0;
                bankBackToZero = false;
            }
        }
        shipModel.transform.localEulerAngles = new Vector3(bankRotationAmount, shipModel.transform.localEulerAngles.y, shipModel.transform.localEulerAngles.z);
    }

}
