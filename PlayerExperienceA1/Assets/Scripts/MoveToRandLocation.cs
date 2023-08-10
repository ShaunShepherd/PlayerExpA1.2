using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToRandLocation : MonoBehaviour
{
    [SerializeField] float moveableArea;
    [SerializeField] float moveSpeed;


    Vector3 startingPos= Vector3.zero;
    Vector3 newTarget = Vector3.zero;

    Grow grow;
    Rigidbody rb;

    void Start()
    {
        startingPos = transform.position;

        newTarget = GenRandPos(startingPos, moveableArea);

        grow = GetComponent<Grow>();

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (grow.growing)
        {
            if (Vector3.Distance(transform.position, newTarget) < 1)
            {
                newTarget = GenRandPos(startingPos, moveableArea);
            }

            MoveToPos(newTarget);
        }
        else
        {
            MoveToPos(startingPos);
        }
    }

    Vector3 GenRandPos(Vector3 centrePoint, float range)
    {
        Vector3 randomPos;

        randomPos = new Vector3(Random.Range(-range, range) + centrePoint.x, Random.Range(-range, range) + centrePoint.y, transform.position.z);

        return randomPos;
    }

    void MoveToPos(Vector3 target)
    {
        Vector3 targetVector = (target + transform.position).normalized * moveSpeed;

        if (Vector3.Distance(transform.position, target) < .5)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            rb.velocity = new Vector3(targetVector.x, targetVector.y, 0);
        }

        //rb.AddForce(((target + transform.position)).normalized * moveSpeed * 10, ForceMode.Force);
    }
}
