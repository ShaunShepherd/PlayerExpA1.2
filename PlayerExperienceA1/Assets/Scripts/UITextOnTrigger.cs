using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextOnTrigger : MonoBehaviour
{
    [SerializeField] TMP_Text uiText;
    [SerializeField] TMP_Text exitUIText;
    [SerializeField] string prompt;

    VaultUnlockWheel unlockWheel;
    bool playerInRange;

    void Start()
    {
        uiText.gameObject.SetActive(false);

        unlockWheel = GetComponent<VaultUnlockWheel>();
    }

    void Update()
    {
        if (playerInRange && !unlockWheel.doorOpened)
        {
            if (!unlockWheel.startWheel)
            {
                uiText.text = prompt;
                exitUIText.gameObject.SetActive(false);
            }
            else
            {
                uiText.text = "press 'q' when the pin is in place";
                exitUIText.gameObject.SetActive(true);
            }
        }
        else
        {
            exitUIText.gameObject.SetActive(false);
        }

        if (unlockWheel.doorOpened)
        {
            uiText.gameObject.SetActive(false);
            exitUIText.gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !unlockWheel.doorOpened)
        {
            uiText.gameObject.SetActive(true);
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            uiText.gameObject.SetActive(false);
            playerInRange = false;
        }
    }
}
