using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TorchMove : MonoBehaviour, IInteractable
{
    [SerializeField] TMP_Text uiText;

    bool equipt;
    bool playerInTrigger;
    void IInteractable.Interact()
    {
        if (equipt)
        {
            equipt = false;
        }
        else
        {
            equipt = true;
        }
    }

    void IInteractable.LookAt()
    {
        
    }

    void Update()
    {
        if (equipt) 
        {
            if (Input.GetKeyUp(KeyCode.E)) 
            {
                equipt = false;
            }
            Debug.Log("move");
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }
}
