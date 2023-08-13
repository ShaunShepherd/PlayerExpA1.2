using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class VaultUnlockWheel : MonoBehaviour, IInteractable
{
    public bool doorOpened;
    public bool startWheel;
    public bool playerInRange;
    public int rotationNumber;

    [SerializeField] GameObject wheel;
    [SerializeField] float rotationSpeed;
    [SerializeField] float wheelTickIncrements;
    [SerializeField] int amountOfPins;
    [SerializeField] int minTicks;
    [SerializeField] int maxTicks;
    [SerializeField] ParticleSystem tickParticles;
    [SerializeField] ParticleSystem pinParticles;
    [SerializeField] ParticleSystem failParticles;
    [SerializeField] GameObject player;
    [SerializeField] Transform playerHolder;
    [SerializeField] Transform smallWheel;

    FMOD.Studio.EventInstance wheelTickSound;
    FMOD.Studio.EventInstance pinUnlockSound;
    FMOD.Studio.EventInstance vaultLocked;
    FMOD.Studio.EventInstance doorOpenSound;
    FMOD.Studio.EventInstance handleRespin;

    float wheelTickTimer;
    float tickCount;
    public int[] pinNumbers;

    public bool unlocking;


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
        if (!doorOpened)
        {
            if (!unlocking)
            {
                startWheel = true;

                GeneratePinNumbers(minTicks, maxTicks, amountOfPins, pinNumbers);

                rotationNumber = 0;

                player.GetComponent<PlayerMovement>().torchEquipt = true;

                unlocking = true;

                player.transform.position = new Vector3(playerHolder.position.x, player.transform.position.y, playerHolder.position.z);
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    unlocking = false;
                    StartCoroutine(StopWheel());
                    startWheel = false;

                    player.GetComponent<PlayerMovement>().torchEquipt = false;
                }
            }
        }
    }

    public void LookAt()
    {

    }

    void Update()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(doorOpenSound, smallWheel);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(handleRespin, smallWheel);

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

                        Instantiate(pinParticles, smallWheel);

                        pinUnlockSound.release();
                    }
                    else if (pinNumbers[rotationNumber] < tickCount)
                    {
                        ResetWheel();
                    }
                    else
                    {
                        Instantiate(tickParticles, smallWheel);

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
                            doorOpenSound = FMODUnity.RuntimeManager.CreateInstance("event:/Vault/VaultDoorOpen");
                            doorOpenSound.start();
                            doorOpenSound.release();

                            animator.SetTrigger("DoorOpen");

                            Instantiate(pinParticles, smallWheel);

                            doorOpened = true;

                            rotationNumber++;
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
        else
        {
            player.GetComponent<PlayerMovement>().torchEquipt = false;
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

        Instantiate(failParticles, smallWheel);

        StartCoroutine(WheelFailedDelay());
    }

    IEnumerator StopWheel()
    {
        animator.SetTrigger("Respin");

        handleRespin = FMODUnity.RuntimeManager.CreateInstance("event:/Vault/VaultDoorRespin");
        handleRespin.start();

        handleRespin.release();

        startWheel = false;

        yield return new WaitForSeconds(1);

        rotationNumber = 0;

        tickCount = 0;

        wheel.transform.rotation = startingRotation;
    }

    IEnumerator WheelFailedDelay()
    {
        animator.SetTrigger("Respin");

        handleRespin = FMODUnity.RuntimeManager.CreateInstance("event:/Vault/VaultDoorRespin");
        handleRespin.start();

        handleRespin.release();

        startWheel = false;

        yield return new WaitForSeconds(1);

        startWheel = true;

        GeneratePinNumbers(minTicks, maxTicks, amountOfPins, pinNumbers);

        rotationNumber = 0;

        tickCount = 0;

        wheel.transform.rotation = startingRotation;
    }
}
