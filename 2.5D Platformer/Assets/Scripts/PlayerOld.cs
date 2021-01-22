using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TO CONSIDER:
//Redo code with LERP, speed (how much t is incremented) is based on distance from point A to point B
//With this current implementation, no matter what I do I can't get more precise than .1 units

//TO DO:
//"Protect" the player from changing after reaching the point until they get out of its range
//Try the lerp stuff

public class PlayerOld : MonoBehaviour
{
    //Data Passed Publicly
    public float speed;
    public int offsetFactor;
    public int roundingFactor;

    //constants
    public float offset;
    public float power;

    //Data Relating to Curves
    private BezierCurve[] curves;
    private int currentCurve;
    private Vector3[] pointsOnCurve;
    private int lastPoint;
    private int nextPoint;
    private Vector3 last;
    private Vector3 next;

    //Constantly Updated Gamme Data
    private bool onGround;
    private float degree;
    private float movementX;
    private float movementZ;

    //Components
    private Rigidbody rb;

    void Start()
    {
        Application.targetFrameRate = 60;

        offset = Mathf.Pow(10, -offsetFactor);
        power = Mathf.Pow(10, roundingFactor);

        curves = FindObjectsOfType<BezierCurve>();
        currentCurve = 0;
        pointsOnCurve = curves[currentCurve].pointsOnCurve;
        lastPoint = 0;
        nextPoint = 1;
        last = pointsOnCurve[lastPoint];
        next = pointsOnCurve[nextPoint];

        transform.position = last;

        onGround = true;
        degree = Mathf.Atan2(next.z - transform.position.z, next.x - transform.position.x);
        movementX = Mathf.Cos(degree);
        movementZ = Mathf.Sin(degree);
        Debug.Log(degree + ": " + movementX + ", " + movementZ);
        //add rotation stuff

        rb = gameObject.GetComponent<Rigidbody>();

    }
    void Update()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);

        //code for moving left & right first, should move player along the current BezierCurve's points.
        //account for edge cases of "start of level" and "end of level", either with an invisible or physical wall.
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //could use translate, by getting angles
            rb.AddRelativeForce(new Vector3(speed * movementX, 0, speed * movementZ), ForceMode.Impulse);


            //This might work, but you'd have to go step-by-step for each position.
            //rb.MovePosition(new Vector3(next.x, transform.position.y, next.z));

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddRelativeForce(new Vector3(-speed * movementX, 0, -speed * movementZ), ForceMode.Impulse);

        }

        //then do jumping
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (onGround)
            {

            }
            //else...? a hover maybe?

        }

        //check if player is at next point yet.
        if (SamePosition(transform.position, next))
        {
            Debug.Log("in sameposition next");
            if (nextPoint == pointsOnCurve.Length - 1)
            {
                if (currentCurve == curves.Length - 1)
                {
                    //End of level
                }
                //increment currentCurve. Make sure ground matches.
            }
            else
            {
                nextPoint++;
                lastPoint++;
                last = pointsOnCurve[lastPoint];
                next = pointsOnCurve[nextPoint];
                degree = Mathf.Atan2(next.z - transform.position.z, next.x - transform.position.x);
                movementX = Mathf.Cos(degree);
                movementZ = Mathf.Sin(degree);
                Debug.Log(degree + ": " + movementX + ", " + movementZ);
            }
        }

        if (SamePosition(transform.position, last))
        {
            Debug.Log("in sameposition last");
            if (lastPoint == 0)
            {
                if (currentCurve == 0)
                {
                    //Start of level
                }
                //decrement currentCurve. Make sure ground matches.
            }
            else
            {
                nextPoint--;
                lastPoint--;
                last = pointsOnCurve[lastPoint];
                next = pointsOnCurve[nextPoint];
                degree = Mathf.Atan2(next.z - transform.position.z, next.x - transform.position.x);
                movementX = Mathf.Cos(degree);
                movementZ = Mathf.Sin(degree);
                Debug.Log(degree + ": " + movementX + ", " + movementZ);
            }
        }
        //TO DO: 
        //Should player slant up slopes?
        //Read data from ground using either a LayerMask or OnCollisionEnter
        //That is where currentCurve's number and camera positions will be drawn from.

        transform.position = new Vector3(Mathf.Round(transform.position.x * power) / power, Mathf.Round(transform.position.y * power) / power, Mathf.Round(transform.position.z * power) / power);
    }

    private bool SamePosition(Vector3 p1, Vector3 p2) 
    {
        return (p1.x > p2.x - offset && p1.x < p2.x + offset && p1.z > p2.z - offset && p1.z < p2.z + offset);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //on collision, use layermask to get data
        //if the data's curve is different from the current curve, teleport to the new curve's closest point.
    }
}
