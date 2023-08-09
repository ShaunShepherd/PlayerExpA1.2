using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUnlock : MonoBehaviour
{
    [SerializeField] List<GameObject> torches = new List<GameObject>();

    Animator animator;

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

        if (tochesUnlocked >= torches.Count)
        {
            Debug.Log("Door Unlcoked");

            animator.SetTrigger("OpenDoor");
        }
    }
}
