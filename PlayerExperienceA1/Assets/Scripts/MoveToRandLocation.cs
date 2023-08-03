using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToRandLocation : MonoBehaviour
{
    [SerializeField] float moveableArea;
    [SerializeField] float moveSpeed;


    Vector3 startingPos;

    Vector3 newTarget;

    void Start()
    {
        startingPos = transform.position;

        newTarget = GenRandPos(startingPos, moveableArea);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, newTarget) < .1)
        {
            newTarget = GenRandPos(startingPos, moveableArea);
        }

        MoveToPos(newTarget);
    }

    Vector3 GenRandPos(Vector3 centrePoint, float range)
    {
        Vector3 randomPos;

        randomPos = new Vector3(Random.Range(-range, range) + centrePoint.x, Random.Range(-range, range) + centrePoint.y, transform.position.z);

        return randomPos;
    }

    void MoveToPos(Vector3 target)
    {
        transform.position = Vector3.Lerp(transform.position, target, moveSpeed);
    }
}
