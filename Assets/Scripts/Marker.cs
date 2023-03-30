using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Marker : MonoBehaviour
{
    public GameObject car;
    public GameObject nextMarker;

    private float velo = 5f;

    // Update is called once per frame
    void Update()
    {
        Vector3 carPos = car.transform.position;
        Vector3 markerPos = transform.position;

        Vector3 direction = markerPos - carPos;

        direction.Normalize();

        car.transform.position = carPos + (direction * Time.deltaTime * velo);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == car)
        {
            this.gameObject.SetActive(false);
            if(nextMarker != null) nextMarker.SetActive(true);
        }
    }
}
