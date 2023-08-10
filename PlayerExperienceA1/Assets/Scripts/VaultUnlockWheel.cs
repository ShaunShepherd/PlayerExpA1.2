using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class VaultUnlockWheel : MonoBehaviour, IInteractable
{
    public bool startWheel;
    public bool playerInRange;
    public int rotationNumber;

    [SerializeField] GameObject wheel;
    [SerializeField] float rotationSpeed;
    [SerializeField] float wheelTickIncrements;
    [SerializeField] int amountOfPins;
    [SerializeField] int minTicks;
    [SerializeField] int maxTicks;
    [SerializeField] ParticleSystem unlockParticles;
    [SerializeField] ParticleSystem clickParticles;
    [SerializeField] ParticleSystem failParticles;

    FMOD.Studio.EventInstance wheelTickSound;
    FMOD.Studio.EventInstance pinUnlockSound;
    FMOD.Studio.EventInstance vaultLocked;

    float wheelTickTimer;
    float tickCount;
    public int[] pinNumbers;

    bool doorOpened;

    Quaternion startingRotation;
    Animator animator;

    void Start()
    {
        pinNumbers= new int[amountOfPins];
        
        animator = GetComponent<Animator>();

        startingRotation = wheel.transform.rotation;
    }
    public void Interact()
    {
        startWheel = true;

        GeneratePinNumbers(minTicks, maxTicks, amountOfPins, pinNumbers);

        rotationNumber= 0;
    }

    public void LookAt()
    {

    }

    void Update()
    {
        if (!doorOpened) 
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

                    if (pinNumbers[rotationNumber] == tickCount)
                    {
                        pinUnlockSound = FMODUnity.RuntimeManager.CreateInstance("event:/Vault/VaultPinUnlock");
                        pinUnlockSound.start();
                        var particles = Instantiate(clickParticles, transform);                       
                        particles.transform.parent = null;
                        pinUnlockSound.release();
                    }
                    else if (pinNumbers[rotationNumber] < tickCount)
                    {
                        ResetWheel();
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
                    if (pinNumbers[rotationNumber] == tickCount)
                    {
                        rotationSpeed *= -1;
                        tickCount = 0;

                        if (rotationNumber == amountOfPins - 1)
                        {
                            animator.SetTrigger("DoorOpen");
                            var particles = Instantiate(unlockParticles, transform);
                            particles.transform.parent = null;

                            doorOpened = true;
                        }
                        else
                        {
                            rotationNumber++;
                        }
                    }
                    else
                    {
                        Debug.Log("Reset");
                        ResetWheel();
                    }
                }
            }
        }
  
    }

    void GeneratePinNumbers(int minRange, int MaxRange, int countOfIteration, int[] storageArray)
    {
        for (int i = 0; i < countOfIteration; i++) 
        {
            storageArray[i] = Random.Range(minRange, MaxRange);
        }
    }

    void ResetWheel() 
    {
        vaultLocked = FMODUnity.RuntimeManager.CreateInstance("event:/Vault/VaultLocked(failed)");
        vaultLocked.start();
        vaultLocked.release();

        StartCoroutine(WheelFailedDelay());
    }

    IEnumerator WheelFailedDelay()
    {
        startWheel = false;

        yield return new WaitForSeconds(1);

        startWheel = true;

        GeneratePinNumbers(minTicks, maxTicks, amountOfPins, pinNumbers);

        rotationNumber = 0;

        tickCount = 0;

        wheel.transform.rotation = startingRotation;
    }
}
