using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandContact : MonoBehaviour
{
    public bool artFest;
    public bool artTimer;

    public IslandFall jetztDran;
    public InselArt art;
    public InselTimer timer;

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
        // Konsistenz der Insel nach Kontakt ist Fest
        if (art == InselArt.fest)
        {
            artFest = true;
                if (timer == InselTimer.timer)
                {
                    artTimer = true;
                }
                if (timer == InselTimer.noneTimer)
                {
                    artTimer = false;
                }
        }
        // Konsistenz der Insel nach Kontakt ist Bruch
        if (art == InselArt.bruch)
        {
            artFest = false;
                if (timer == InselTimer.timer)
                {
                    artTimer = true;
                }
                if (timer == InselTimer.noneTimer)
                {
                    artTimer = false;
                }
        }
        jetztDran.fallen();
    }
}
