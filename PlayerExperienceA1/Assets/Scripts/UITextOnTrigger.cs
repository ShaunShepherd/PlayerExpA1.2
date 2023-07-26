using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextOnTrigger : MonoBehaviour
{
    [SerializeField] TMP_Text uiText;
    [SerializeField] string prompt;

    void Start()
    {
        uiText.gameObject.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            uiText.gameObject.SetActive(true);
            uiText.text = prompt;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            uiText.gameObject.SetActive(false);
        }
    }
}
