using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinUI : MonoBehaviour
{
    [SerializeField] List<GameObject> pins = new List<GameObject>();

    VaultUnlockWheel unlockWheel;

    void Start()
    {
        unlockWheel = GetComponent<VaultUnlockWheel>();
    }

    void Update()
    {
        if (unlockWheel.unlocking) 
        {
            int numUnlcoked = unlockWheel.rotationNumber;

            Debug.Log(numUnlcoked);
            for (int i = 0; i < pins.Count; i++)
            {
                if (numUnlcoked > 0)
                {
                    pins[i].gameObject.SetActive(false);
                    numUnlcoked--;
                }
                else
                {
                    pins[i].gameObject.SetActive(true);
                }

            }
        }
        else
        {
            for (int i = 0; i < pins.Count; i++)
            {
                pins[i].gameObject.SetActive(false);
            }
        }
    }
}
