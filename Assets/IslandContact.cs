using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class IslandContact : MonoBehaviour
{
    public bool bruch;
    public bool timer;
    public bool hookcount;

    public float secondsToFall;
    public int hooksToFall;
    public int actualHooks = 0;

    public GameObject inselStandard;
    public GameObject inselBruch;

    public CharacterMovement characterMovement;
    public HookState oldLeftHookState = HookState.pulledIn;
    public HookState oldRightHookState = HookState.pulledIn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (oldLeftHookState != characterMovement.leftHookState && characterMovement.leftHookState == HookState.connected ||
            oldRightHookState != characterMovement.rightHookState && characterMovement.rightHookState == HookState.connected) 
        {
            // Hat einen Timer
            if (timer == true)
            {
                StartCoroutine(ExampleCoroutine());
            }
            // Hat einen Hookcount
            if (hookcount == true)
            {
                HookZaehler();
            }
        }
        oldLeftHookState = characterMovement.leftHookState;
        oldRightHookState = characterMovement.rightHookState;*/
    }

    // Bei Kontakt mit Insel
    public void Kollision(GameObject gameObject)
    {
        if(gameObject.tag == "grapple") {
            float amplitude = 0.1f;
            float duration =1f;
            var device = InputSystem.GetDevice<XRController>(CommonUsages.RightHand);
            var command = UnityEngine.InputSystem.XR.Haptics.SendHapticImpulseCommand.Create(0, amplitude, duration);
            device.ExecuteCommand(ref command);
            float amplitude2 = 0.1f;
            float duration2 = 1f;
            var device2 = InputSystem.GetDevice<XRController>(CommonUsages.LeftHand);
            var command2 = UnityEngine.InputSystem.XR.Haptics.SendHapticImpulseCommand.Create(0, amplitude2, duration2);
            device2.ExecuteCommand(ref command2);

            if (oldLeftHookState != characterMovement.leftHookState && characterMovement.leftHookState == HookState.connected ||
                oldRightHookState != characterMovement.rightHookState && characterMovement.rightHookState == HookState.connected)
            {
                // Hat einen Timer
                if (timer == true)
                {
                    StartCoroutine(ExampleCoroutine());
                }
                // Hat einen Hookcount
                if (hookcount == true)
                {
                    HookZaehler();
                }
            }
            oldLeftHookState = characterMovement.leftHookState;
            oldRightHookState = characterMovement.rightHookState;
        }
    }

    // Timermethode
    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(secondsToFall);
        inselBruch.SetActive(true);
        inselStandard.SetActive(false);
    }

    // Hookmethode
    void HookZaehler()
    {
        if(actualHooks >= hooksToFall)
        {
            inselBruch.SetActive(true);
            inselStandard.SetActive(false);
        }
        else
        {
            actualHooks++;
        }
    }
}
