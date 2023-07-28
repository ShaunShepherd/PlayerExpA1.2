using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VaultUnlockWheel : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject wheel;
    [SerializeField] float rotationSpeed;
    [SerializeField] float wheelTickIncrements;

    [SerializeField] bool startWheel;

    FMOD.Studio.EventInstance wheelTickSound;
    FMOD.Studio.EventInstance pinUnlockSound;

    float wheelTickTimer;
    float tickCount;

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
            if (wheelTickTimer > 0) 
            {
                wheelTickTimer -= Time.deltaTime;
            }
            else
            {
                tickCount++;

                if (tickCount == 4)
                {
                    pinUnlockSound = FMODUnity.RuntimeManager.CreateInstance("event:/Vault/VaultPinUnlock");
                    pinUnlockSound.start();
                    pinUnlockSound.release();
                }
                else 
                {
                    wheelTickSound = FMODUnity.RuntimeManager.CreateInstance("event:/Vault/VaultWheelTick");
                    wheelTickSound.start();
                    wheelTickSound.release();
                }

                wheelTickTimer = wheelTickIncrements;
            }

            wheel.transform.RotateAround(wheel.transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Q)) 
            {
                if (tickCount ==4)
                {
                    rotationSpeed *= -1;
                    tickCount = 0;
                    Debug.Log("GOT IT");
                }
                else
                {
                    rotationSpeed *= -1;
                }
            }
        }
    }
}
