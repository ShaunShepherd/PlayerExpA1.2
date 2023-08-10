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

    bool swimSoundPlaying;

    FMOD.Studio.EventInstance swimSound;

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

                if (!swimSoundPlaying)
                {
                    swimSound = FMODUnity.RuntimeManager.CreateInstance("event:/Pufferfish/FishSwim");
                    swimSound.start();

                    swimSoundPlaying = true;
                }
            }
        }
        else if (!grow.growing && Vector3.Distance(transform.position, startingPos) < .2)
        {
            LookAt(player.transform.position);
            animator.SetBool("Swimming", false);

            swimSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            swimSound.release();

            swimSoundPlaying = false;
        }
        else
        {
            newTarget = startingPos;
            LookAt(newTarget);
            animator.SetBool("Swimming", true);

            if (!swimSoundPlaying)
            {
                swimSound = FMODUnity.RuntimeManager.CreateInstance("event:/Pufferfish/FishSwim");
                swimSound.start();

                swimSoundPlaying = true;
            }
        }
    
        transform.position = Vector3.Lerp(transform.position, newTarget, moveSpeed * Time.deltaTime);
    }

    void OnDestroy()
    {
        swimSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        swimSound.release();

        swimSoundPlaying = false;
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
