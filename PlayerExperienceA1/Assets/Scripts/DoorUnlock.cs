using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUnlock : MonoBehaviour
{
    [SerializeField] List<GameObject> torches = new List<GameObject>();

    Animator animator;

    bool doorOpened;

    FMOD.Studio.EventInstance openSound;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
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
    }

    IEnumerator OpenDoor()
    {
        openSound = FMODUnity.RuntimeManager.CreateInstance("event:/Torchs/Unlock");

        openSound.start();
        openSound.release();

        yield return new WaitForSeconds(7);

        animator.SetTrigger("OpenDoor");
    }
}
