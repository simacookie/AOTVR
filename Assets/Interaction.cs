using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField]
    GameObject sword_left;
    [SerializeField]
    GameObject sword_right;
    [SerializeField]
    Animator animationController;
  

    RaycastHit hitleft;
    bool inZone = false;
    public bool isinZone()
    {
        return inZone;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            inZone = true;
            animationController.SetBool("Animation", false);
            sword_right.SetActive(false);
            sword_left.SetActive(false);
        }
    }

    void OnTriggerStay(Collider other)
    {
       

    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            sword_right.SetActive(true);
            sword_left.SetActive(true);
            inZone = false;
            animationController.SetBool("Animation", true);
        }
    }
}
