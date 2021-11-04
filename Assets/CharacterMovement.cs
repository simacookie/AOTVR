using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public GameObject gripRight;
    public InputActionReference triggerPress;

    CharacterController characterController;
    const float GRAVITY = 0.15f;
    float currentGravity = 0;
    [Header("Physics stats")]

    [SerializeField]
    float speedX = 0;
    [SerializeField]
    float speedY = 0;
    [SerializeField]
    float speedZ = 0;
    [SerializeField]
    float accelerationX = 0;
    [SerializeField]
    float accelerationY = 0;
    [SerializeField]
    float accelerationZ = 0;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    
    void Update()
    {
        //Inputs
        float value = triggerPress.action.ReadValue<float>();
        if (value > 0.5)
        {
            accelerationX = gripRight.transform.forward.x;
            accelerationY = gripRight.transform.forward.y;
            accelerationZ = gripRight.transform.forward.z;
        }
        else
        {
            accelerationX = 0;
            accelerationY = 0;
            accelerationZ = 0;
        }

        //Movement
        Vector3 movementVec = new Vector3(speedX, speedY, speedZ);
        characterController.Move(movementVec * Time.deltaTime);
    }
    private void CalculateNewSpeed()
    {
        speedX += accelerationX;
        speedY += accelerationY - GRAVITY;
        speedZ += accelerationZ;
    }
	private void FixedUpdate()
	{
        CalculateNewSpeed();

        //friction
        if (characterController.isGrounded)
        {
            float friction = 0.9f;
            speedX *= friction;
            speedY *= friction;
            speedZ *= friction;
            Debug.Log("FRICTION");
        }

    }
}
