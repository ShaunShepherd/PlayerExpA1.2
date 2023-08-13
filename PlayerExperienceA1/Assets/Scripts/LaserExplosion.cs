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
    [SerializeField] CameraShakeController camShake;

    [SerializeField] float numberOfPops;

    [SerializeField] GameObject trail;


    bool fired = false;
    bool canFire = true;
    bool playerInRange;

    float popCount = 1;

    GameObject trailGO;
    Animator animator;

    Vector3 targetVector;

    FMOD.Studio.EventInstance buttonPressSound;

    public void Interact()
    {
        if (!fired && canFire)
        {
            animator.SetTrigger("ButtonPress");

            buttonPressSound = FMODUnity.RuntimeManager.CreateInstance("event:/Explosion/ButtonPress");
            buttonPressSound.start();
            buttonPressSound.release();

            StartCoroutine(StartSequence());

            uiText.gameObject.SetActive(false);
        }
    }

    public void LookAt()
    {

    }

    void Start()
    {
        particleSpawner.SetActive(false);

        particleSpawner.transform.position = laserOrigin.position;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(buttonPressSound, GetComponent<Transform>());
        if (fired && !canFire)
        {
            MoveSpawner();

            if (Vector3.Distance(particleSpawner.transform.position, laserEnd.position) < 0.1)
            {
                Destroy(trailGO);

                ParticleSpawn(initalPop);
                ParticleSpawn(smokeParticle);
                ParticleSpawn(puffParticle);

                StartCoroutine(DelayShake());

                particleSpawner.SetActive(false);

                fired = false;

                particleSpawner.transform.position = laserOrigin.position;

                SpawnExplosion(explosionSpawn.position);

                canFire = true;
                fired = false;

                popCount = 1;

            }
        }

        if (playerInRange && canFire)
        {
            uiText.gameObject.SetActive(true);
        }
        else
        {
            uiText.gameObject.SetActive(false);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
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
        canFire = false;

        ParticleSpawn(chargeUpParticle);

        yield return new WaitForSeconds(3);

        fired = true;

        ParticleSpawn(smallPopsParticle);

        particleSpawner.SetActive(true);

        trailGO = Instantiate(trail, particleSpawner.transform);

        targetVector = laserEnd.position - laserOrigin.position;
    }

    IEnumerator DelayShake()
    {
        yield return new WaitForSeconds(0.3f);

        camShake.ShakeCamera(4, 0.8f);
        canFire = true;
    }
}
