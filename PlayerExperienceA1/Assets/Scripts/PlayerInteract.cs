using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera playerCamera;

    void FixedUpdate()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitInfo))
        {
            IInteractable interactable = hitInfo.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}
