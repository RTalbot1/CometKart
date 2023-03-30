using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;

    public Rigidbody sphere;

    public float forwardAccel, reverseAccel, maxSpeed, turnStrength, driftStrength, gravityForce = 10f, dragOnGround = 3;
    public float currentSpeed = 0;

    private float speedInput, turnInput;
    private bool grounded;

    public LayerMask whatIsGround;
    public float groundRayLength = 0.5f;
    public Transform groundRayPoint;

    public Transform leftfront, rightfront;
    public float maxWheelTurn=25;

    // Start is called before the first frame update
    void Start()
    {
        sphere.transform.parent = null;
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        grounded = false;
        RaycastHit hit;
        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }

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
        //sphere.AddForce(transform.forward * forwardAccel * 1000);
        currentSpeed = sphere.velocity.y;
    }

    private void Update()
    {
        speedInput = 0;
        bool forward = playerInput.actions["Accelerate"].IsPressed();
        bool backward = playerInput.actions["Brake"].IsPressed();
        if (forward && !backward)
        {
            speedInput = 1f * forwardAccel * 1000;
        }
        else if(!forward && backward)
        {
            speedInput = -1f * reverseAccel * 1000;
        }

        turnInput = playerInput.actions["FullMovement"].ReadValue<Vector2>().x;
        if (playerInput.actions["Drift"].IsPressed() && Mathf.Abs(speedInput) > 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * driftStrength * Time.deltaTime, 0f));
            maxWheelTurn = 40;
        }
        else if (Mathf.Abs(speedInput) > 0)
        { 
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime, 0f));
            maxWheelTurn = 30;
        }

        leftfront.localRotation = Quaternion.Euler(leftfront.localRotation.eulerAngles.x, leftfront.localRotation.eulerAngles.y, turnInput * maxWheelTurn - 90) ;
        rightfront.localRotation = Quaternion.Euler(rightfront.localRotation.eulerAngles.x, rightfront.localRotation.eulerAngles.y, turnInput * maxWheelTurn - 90 );

        transform.position = sphere.transform.position;
    }
}
