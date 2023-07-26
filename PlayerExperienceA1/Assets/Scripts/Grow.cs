using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grow : MonoBehaviour, IInteractable
{
    [SerializeField] float growRate;
    [SerializeField] float maxSize;
    [SerializeField] ParticleSystem popParticles;
    public void Interact()
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
