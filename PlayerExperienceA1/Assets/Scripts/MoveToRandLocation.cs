using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToRandLocation : MonoBehaviour
{
    [SerializeField] float moveableArea;
    [SerializeField] float moveSpeed;
    [SerializeField] GameObject player;
    [SerializeField] Animator animator;

    public Vector3 startingPos;
    Vector3 newTarget = Vector3.zero;

    Grow grow;
    Rigidbody rb;

    void Start()
    {
        newTarget = startingPos;

        grow = GetComponent<Grow>();

        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (grow.growing)
        {
            if (Vector3.Distance(transform.position, newTarget) < .2)
            {
                newTarget = GenRandPos(startingPos, moveableArea);
                LookAt(newTarget);
                animator.SetBool("Swimming", true);
            }
        }
        else if (!grow.growing && Vector3.Distance(transform.position, startingPos) < .2)
        {
            Debug.Log("Fish is home");
            LookAt(player.transform.position);
            animator.SetBool("Swimming", false);
        }
        else
        {
            Debug.Log("fish is trying to get ho,e");

        
            newTarget = startingPos;
            LookAt(newTarget);
            animator.SetBool("Swimming", true);
        }

        

        transform.position = Vector3.Lerp(transform.position, newTarget, moveSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        //rb.velocity = (transform.position - newTarget).normalized * moveSpeed * Time.fixedDeltaTime;
    }
    Vector3 GenRandPos(Vector3 centrePoint, float range)
    {
        Vector3 randomPos;

        randomPos = new Vector3(Random.Range(-range, range) + centrePoint.x, Random.Range(-range, range) + centrePoint.y, transform.position.z);

        return randomPos;
    }

    void LookAt(Vector3 target)
    {
        Vector3 targetVector = target - transform.position;

        Quaternion lookAt = Quaternion.LookRotation(targetVector * -1);

      
        //Quaternion.Lerp(transform.rotation, lookAt, 5 * Time.deltaTime)
        transform.rotation = lookAt;
    }

}
