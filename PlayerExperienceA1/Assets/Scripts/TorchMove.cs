using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TorchMove : MonoBehaviour
{
    public bool unlocked;
    [SerializeField] TMP_Text uiText;
    [SerializeField] GameObject player;
    [SerializeField] float movementDrag;
    [SerializeField] Transform playerHolder;
    [SerializeField] float maxDistance;
    [SerializeField] float unlockDistance;
    [SerializeField] float distanceBuffer;
    [SerializeField] float hummBuffer;
     
    bool equipt = false;
    bool playerInTrigger;
    bool dragSoundPlaying;
    bool closeHummPlaying;

    float distanceOffset;
    float minDistance;

    FMOD.Studio.EventInstance pickUpSound;
    FMOD.Studio.EventInstance dropSound;
    FMOD.Studio.EventInstance dragSound;
    FMOD.Studio.EventInstance closeHummSound;
    FMOD.Studio.EventInstance correctHummSound;

    private void Start()
    {
        minDistance = transform.position.z;
        maxDistance = transform.position.z - maxDistance;

        unlockDistance = transform.position.z - unlockDistance;

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

                dropSound = FMODUnity.RuntimeManager.CreateInstance("event:/Torchs/TorchDrop");
                dropSound.start();
                dropSound.release();

                dragSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                dragSound.release();

                dragSoundPlaying = false;
            }
            else
            { 
                equipt= true;

                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

                playerMovement.torchEquipt = true;

                playerMovement.MoveToPos(playerHolder);

                distanceOffset = player.transform.position.z - transform.position.z;

                pickUpSound = FMODUnity.RuntimeManager.CreateInstance("event:/Torchs/TorchPickup");
                pickUpSound.start();
                pickUpSound.release();
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

            MoveWithPlayer();
        }

        if (transform.position.z > (unlockDistance - distanceBuffer) && transform.position.z < (unlockDistance + distanceBuffer) && !equipt)
        {
            if (closeHummPlaying)
            {
                correctHummSound = FMODUnity.RuntimeManager.CreateInstance("event:/Torchs/CorrectHumm");
                correctHummSound.start();
                correctHummSound.release();

                closeHummSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                closeHummSound.release();

                closeHummPlaying = false;
            }

            unlocked = true;
        }
        else
        {
            unlocked = false;
        }

        if (equipt)
        {
            PlayHummWhenClose();
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

        if (Mathf.Abs(player.GetComponent<Rigidbody>().velocity.z) > 1)
        {
            if (!dragSoundPlaying)
            {
                dragSound = FMODUnity.RuntimeManager.CreateInstance("event:/Torchs/TorchDrag");
                dragSound.start();

                dragSoundPlaying = true;
            }
        }
        else
        {
            dragSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            dragSound.release();

            dragSoundPlaying = false;
        }


        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, targetZPos), movementDrag);
    }    

    void PlayHummWhenClose()
    {
        if (transform.position.z > (unlockDistance - hummBuffer) && transform.position.z < (unlockDistance + hummBuffer))
        {
            float pitch = (12/hummBuffer) * Mathf.Abs(Mathf.Abs(Mathf.Abs(unlockDistance) - Mathf.Abs(transform.position.z)) - hummBuffer);

            closeHummSound.setParameterByName("Pitch", pitch);
            if (!closeHummPlaying)
            {
                closeHummSound = FMODUnity.RuntimeManager.CreateInstance("event:/Torchs/CloseHumm");
                closeHummSound.start();

                closeHummPlaying = true;
            }
        }
        else
        {
            closeHummSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            closeHummSound.release();

            closeHummPlaying = false;
        }
    }
}
