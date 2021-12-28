using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public float anchorSpeed;
    public GameObject gripRight;
    public Transform ropeSourceRight;
    public Transform ropeSourceLeft;
    public InputActionReference triggerPressLeft;
    public InputActionReference triggerPressRight;
    public InputActionReference ForwardPressRight;
    public InputActionReference triggerAxisLeft;
    public InputActionReference triggerAxisRight;
    Vector3 currentTargetPointRightAnchor;
    Vector3 currentTargetPointLeftAnchor;
    [SerializeField]
    GameObject anchorLeft;
    [SerializeField]
    GameObject anchorRight;
    [SerializeField]
    GameObject head;
    [SerializeField]
    Cursor cursorLeft;
    [SerializeField]
    Cursor cursorRight;
    CharacterController characterController;
    const float GRAVITY = 0.15f;
    float currentGravity = 0;
    [Header("Physics stats")]
    // bool to make it trigger only the first frame of the press
    private bool triggerLeftPressed = false;
    private bool triggerRightPressed = false;
    private bool leftAnchorFlying = false;
    private bool rightAnchorFlying = false;
    private bool leftCursorAvailable = false;
    private bool rightCursorAvailable = false;
    public HookState leftHookState = HookState.pulledIn; 
    public HookState rightHookState = HookState.pulledIn;
	private List<Vector3> accelerationVecs = new List<Vector3>()
	{
	};
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
    [SerializeField]
    float gasDuration = 0.2f;
    [SerializeField]
    float gasCooldown = 1;
    [SerializeField]
    float gasStrength = 3;
	bool gasInUse;
    float distanceTravelledRight;
    float distanceTravelledLeft;
    [SerializeField]
	private float ropePullStrength;
	[SerializeField]
	Vector3 lastPostition;
	[SerializeField]
	float ropeSpeedLeft;
	[SerializeField]
	float ropeSpeedRight;

	float lastRopeLengthLeft;
	float lastRopeLengthRight;
	public Vector3 cursorAcceleration;
	public Vector3 leftRopeAcceleration;
	public Vector3 rightRopeAcceleration;
	public Vector3 gasAcceleration;

	private AudioSource movementAudio;
	public float speedToMakeSound = 3f;
	public float speedMag = 0;
	public Vector3 accellerationLastFrame = new Vector3();
	// Start is called before the first frame update
	void Start()
    {
		Vector3 ropeSourceToCursorVecLeft = currentTargetPointLeftAnchor - ropeSourceLeft.position;
		lastRopeLengthLeft = ropeSourceToCursorVecLeft.magnitude;
		characterController = GetComponent<CharacterController>();
		AddAcceleration(cursorAcceleration);
		AddAcceleration(leftRopeAcceleration);
		AddAcceleration(rightRopeAcceleration);
		AddAcceleration(gasAcceleration);

		movementAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {


		//Inputs

		float valueleft = triggerPressLeft.action.ReadValue<float>();
        if (valueleft > 0.5 )
		{
			if(!triggerLeftPressed && cursorLeft.hitting)
				StartLeftHookThrow();
			triggerLeftPressed = true;
        }
        else triggerLeftPressed = false;      
        float valueRight = triggerPressRight.action.ReadValue<float>();
        if (valueRight > 0.5)
		{
			if (!triggerRightPressed && cursorLeft.hitting)
				StartRightHookThrow();
			triggerRightPressed = true;
        }
        else triggerRightPressed = false;      
        
        if (leftHookState == HookState.connected && triggerAxisLeft.action.ReadValue<float>() < 0.9f)
        {
			DisconnectLeftHook();
			UpdateAccelerationVec(1, new Vector3(0, 0, 0));
		}
        if (rightHookState == HookState.connected && triggerAxisRight.action.ReadValue<float>() < 0.9f)
        {
			DisconnectRightHook();
			UpdateAccelerationVec(2, new Vector3(0, 0, 0));
		}
        

        if (leftHookState == HookState.isFlying)
		{
			MoveLeftHook();
		}
		if (rightHookState == HookState.isFlying)
		{
			MoveRightHook();
		}

		      if (ForwardPressRight.action.ReadValue<Vector2>().y > 0 && !gasInUse)
		{
			StartCoroutine("Thrust");
		}

		//if (ForwardPressRight.action.ReadValue<Vector2>().y > 0 || ForwardPressRight.action.ReadValue<Vector2>().y < 0)
		//{
		//	AccelerateTowardsCursor();
		//	AccelerateTowardsCursor();
		//}
		//else {
		//	UpdateAccelerationVec(0, new Vector3());
		//}
		if (leftHookState == HookState.connected)
		{
			AccelerateTowardsLeftHook();
		}
		if (rightHookState == HookState.connected)
		{
			AccelerateTowardsRightHook();
		}


		//Movement
		Vector3 movementVec = new Vector3(speedX, speedY, speedZ);
        CollisionFlags flag =  characterController.Move(movementVec * Time.deltaTime);

		//Sound
		Vector3 acceleration = new Vector3(accelerationX, accelerationY, accelerationZ);
		if (movementVec.sqrMagnitude > speedToMakeSound)
        {
			movementAudio.volume = MathMap.Map(speedMag, speedToMakeSound, speedToMakeSound + 1000f, 0.5f,1f, true);
			if (acceleration.magnitude >= accellerationLastFrame.magnitude && movementAudio.pitch <= 1.5f)
			{
				movementAudio.pitch += 0.05f;
			}
			else if (acceleration.magnitude <= accellerationLastFrame.magnitude && movementAudio.pitch >= 0.5f)
            {
				movementAudio.pitch -= 0.05f;
            }
        } else if(movementVec.sqrMagnitude < speedToMakeSound)
        {
			movementAudio.volume = 0;
			movementAudio.pitch = 1f;
        }
		
    }
	private void LateUpdate()
	{
		
		if (leftHookState == HookState.pulledIn) anchorLeft.transform.position = ropeSourceLeft.position;

		if (rightHookState == HookState.pulledIn) anchorRight.transform.position = ropeSourceRight.position;
		//delete?
		lastPostition = transform.position;
	}

	IEnumerator Thrust()
	{
		AccelerateForwardAndUp();
		gasInUse = true;
		yield return new WaitForSeconds(gasDuration);
		UpdateAccelerationVec(3, new Vector3(0, 0, 0));
		yield return new WaitForSeconds(gasCooldown);
		gasInUse = false;
	}
	private void AccelerateTowardsLeftHook()
	{
		Vector3 ropeSourceToCursorVec = currentTargetPointLeftAnchor - ropeSourceLeft.position;
		
		UpdateAccelerationVec(1, ropeSourceToCursorVec.normalized * ropePullStrength * (float)Math.Pow(500, ropeSpeedLeft));
	}
	private void AccelerateTowardsRightHook()
	{
		Vector3 ropeSourceToCursorVec = currentTargetPointRightAnchor - ropeSourceRight.position;
		
		UpdateAccelerationVec(2, ropeSourceToCursorVec.normalized * ropePullStrength * (float)Math.Pow(500,ropeSpeedRight));
		Debug.Log("ropespeed:    " + ropeSpeedRight);
		Debug.Log("multiplier:   " + (float)Math.Pow(500, ropeSpeedRight));
	}


	private void AccelerateTowardsCursor()
	{
		
		cursorAcceleration.x = cursorRight.transform.forward.x * 0.25f;
		cursorAcceleration.y = cursorRight.transform.forward.y * 0.25f;
		cursorAcceleration.z = cursorRight.transform.forward.z * 0.25f;
		UpdateAccelerationVec(0,cursorAcceleration);

	}
	private void AccelerateForwardAndUp()
	{
		Debug.Log("UP");
		gasAcceleration.x = cursorRight.transform.forward.x * gasStrength;
		gasAcceleration.y = cursorRight.transform.forward.y * gasStrength;
		gasAcceleration.z = cursorRight.transform.forward.z * gasStrength;
		UpdateAccelerationVec(3,gasAcceleration);

	}

	private void StartLeftHookThrow()
	{
		//anchorleft.transform.position = new Vector3(cursorleft.hit.point.x, cursorleft.hit.point.y, cursorleft.hit.point.z);
		currentTargetPointLeftAnchor = new Vector3(cursorLeft.hit.point.x, cursorLeft.hit.point.y, cursorLeft.hit.point.z);
		anchorLeft.transform.position = ropeSourceLeft.position;
		distanceTravelledLeft = 0;

		leftHookState = HookState.isFlying;
	}

	private void DisconnectLeftHook()
	{
		anchorLeft.transform.position = ropeSourceLeft.position;
		distanceTravelledLeft = 0;
		leftHookState = HookState.pulledIn;
	}
	private void DisconnectRightHook()
	{
		anchorRight.transform.position = ropeSourceRight.position;
		distanceTravelledRight = 0;
		rightHookState = HookState.pulledIn;
	}

	private void StartRightHookThrow()
	{
		//anchorRight.transform.position = new Vector3(cursorRight.hit.point.x, cursorRight.hit.point.y, cursorRight.hit.point.z);
		currentTargetPointRightAnchor = new Vector3(cursorRight.hit.point.x, cursorRight.hit.point.y, cursorRight.hit.point.z);
		anchorRight.transform.position = ropeSourceRight.position;
		distanceTravelledRight = 0;

		rightHookState = HookState.isFlying;
	}


	private void MoveLeftHook()
	{
		Vector3 ropeSourceToCursorVec = currentTargetPointLeftAnchor - ropeSourceLeft.position;
		Vector3 anchorTravelDirection = (ropeSourceToCursorVec).normalized;
		float momentumPercentage = Vector3.Dot(GetMovementVec().normalized, anchorTravelDirection);
		//anchorLeft.transform.position = (anchorTravelDirection * anchorSpeed) + (GetMovementVec() * momentumPercentage) * Time.deltaTime;
		Vector3 e = anchorTravelDirection * anchorSpeed;
		Vector3 anchoreMovementVecc = e * 0.01f;
		if (ropeSourceToCursorVec.magnitude >= (anchorLeft.transform.position - ropeSourceLeft.position).magnitude)
		{
			anchorLeft.transform.position = ropeSourceLeft.position + anchoreMovementVecc + anchorTravelDirection * distanceTravelledLeft;
			distanceTravelledLeft += anchoreMovementVecc.magnitude;
		}
		else
		{
			anchorLeft.transform.position = currentTargetPointLeftAnchor;
			leftHookState = HookState.connected;
			distanceTravelledLeft = 0;
		}
	}

	private void MoveRightHook()
	{
		Vector3 ropeSourceToCursorVec = currentTargetPointRightAnchor - ropeSourceRight.position;
		Vector3 anchorTravelDirection = (ropeSourceToCursorVec).normalized;
		float momentumPercentage = Vector3.Dot(GetMovementVec().normalized, anchorTravelDirection);
		//anchorLeft.transform.position = (anchorTravelDirection * anchorSpeed) + (GetMovementVec() * momentumPercentage) * Time.deltaTime;
		Vector3 e = anchorTravelDirection * anchorSpeed;
		Vector3 anchoreMovementVecc = e * 0.01f;
		if (ropeSourceToCursorVec.magnitude >= (anchorRight.transform.position - ropeSourceRight.position).magnitude)
		{
			anchorRight.transform.position = ropeSourceRight.position + anchoreMovementVecc + anchorTravelDirection * distanceTravelledRight;
			distanceTravelledRight += anchoreMovementVecc.magnitude;
		}
		else
		{

			anchorRight.transform.position = currentTargetPointRightAnchor;
			rightHookState = HookState.connected;
			distanceTravelledRight = 0;
		}
	}

	private void CalculateNewSpeed()
    {
		speedX = characterController.velocity.x;
		speedY = characterController.velocity.y;
		speedZ = characterController.velocity.z;

		speedX += accelerationX;
        speedY += accelerationY - GRAVITY;
        speedZ += accelerationZ;
    }
	private void FixedUpdate()
	{
		// rope speed
		Vector3 ropeSourceToCursorVecLeft = currentTargetPointLeftAnchor - ropeSourceLeft.position;
		ropeSpeedLeft = ropeSourceToCursorVecLeft.magnitude - lastRopeLengthLeft;
		lastRopeLengthLeft = ropeSourceToCursorVecLeft.magnitude;
		Vector3 ropeSourceToCursorVecRight = currentTargetPointRightAnchor - ropeSourceRight.position;
		ropeSpeedRight = ropeSourceToCursorVecRight.magnitude - lastRopeLengthRight;
		lastRopeLengthRight = ropeSourceToCursorVecRight.magnitude;

		CalculateTotalAccelerationVec();
        CalculateNewSpeed();

        //friction
        if (characterController.isGrounded)
        {
            float friction = 0.9f;
            speedX *= friction;
            speedY *= friction;
            speedZ *= friction;
        }

    }
	private void CalculateTotalAccelerationVec()
	{
		Vector3 currentTotalAcceleration = new Vector3();
		foreach (Vector3 accelerationVec in accelerationVecs)
		{
			currentTotalAcceleration += accelerationVec;
		}
		accelerationX = currentTotalAcceleration.x;
		accelerationY = currentTotalAcceleration.y;
		accelerationZ = currentTotalAcceleration.z;
	}
    public Vector3 GetMovementVec()
    {
        return new Vector3(speedX, speedY, speedZ);
    }
	/// <summary>
	/// Adds an acceleration force. Will 
	/// </summary>
	/// <param name="accelerationVec"></param>
	public void AddAcceleration(Vector3 accelerationVec)
	{
		accelerationVecs.Add(accelerationVec);
	}
	public void UpdateAccelerationVec(int index, Vector3 vec)
	{
		
		accelerationVecs[index] = vec;
	}
	private void SetTotalAcceleration(Vector3 acceleration)
	{
		accelerationX = 0;
		accelerationY = 0;
		accelerationZ = 0;
	}

}
public enum HookState
{
    pulledIn,
    isFlying,
    connected
}