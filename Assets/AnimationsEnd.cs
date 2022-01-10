using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimationsEnd : MonoBehaviour
{
    [SerializeField]
    GameObject Pfeil1;
    [SerializeField]
    GameObject Pfeil2;
    [SerializeField]
    GameObject Pfeil3;
    [SerializeField]
    GameObject Pfeil4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void animationComplete()
    {
        Pfeil1.SetActive(false);
        Pfeil2.SetActive(false);
        Pfeil3.SetActive(false);
        Pfeil4.SetActive(false);
    }
}
