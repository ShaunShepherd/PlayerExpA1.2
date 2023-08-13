using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserExplosion : MonoBehaviour, IInteractable
{
    [SerializeField] float explosionRadius = 5;
    [SerializeField] float explosionForce = 500;
    [SerializeField] float laserMoveSpeed;

    [SerializeField] GameObject uiText;
    [SerializeField] Transform explosionSpawn;
    [SerializeField] Transform laserOrigin;
    [SerializeField] Transform laserEnd;
    [SerializeField] GameObject particleSpawner;
    [SerializeField] GameObject initalPop;
    [SerializeField] GameObject smokeParticle;
    [SerializeField] GameObject puffParticle;
    [SerializeField] GameObject smallPopsParticle;
    [SerializeField] GameObject chargeUpParticle;

    [SerializeField] float numberOfPops;

    [SerializeField] GameObject trail;


    bool fired = false;
    bool canFire = true;

    float popCount = 1;

    GameObject trailGO;

    Vector3 targetVector;

    public void Interact()
    {
        if (!fired && canFire)
        {
            StartCoroutine(StartSequence());
        }
    }

    public void LookAt()
    {

    }

    void Start()
    {
        particleSpawner.SetActive(false);

        particleSpawner.transform.position = laserOrigin.position;
    }

    void Update()
    {
        if (fired && !canFire)
        {
            MoveSpawner();

            if (Vector3.Distance(particleSpawner.transform.position, laserEnd.position) < 0.1)
            {
                Destroy(trailGO);

                ParticleSpawn(initalPop);
                ParticleSpawn(smokeParticle);
                ParticleSpawn(puffParticle);

                particleSpawner.SetActive(false);

                particleSpawner.transform.position = laserOrigin.position;

                SpawnExplosion(explosionSpawn.position);

                canFire = true;
                fired = false;

                popCount = 1;

            }
        }
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

    void MoveSpawner()
    {
        if (Vector3.Distance(trailGO.transform.position, particleSpawner.transform.position) < 0.1)
        {
            //trailGO.transform.position = laserEnd.position;
        }

        Debug.Log("The pop count " + numberOfPops);

        Debug.Log("The pop inc dist: " + (popCount / numberOfPops * Vector3.Distance(laserOrigin.position, laserEnd.position)));

        if (Vector3.Distance(particleSpawner.transform.position, laserOrigin.position) > (popCount/numberOfPops * Vector3.Distance(laserOrigin.position, laserEnd.position)))
        {
            ParticleSpawn(smallPopsParticle);

            popCount++;
        }

        particleSpawner.transform.Translate(targetVector.normalized * Time.deltaTime * laserMoveSpeed);
    }

    void ParticleSpawn(GameObject go)
    {
        var initalPopPar = Instantiate(go, particleSpawner.transform);
        initalPopPar.transform.parent = null;
    }

    IEnumerator StartSequence()
    {
        ParticleSpawn(chargeUpParticle);

        yield return new WaitForSeconds(3);

        ParticleSpawn(smallPopsParticle);

        particleSpawner.SetActive(true);

        trailGO = Instantiate(trail, particleSpawner.transform);

        targetVector = laserEnd.position - laserOrigin.position;

        canFire = false;
        fired = true;
    }
}
