using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VaultUnlockWheel : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject wheel;
    [SerializeField] float rotationSpeed;

    [SerializeField] bool startWheel;

    FMOD.Studio.EventInstance wheelTickSound;

    float wheelTickIncrement;
    float lastRotation;

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
            //wheel.transform.Rotate(new Vector3(0,1,0) * rotationSpeed * Time.deltaTime);

            if (wheel.transform.rotation.x != lastRotation)
            {
                wheelTickIncrement += Mathf.Abs(Mathf.Abs(wheel.transform.rotation.x) - Mathf.Abs(lastRotation));
                lastRotation = wheel.transform.rotation.x;
            }

            wheel.transform.RotateAround(wheel.transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Q)) 
            {
                rotationSpeed *= -1;

            }

            if (wheelTickIncrement * Mathf.Rad2Deg > 10)
            {
                wheelTickSound = FMODUnity.RuntimeManager.CreateInstance("event:/Vault/VaultWheelTick");
                wheelTickSound.start();
                wheelTickSound.release();

                wheelTickIncrement = 0;
            }

            //Debug.Log("Wheel tick rot: " + wheelTickIncrement);
            Debug.Log("transform rot: " + wheelTickIncrement * Mathf.Rad2Deg);
        }

    }


}
