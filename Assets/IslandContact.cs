using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Bei Kontakt mit Insel
    public void OnTriggerEnter(Collider other)
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
