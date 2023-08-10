using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] GameObject spawnObject;
    [SerializeField] Transform origin;

    GameObject instance;
    Rigidbody instRB;

    bool spawnedStarted;

    void Update()
    {
        if (instance == null)
        {
            if (!spawnedStarted)
            {
                StartCoroutine(DelaySpawn());
            }

        }
    }

    IEnumerator DelayGravityOff()
    {
        yield return new WaitForSeconds(0.6f);

        instRB.useGravity = false;
        instRB.velocity = Vector3.zero;
    }

    IEnumerator DelaySpawn()
    {
        spawnedStarted = true;
        yield return new WaitForSeconds(2);

        instance = Instantiate(spawnObject, transform);

        instance.GetComponent<MoveToRandLocation>().startingPos = origin.position;

        instRB = instance.GetComponent<Rigidbody>();

        instRB.useGravity = true;

        StartCoroutine(DelayGravityOff());

        spawnedStarted = false;
    }
}
