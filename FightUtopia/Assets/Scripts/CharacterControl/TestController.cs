/*
*Author Sean Hoey X11000759
*Date: 11/10/2014
*/
using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour {
	public float walkSpeed = 10.0f;
	// after trotAfterSeconds of walking we trot with trotSpeed
	public float trotSpeed = 15.0f;
	// when pressing "Fire3" button (cmd) we start running
	public float runSpeed = 20.0f;
	public GameObject CameraMain;
	// The gravity for the character
	float gravity = 20.0f;
	// The gravity in controlled descent mode
	float speedSmoothing = 10.0f;
	float rotateSpeed = 500.0f;
	float trotAfterSeconds = 3.0f;
	// The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
	private float lockCameraTimer = 0.0f;
	// The current move direction in x-z
	private Vector3 moveDirection = Vector3.zero;
	// The current vertical speed
	private float verticalSpeed = 0.0f;
	// The current x-z move speed
	private float moveSpeed = 0.0f;
	// Are we moving backwards (This locks the camera to not do a 180 degree spin)
	private bool movingBack = false;
	// Is the user pressing any keys?
	private bool isMoving = false;
	// When did the user start walking (Used for going into trot after a while)
	private float walkTimeStart = 0.0f;
	private bool isControllable = true;
	private CollisionFlags collisionFlags ;
    Animator CharAnim;
    bool Running=false;

	
	// Use this for initialization
	void Awake () {
		moveDirection = transform.TransformDirection(Vector3.forward);
       
	}
    void Start()
    {
        CharAnim=gameObject.GetComponent<Animator>();
        
    }
	void UpdateSmoothedMovementDirection(){
		Transform cameraTransform = CameraMain.transform;
		
		// Forward vector relative to the camera along the x-z plane	
		Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
		
		// Right vector relative to the camera
		// Always orthogonal to the forward vector
		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		
		var v = Input.GetAxisRaw("LeftAnologUpDown");
		var h = Input.GetAxisRaw("LeftAnologLeftRight");
        
		// Are we moving backwards or looking backwards
		if (v < -0.2)
			movingBack = true;
		else
			movingBack = false;
		
		var wasMoving = isMoving;
		isMoving = Mathf.Abs (h) > 0.1 || Mathf.Abs (v) > 0.1;
		
		// Target direction relative to the camera
		Vector3 targetDirection =h * right + v * forward;
		
		// Grounded controls
			// Lock camera for short period when transitioning moving & standing still
			lockCameraTimer += Time.deltaTime;
			if (isMoving != wasMoving)
				lockCameraTimer = 0.0f;
			
			// We store speed and direction seperately,
			// so that when the character stands still we still have a valid forward direction
			// moveDirection is always normalized, and we only update it if there is user input.
			if (targetDirection != Vector3.zero)
			{
				// If we are really slow, just snap to the target direction
				if (moveSpeed < walkSpeed * 0.9 )
				{
					moveDirection = targetDirection.normalized;
				}
				// Otherwise smoothly turn towards it
				else
				{
					moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
					
					moveDirection = moveDirection.normalized;
				}
			}
			
			// Smooth the speed based on the current target direction
			var curSmooth = speedSmoothing * Time.deltaTime;
			
			// Choose target speed
			//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
			var targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);
			
			
			
			// Pick speed modifier
			if (Input.GetButton("L2"))
			{
				targetSpeed *= runSpeed;
                Running = true;
				
			}
			else if (Time.time - trotAfterSeconds > walkTimeStart)
			{
				targetSpeed *= trotSpeed;
                Running = false;	
			}
			else
			{
				targetSpeed *= walkSpeed;
                Running = false;
			}
			
			moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
			
			// Reset walk time start when we slow down
            if (moveSpeed < walkSpeed * 0.3)
            {
                walkTimeStart = Time.time;
            }
            if (v == 0 && h == 0)
            {
                moveSpeed = 0;
            }
		
	}
	
	
	
	
	
	float CalculateJumpVerticalSpeed (float targetJumpHeight){
		return Mathf.Sqrt(2 * targetJumpHeight * gravity);
	}


    void EnemyIsNear()
    {
        if (!Running)
        {
            CharAnim.SetBool("EnemyNear", true);
        }
    }
    void EnemyNotNear()
    {
        CharAnim.SetBool("EnemyNear", false);
    }

	// Update is called once per frame
    void Update(){
            if (!isControllable)
            {
                // kill all inputs if not controllable.
                Input.ResetInputAxes();
            }
            /*if (Input.GetButtonDown("Triangle"))
            {
                lastJumpButtonTime = Time.time;
            }
            */
            UpdateSmoothedMovementDirection();

            // Apply gravity
            // - extra power jump modifies gravity
            // - controlledDescent mode modifies gravity


            CharAnim.SetFloat("Speed", moveSpeed);
            // Calculate actual motion
            Vector3 movement = moveDirection * moveSpeed + new Vector3(0, verticalSpeed, 0);
            movement *= Time.deltaTime;

            CharacterController controller = gameObject.GetComponent<CharacterController>();
            collisionFlags = controller.Move(movement);

            // Set rotation to the move direction
       

                transform.rotation = Quaternion.LookRotation(moveDirection);


            // We are in jump mode but just became grounded
           
            /*if(Input.GetAxis("LeftAnologUpDown")<-0.5f){
                print("LeftAnologUpDown down");
            }
            if(Input.GetAxis("LeftAnologLeftRight")>0.5f){
                print ("LeftAnologLeftRight Right");
            }
            if(Input.GetAxis("LeftAnologLeftRight")<-0.5f){
                print ("LeftAnologLeftRight left");
            }
            if(Input.GetAxis("RightAnologLeftRight")>0.5f){
                print ("RightAnologLeftRight right");
            }
            if(Input.GetAxis("RightAnologLeftRight")<-0.5f){
                print ("RightAnologLeftRight left");
            }
            if(Input.GetAxis("RightAnologUpDown")>0.5f){
                print ("RightAnologUpDown up");
            }
            if(Input.GetAxis("RightAnologUpDown")<-0.5f){
                print ("RightAnologUpDown down");
            }
            if(Input.GetAxis("DPadUpDown")>0.5f){
                print ("DPadUpDown up");
            }
            if(Input.GetAxis("DPadUpDown")<-0.5f){
                print ("DPadUpDown down");
            }
            if(Input.GetAxis("DPadLeftRight")>0.5f){
                print ("DPadLeftRigh right");
            }
            if(Input.GetAxis("DPadLeftRight")<-0.5f){
                print ("DPadLeftRigh left");
            }
            if(Input.GetButton("Triangle")){
                print("Triangle");
            }
            if(Input.GetButton("Square")){
                print("square");
            }
            if(Input.GetButton("Circle")){
                print("circle");
            }
            if(Input.GetButton("X")){
                print("X");
            }
            if(Input.GetButton("L1")){
                print("L1");
            }
            if(Input.GetButton("L2")){
                print("L2");
            }
            if(Input.GetButton("R1")){
                print("R1");
            }
            if(Input.GetButton("R2")){
                print("R2");
            }
            if(Input.GetButton("Select")){
                print("Select");
            }
            if(Input.GetButton("Start")){
                print("Start");
            }
            if(Input.GetButton("LeftClick")){
                print("LeftClick");
            }
            if(Input.GetButton("RightClick")){
                print("RightClick");
            }
            if(Input.GetButton("Home")){
                print("Home");
            }*/
             
	}
	
	float GetSpeed () {
		return moveSpeed;
	}
	
	
	
	
	Vector3 GetDirection () {
		return moveDirection;
	}
	
	bool IsMovingBackwards () {
		return movingBack;
	}
	
	float GetLockCameraTimer () 
	{
		return lockCameraTimer;
	}
	
	bool IsMoving () 
	{
		return Mathf.Abs(Input.GetAxisRaw("LeftAnologUpDown")) + Mathf.Abs(Input.GetAxisRaw("LeftAnologLeftRight")) > 0.5;
	}
	
	void Reset ()
	{
		gameObject.tag = "Player";
	}
	
	
}