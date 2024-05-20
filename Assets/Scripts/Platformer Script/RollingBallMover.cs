using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBallMover : MonoBehaviour
{
    public GameObject startPoint;
    public GameObject endPoint;

    public float speed;

    void Update()
    {
        if (Vector3.Distance(transform.position, endPoint.transform.position) < .1f)
        {
            transform.position = startPoint.transform.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, endPoint.transform.position, speed * Time.deltaTime);
        transform.Rotate(new Vector3(0, speed * 0.5f, 0)); 
    }
}
