using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
  
    [SerializeField]
    GameObject sword_left;
    [SerializeField]
    GameObject sword_right;

    RaycastHit hitleft;
    bool inZone = false;
    public bool isinZone()
    { 
        return inZone;
    }

    // Start is called before the first frame update
    void Start() { 
      
    }

// Update is called once per frame
void Update()
    {
        
    }
   void OnTriggerEnter(Collider other)
    {
        inZone = true;
    }

    void OnTriggerStay(Collider other)
    {
        sword_right.SetActive(false);
        sword_left.SetActive(false);

    }
    void OnTriggerExit(Collider other)
    {
        sword_right.SetActive(true);
        sword_left.SetActive(true);
        inZone = false;
    }
}
