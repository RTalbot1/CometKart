using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTestScript : MonoBehaviour
{
    private float velo = -2f;

    private Vector2 start;
    private Vector2 half;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * velo);
    }

    void Start()
    {
        start = new Vector2(12.66f, 1.89f);
        half = new Vector2(-7.46f, -6.36f);
        StartCoroutine(SwitchPosition());
    }

    IEnumerator SwitchPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            transform.position = new Vector3(half.x, 1.12f, half.y);
            velo *= -1;
            yield return new WaitForSeconds(5f);
            transform.position = new Vector3(start.x, 1.12f, start.y);
            velo *= -1;
        }
    }
}