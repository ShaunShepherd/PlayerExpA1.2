using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultUnlockWheel : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject wheel;
    [SerializeField] float rotationSpeed;

    [SerializeField] bool startWheel;

    public void Interact()
    {
        startWheel = true;  
    }

    public void LookAt()
    {

    }

    void Update()
    {
        if (startWheel) 
        {
            wheel.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Q)) 
            {
                rotationSpeed *= -1;
            }
        }

    }


}
