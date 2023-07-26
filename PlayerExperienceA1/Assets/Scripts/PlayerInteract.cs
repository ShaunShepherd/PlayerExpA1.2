using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera playerCamera;

    List<GameObject> objectsInRange = new List<GameObject>();

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E)) 
        {
            if (objectsInRange.Count > 0) 
            {
                objectsInRange[0].gameObject.GetComponent<IInteractable>().Interact();
            }
        }
    }
    void FixedUpdate()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitInfo))
        {
            IInteractable interactable = hitInfo.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.LookAt();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.gameObject.GetComponent<IInteractable>();

        if (interactable != null)
        {
            objectsInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.gameObject.GetComponent<IInteractable>();

        if (interactable != null)
        {
            objectsInRange.Remove(other.gameObject);
        }
    }
}
