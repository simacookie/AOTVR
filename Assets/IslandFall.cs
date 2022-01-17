using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandFall : MonoBehaviour
{
    bool fall;
    bool gebrochen;
    bool kontakt;
    bool zeitbasiert;

    public GameObject inselFest;
    public GameObject inselFall;
    public GameObject inselBruch1;
    public GameObject inselBruch2;
    public GameObject inselBruch3;
    public GameObject inselBruch4;

    // Start is called before the first frame update
    void Start()
    {
        fall = false;
        gebrochen = false;
        kontakt = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // 
    /*public void verbinden()
    {
        if (inselFestTimer == isActiveAndEnabled)
        {
            zeitbasiert = true;
            inselFestTimer.SetActive(false);
        }
        else
        {
            zeitbasiert = false;
            inselFest.SetActive(false);
        }
    }*/

    // Insel faellt
    public void fallen()
    {
    /*    if ()
        {
            // weitere Abfragen nach Menge der Inselteile
        }
        else
        {
            
        }*/
    }
}
public enum InselArt
{
    fest, bruch
}

public enum InselTimer
{
    timer, noneTimer
}
