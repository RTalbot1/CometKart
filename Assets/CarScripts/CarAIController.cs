using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAIController : MonoBehaviour
{
    public List<Transform> waypoints;
    public Rigidbody sphere;
    public float speedInput;
    public float currentSpeed;
    public float maxSpeed = 10f;
    public float forwardAccel = 1f;
    public float reverseAccel = 1f;
    public float turnStrength = 1f;
    public float gravityForce = 10f;
    public float dragOnGround = 3;

    public bool grounded;
    public LayerMask whatIsGround;
    public float groundRayLength = 0.5f;
    public Transform groundRayPoint;
    public Transform groundRayPoint2;

    public Transform leftfront, rightfront;
    public float maxWheelTurn = 25;

    private int currentWaypoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        sphere.transform.parent = null;
    }

    private void FixedUpdate()
    {
       // Debug.Log("grounded: " + grounded);

        if (grounded)
        {
            sphere.drag = dragOnGround;
            if (Mathf.Abs(speedInput) > 0)
            {
                sphere.AddForce(transform.forward * speedInput);
            }
        }
        else
        {
            sphere.drag = 0.1f;
            sphere.AddForce(Vector3.up * -gravityForce * 100);
        }
        currentSpeed = sphere.velocity.y;
    }

    void Update()
    {
        grounded = false;
        RaycastHit hit;
        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround) || Physics.Raycast(groundRayPoint2.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }

        Vector3 direction = waypoints[currentWaypoint].position - sphere.transform.position;
        direction.y = 0f;

        if (direction.magnitude < 15f)
        {
            currentWaypoint++;

            if (currentWaypoint >= waypoints.Count)
            {
                currentWaypoint = 0;
            }

            direction = waypoints[currentWaypoint].position - sphere.transform.position;
            direction.y = 0f;
        }

        float angle = Vector3.Angle(transform.forward, direction);

        if (angle > 0f)
        {
            // /Debug.Log("Angle: " + angle);
        }

       if(angle >= 30)
        {
            speedInput = forwardAccel * 2800 * Time.deltaTime;
        } 
        else if (angle >= 1)
        {
            speedInput = forwardAccel * 42056 * Time.deltaTime;
        } 
        else
        {
            speedInput = forwardAccel * 70093 * Time.deltaTime;
        }

        if (Mathf.Abs(speedInput) > 0)
        {
            //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnStrength * Time.deltaTime, 0f));
            maxWheelTurn = 30;
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), turnStrength * Time.deltaTime);

        leftfront.localRotation = Quaternion.Euler(leftfront.localRotation.eulerAngles.x, leftfront.localRotation.eulerAngles.y, maxWheelTurn - 90);
        rightfront.localRotation = Quaternion.Euler(rightfront.localRotation.eulerAngles.x, rightfront.localRotation.eulerAngles.y, maxWheelTurn - 90);

        transform.position = sphere.transform.position;
    }
}
