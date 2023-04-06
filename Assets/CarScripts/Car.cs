using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Car : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;

    private float maxSpeed = 20.0f;
    private float moveSpeed = 10f;
    public float currentSpeed;

    private float gravity = 20;

    private float normalRotationSpeed = 0.6f;
    private float driftRotationSpeed = 1f;

    public Transform _myTransform;

    private Rigidbody rb;

    void OnValidate()
    {
        playerInput = GetComponent<PlayerInput>();
        _myTransform = transform;
        rb = GetComponent<Rigidbody>();
    }

    private void StartAccelerating()
    {
        rb.AddRelativeForce(new Vector3(Vector3.forward.x, 0, Vector3.forward.z) *moveSpeed);
    }


    private void Brake()
    {
        rb.AddRelativeForce(new Vector3(Vector3.forward.x, 0, Vector3.forward.z) * -moveSpeed);
    }



    // Update is called once per frame
    void Update()
    {
        currentSpeed = rb.velocity.magnitude;
        Vector3 horizontalMovement = Vector3.zero;
        horizontalMovement.x = playerInput.actions["MoveSideways"].ReadValue<Vector2>().x;
        float rotation = 1;
        
        if (playerInput.actions["Brake"].IsPressed())
        {
            Brake();
        }
        else if (playerInput.actions["accelerate"].IsPressed())
        {
            StartAccelerating();
        }

        if (horizontalMovement.x != 0 && currentSpeed > 2) {
            if (playerInput.actions["Drift"].IsPressed())
            {
                rotation = driftRotationSpeed;
                if (horizontalMovement.x > 1)
                {

                }
            }
            else
            {
                rotation = normalRotationSpeed;
            }
            Quaternion t = _myTransform.rotation;
            _myTransform.eulerAngles = new Vector3(0, _myTransform.eulerAngles.y, 0);
            horizontalMovement = _myTransform.TransformDirection(horizontalMovement);
            _myTransform.rotation = t;
            //rb.AddTorque(_myTransform.up * horizontalMovement);
            _myTransform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(horizontalMovement) , rotation * Time.deltaTime);
        }

        //adds gravity to car
        //rb.AddRelativeForce(Vector3.down * gravity * rb.mass);  
    }
}
