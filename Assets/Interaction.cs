using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField]
    GameObject sword_left;
    [SerializeField]
    GameObject sword_right;

    void OnTriggerStay(Collider other)
    {
        Debug.Log(" Funktioniert soweit");
        sword_right.SetActive(false);
        sword_left.SetActive(false);
    }
}
