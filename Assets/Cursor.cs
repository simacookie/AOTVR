using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    [SerializeField]
    float cursorSize;
    [SerializeField]
    Transform camera;
    public RaycastHit hit;
    [SerializeField]
    public GameObject cursor;
    [SerializeField]
    LayerMask layerMask;
    public bool hitting;
    // Start is called before the first frame update
    void Start()
    {
        
    }
	private void LateUpdate()
	{
        float newSize = (camera.position - cursor.transform.position).magnitude * cursorSize;
        cursor.transform.localScale = new Vector3(newSize, newSize, newSize);

    }

    public void SetHitIndicator(bool hit)
    {
        if (hit) cursor.GetComponent<SpriteRenderer>().color = Color.yellow;
        else cursor.GetComponent<SpriteRenderer>().color = Color.white;
    }
    // Update is called once per frame
    void Update()
    {


        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.

        
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            hitting = true;
            cursor.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }
        else
        {
            hitting = false;

            cursor.transform.position = transform.position + transform.forward*100;

        }
        // Bit shift the index of the layer (8) to get a bit mask
        
        SetHitIndicator(hitting);


        // Does the ray intersect any objects excluding the player layer
    }
}
