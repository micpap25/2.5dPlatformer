using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//TO DO:
//Make the particles go in the opposite direction of motion, and make them emit faster, also make them disappear faster, and put in actual materials. 

public class Player : MonoBehaviour
{
    //Data Passed Publicly
    public float speed;
    //public int offsetFactor;
    //public int roundingFactor;

    //constants
    //private float offset;
    //private float power;

    //Data Relating to Curves
    private BezierCurve[] curves;
    public int currentCurve;
    private Vector3[] pointsOnCurve;
    private int lastIndex;
    private int nextIndex;
    private Vector3 last;
    private Vector3 next;


    //Constantly Updated Gamme Data
    private bool onGround;
    public GameObject groundCheck;
    private float t;

    //Components
    private Rigidbody rb;

    //Particles
    public ParticleSystem dust;

    void Start()
    {
        Application.targetFrameRate = 60;

        //offset = Mathf.Pow(10, -offsetFactor);
        //power = Mathf.Pow(10, roundingFactor);

        //SORT CURVES BY NAME
        //NAMING SCHEME: "Curve " Numbers -> Uppercase Letters -> Lowercase Letters -> The same with tildes before them.
        curves = FindObjectsOfType<BezierCurve>().OrderBy(obj=>obj.name).ToArray();
        currentCurve = 0;
        pointsOnCurve = curves[currentCurve].pointsOnCurve;
        lastIndex = 0;
        nextIndex = 1;
        last = pointsOnCurve[lastIndex];
        next = pointsOnCurve[nextIndex];

        //transform.position = last;

        onGround = true;
        t = Vector3.kEpsilon * 2;

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
            t += (speed / Vector3.Distance(last, next));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            t -= (speed / Vector3.Distance(last, next));
        }

        //then do jumping
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (onGround)
            {

            }
            //else...? a hover maybe?

        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (transform.rotation.y > 0)
                transform.Rotate(0.0f, -180.0f, 0.0f);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (transform.rotation.y < 0)
                transform.Rotate(0.0f, 180.0f, 0.0f);
        }

        /*
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            dust.Stop();
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            dust.Stop();
        } 
        */

        //check if player is at next point yet.
        if (SamePosition(transform.position, next))
        {
            Debug.Log("in sameposition next");
            if (nextIndex == pointsOnCurve.Length - 1)
            {
                if (currentCurve == curves.Length - 1)
                {
                    //End of level
                }
                else
                {
                    currentCurve++;
                    pointsOnCurve = curves[currentCurve].pointsOnCurve;
                    lastIndex = 0;
                    nextIndex = 1;
                    last = pointsOnCurve[lastIndex];
                    next = pointsOnCurve[nextIndex];
                }
            }
            else
            {
                nextIndex++;
                lastIndex++;
                last = pointsOnCurve[lastIndex];
                next = pointsOnCurve[nextIndex];
            }
            t = Vector3.kEpsilon * 2;
        }

        else if (SamePosition(transform.position, last))
        {
            Debug.Log("in sameposition last");
            if (lastIndex == 0)
            {
                if (currentCurve == 0)
                {
                    //Start of level
                }
                else
                {
                    currentCurve--;
                    pointsOnCurve = curves[currentCurve].pointsOnCurve;
                    lastIndex = pointsOnCurve.Length - 2;
                    nextIndex = pointsOnCurve.Length - 1;
                    last = pointsOnCurve[lastIndex];
                    next = pointsOnCurve[nextIndex];
                }
            }
            else
            {
                nextIndex--;
                lastIndex--;
                last = pointsOnCurve[lastIndex];
                next = pointsOnCurve[nextIndex];
            }
            t = 1 - Vector3.kEpsilon * 2;
        }
        //TO DO: 
        //Should player slant up slopes?
        //Read data from ground using either a LayerMask or OnCollisionEnter
        //That is where currentCurve's number and camera positions will be drawn from.

        transform.position = Vector3.Lerp(new Vector3(last.x, transform.position.y, last.z), new Vector3(next.x, transform.position.y, next.z), t);
        //transform.position = new Vector3(Mathf.Round(transform.position.x * power) / power, Mathf.Round(transform.position.y * power) / power, Mathf.Round(transform.position.z * power) / power);
    }

    private bool SamePosition(Vector3 p1, Vector3 p2)
    {
        return (p1.x > p2.x - Vector3.kEpsilon && p1.x < p2.x + Vector3.kEpsilon && p1.z > p2.z - Vector3.kEpsilon && p1.z < p2.z + Vector3.kEpsilon);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //on collision (landing or changing platforms), use layermask to get data
        //if the data's curve is different from the current curve, teleport to the new curve's closest point.
    }
}
