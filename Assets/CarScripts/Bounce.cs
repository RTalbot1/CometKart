using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public Rigidbody player;
    public Rigidbody sphere;

    // Start is called before the first frame update
    void Start()
    {

        player = GetComponent<Rigidbody>();
        sphere = GetComponent<Rigidbody>(); 
        
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "wall")
        {
            var direction = Vector3.Reflect(sphere.velocity.normalized, collision.contacts[0].normal);
            player.velocity = direction * Mathf.Max(sphere.velocity.magnitude * 2, 50f);
        }
        
    }
}
