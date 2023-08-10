using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grow : MonoBehaviour, IInteractable
{
    public bool growing = false;

    [SerializeField] float growRate;
    [SerializeField] float maxSize;
    [SerializeField] float shrinkDelay;
    [SerializeField] ParticleSystem popParticles;

    float previousScale;
    float startingScale;
    float shrinkDelayTimer;

    void Start()
    {
        startingScale = transform.transform.localScale.x;

        previousScale = startingScale;
    }

    public void Interact()
    {

    }

    void Update()
    {
        if (transform.localScale.x > previousScale)
        {
            growing = true;

            previousScale = transform.localScale.x;

            shrinkDelayTimer = 0;
        }
        else
        {
            previousScale = transform.localScale.x;

            shrinkDelayTimer += Time.deltaTime;

            if (shrinkDelayTimer > shrinkDelay)
            {
                Shrink();
            }
        }


    }

    void Shrink()
    {
        if (transform.localScale.x > startingScale)
        {
            transform.localScale /= 1 + growRate / 1000;

            growing = false;
        }
    }

    public void LookAt()
    {
        if (transform.localScale.x < maxSize)
        {
            transform.localScale *= 1 + growRate / 100;
        }
        else
        {
            var particles = Instantiate(popParticles, transform);
            particles.transform.parent = null;
            Destroy(gameObject);
        }
    }
}
