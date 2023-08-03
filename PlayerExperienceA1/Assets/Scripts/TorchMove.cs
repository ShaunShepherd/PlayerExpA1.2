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

    bool equipt = false;
    bool playerInTrigger;

    float distanceOffset;


    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (equipt)
            {
                equipt= false;

                player.GetComponent<PlayerMovement>().torchEquipt = false;

                uiText.gameObject.SetActive(false);
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
        }
    }

    void MoveWithPlayer()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, player.transform.position.z - distanceOffset), movementDrag);
        //transform.Translate(Vector3.up * (playerTrans.position.z - transform.position.z));
    }    
}
