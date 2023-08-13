using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUnlock : MonoBehaviour
{
    [SerializeField] List<GameObject> torches = new List<GameObject>();

    [SerializeField] List<GameObject> planets = new List<GameObject>();

    [SerializeField] List<GameObject> particleEffects = new List<GameObject>();

    [SerializeField] CameraShakeController camShake;

    Animator animator;

    bool doorOpened;

    FMOD.Studio.EventInstance openSound;

    bool[] planetUnlocked = new bool[8];

    void Start()
    {
        animator = GetComponent<Animator>();

        for (int i = 0; i < planetUnlocked.Length; i++)
        {
            planetUnlocked[i] = false;
        }
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Minus))
        {
            StartCoroutine(OpenDoor());
        }
        int tochesUnlocked = 0;

        foreach(GameObject torch in torches)
        {
            if (torch.GetComponent<TorchMove>().unlocked)
            {
                tochesUnlocked++;
            }
        }

        if (tochesUnlocked >= torches.Count && !doorOpened)
        {

            StartCoroutine(OpenDoor());


            doorOpened = true;
        }

        for (int i = 0; i < planets.Count; i ++)
        {
            if (torches[i].GetComponent<TorchMove>().unlocked && !planetUnlocked[i])
            {
                Instantiate(particleEffects[i], planets[i].transform);
                planetUnlocked[i] = true;
            }

            if (!torches[i].GetComponent<TorchMove>().unlocked)
            {
                planetUnlocked[i] = false;
            }
        }
    }

    IEnumerator OpenDoor()
    {
        openSound = FMODUnity.RuntimeManager.CreateInstance("event:/Torchs/Unlock");

        openSound.start();
        openSound.release();

        StartCoroutine(DelayParticle());

        yield return new WaitForSeconds(5.7f);

        for (int i = 0; i < planets.Count; i++)
        {
            Instantiate(particleEffects[i], planets[i].transform);
        }

        camShake.ShakeCamera(0.7f, 0.5f);

        yield return new WaitForSeconds(2);


        animator.SetTrigger("OpenDoor");
    }

    IEnumerator DelayParticle()
    {
        for (int i = 0; i < planets.Count; i++)
        {
            Instantiate(particleEffects[i], planets[i].transform);

            yield return new WaitForSeconds(.33f);
        }
    }
}
