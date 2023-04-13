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
        Debug.Log("grounded: " + grounded);

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

        //Debug.Log("Current Waypoint: " + currentWaypoint);
        //Debug.Log("Max Speed: " + maxSpeed);

        if (direction.magnitude < 3f)
        {
            currentWaypoint++;

            if (currentWaypoint >= waypoints.Count)
            {
                currentWaypoint = 0;
            }
        }
        else
        {
            float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

            float angleFactor = Mathf.Clamp01(1.5f - Mathf.Abs(angle) / 90f);
            float distanceFactor = Mathf.Clamp01(direction.magnitude / 10f);
            float speedFactor = Mathf.Min(angleFactor, distanceFactor);
            float targetSpeed = maxSpeed * speedFactor;

            speedInput = forwardAccel * 100000 * Time.deltaTime;

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
}
