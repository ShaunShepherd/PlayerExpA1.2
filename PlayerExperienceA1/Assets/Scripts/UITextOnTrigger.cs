using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextOnTrigger : MonoBehaviour
{
    [SerializeField] TMP_Text uiText;
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
        if (playerInRange)
        {
            if (!unlockWheel.startWheel)
            {
                uiText.text = prompt;
            }
            else
            {
                uiText.text = "Press 'Q' When the Pin is in Place";
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
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
