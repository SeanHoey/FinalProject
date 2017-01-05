/*
*Author Sean Hoey X11000759
*Date: 11/10/2014
*/
using UnityEngine;
using System.Collections;

public class TestCameraController : MonoBehaviour {
	public Collider Target;
	Transform _Target;
	public Transform cameraTransform;
	TestController Controller;
	new public Camera camera;
    float ClickRepeatTime = 1f;
    float ClickTime;
	public float rotationUpdateSpeed = 60.0f,lookUpSpeed = 20.0f;
	public float height = 5.0f;
	public float distance=10.0f;
	float heightSmoothLag = 0.3f;
	float snapMaxSpeed = 720.0f;
	float snapSmoothLag = 0.2f;
	private Vector3 headOffset = Vector3.zero;
	private Vector3 centerOffset = Vector3.zero;
	private float targetHeight = 10.0f;
	private float heightVelocity = 0.0f;
	float SetAngle;
	bool FirstPerson=false;
	
	void Awake(){
		if(!cameraTransform)
			cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
		if(!cameraTransform) {
			Debug.Log("Please assign a camera to the ThirdPersonCamera script.");
			enabled = false;	
		}
		
		
		_Target = transform;
		if (_Target)
		{
			Controller = _Target.GetComponent<TestController>();
		}
		
		if (Controller)
		{
			CharacterController characterController = _Target.GetComponent<CharacterController>();
			centerOffset = characterController.bounds.center - _Target.position;
			headOffset = centerOffset;
			headOffset.y = characterController.bounds.max.y - _Target.position.y;
		}
		else
			Debug.Log("Please assign a target to the camera that has a ThirdPersonController script attached.");
		
		
		Cut(_Target, centerOffset);
	}
	
	void LateUpdate () {
		Apply (transform, Vector3.zero);
	}
	
	
	void Cut (Transform dummyTarget,Vector3 dummyCenter)
	{
		float oldHeightSmooth = heightSmoothLag;
		float oldSnapMaxSpeed = snapMaxSpeed;
		float oldSnapSmooth = snapSmoothLag;
		
		snapMaxSpeed = 10000f;
		snapSmoothLag = 0.001f;
		heightSmoothLag = 0.001f;
		
		cameraTransform.LookAt(dummyTarget);
		Apply (transform, Vector3.zero);
		
		heightSmoothLag = oldHeightSmooth;
		snapMaxSpeed = oldSnapMaxSpeed;
		snapSmoothLag = oldSnapSmooth;
	}
	
	// Use this for initialization
	void Start () {
		Target = gameObject.GetComponent<Collider> ();
		if (camera == null)
		{
			if (Camera.main != null)
			{
				camera = Camera.main;
			}
		} 
		SetAngle=cameraTransform.eulerAngles.y;
        ClickTime = Time.time;
	}
	
	void Update(){
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetButton("RightClick")&&Time.time>ClickTime){
			FirstPerson=!FirstPerson;
            ClickTime = Time.time + ClickRepeatTime;
            if (FirstPerson)
            {
                distance = -1;
            }
            else
            {
                distance = 5;
            }
		}
	//rotates camera around character
		float rotationAmounty;
		float rotationAmountx;
		if(Input.GetAxis("RightAnologLeftRight")!=0||Input.GetAxis("RightAnologUpDown")!=0){
			rotationAmounty = Input.GetAxis ("RightAnologLeftRight") * rotationUpdateSpeed * Time.deltaTime;
			cameraTransform.RotateAround (Target.transform.position, Vector3.up, rotationAmounty);
			rotationAmountx = Input.GetAxis ("RightAnologUpDown") * lookUpSpeed * Time.deltaTime;
			cameraTransform.RotateAround (_Target.transform.position, Vector3.right, rotationAmountx);
			cameraTransform.eulerAngles=new Vector3(cameraTransform.eulerAngles.x,cameraTransform.eulerAngles.y,0);
		}
	}
	
	void Apply(Transform dummyTarget,Vector3 dummyCenter){
	
		// Early out if we don't have a target
		if (!Controller)
			return;
		
		var targetCenter = _Target.position + centerOffset;
		
		//	DebugDrawStuff();
		
		// Calculate the current rotation angles
	
		var currentAngle = cameraTransform.eulerAngles.y;

			targetHeight = targetCenter.y + height;
		
		// Damp the height
		var currentHeight = cameraTransform.position.y;
		currentHeight = Mathf.SmoothDamp (currentHeight, targetHeight,ref heightVelocity, heightSmoothLag);
		
		// Convert the angle into a rotation, by which we then reposition the camera
		var currentRotation = Quaternion.Euler (0, currentAngle, 0);
		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		cameraTransform.position = targetCenter;
		
		cameraTransform.position += currentRotation * Vector3.back * distance;
		// Set the height of the camera
		cameraTransform.position =new Vector3(cameraTransform.position.x,currentHeight,cameraTransform.position.z);
		}
}