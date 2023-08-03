using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TorchMove : MonoBehaviour
{
    [SerializeField] TMP_Text uiText;
    [SerializeField] GameObject player;
    [SerializeField] float movementDrag;
    [SerializeField] Transform playerHolder;
    [SerializeField] float maxDistance;
    [SerializeField] float unlockDistance;
     
    bool equipt = false;
    bool playerInTrigger;

    float distanceOffset;
    float minDistance;

    private void Start()
    {
        minDistance = transform.position.z;
        maxDistance = transform.position.z - maxDistance;
    }

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (equipt)
            {
                equipt= false;

                if (!playerInTrigger) 
                {
                    uiText.gameObject.SetActive(false);
                }

                player.GetComponent<PlayerMovement>().torchEquipt = false;
            }
            else
            { 
                equipt= true;

                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

                playerMovement.torchEquipt = true;

                playerMovement.MoveToPos(playerHolder);

                distanceOffset = player.transform.position.z - transform.position.z;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && !playerInTrigger && equipt) 
        {
            equipt = false;

            uiText.gameObject.SetActive(false);

            player.GetComponent<PlayerMovement>().torchEquipt = false;
        }

        if (playerInTrigger && !equipt)
        {
            uiText.text = "Press E to move torch";
        }

        if (equipt) 
        {
            uiText.text = "Press E to let go";

            Debug.Log("move");

            MoveWithPlayer();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInTrigger = true;

            uiText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInTrigger = false;

            if (!equipt)
            {
                uiText.gameObject.SetActive(false);
            }
        }
    }

    void MoveWithPlayer()
    {
        float targetZPos = Mathf.Clamp((player.transform.position.z - distanceOffset), maxDistance, minDistance);

        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, targetZPos), movementDrag);
    }    
}
