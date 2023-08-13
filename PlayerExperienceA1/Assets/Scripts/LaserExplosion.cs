using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserExplosion : MonoBehaviour, IInteractable
{
    [SerializeField] float explosionRadius = 5;
    [SerializeField] float explosionForce = 500;

    [SerializeField] GameObject uiText;
    [SerializeField] Transform explosionSpawn;

    public void Interact()
    {
        SpawnExplosion(explosionSpawn.position);
    }

    public void LookAt()
    {

    }

    void Start()
    {

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            uiText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            uiText.gameObject.SetActive(false);
        }
    }

    void SpawnExplosion(Vector3 spawnPos)
    {
        var surroundingObjects = Physics.OverlapSphere(spawnPos, explosionRadius);



        foreach (var obj in surroundingObjects)
        {
            var rb = obj.GetComponent<Rigidbody>();

            if (rb == null) continue;

            rb.AddExplosionForce(explosionForce, spawnPos, explosionRadius);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(explosionSpawn.position, explosionRadius);
    }
}
