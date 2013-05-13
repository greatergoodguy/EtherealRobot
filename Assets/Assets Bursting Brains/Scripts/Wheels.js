var Drive : boolean =true;
var Steering : boolean =true;
var back : boolean =false;
var throttle=0.0;
private var turn=0.0;
var torqMax;
var torq;
var rotationSpeed : float;
private var brake=0.0;
var MaxTurn;
var hit : WheelHit;
var oldposition : Vector3;
var initAccX:float;
var initAccY:float;

function Start(){
	initAccX=Input.acceleration.x;
	initAccY=Input.acceleration.y;
	torqMax=100; 
	torq=torqMax;
	
	var wheel : WheelCollider = GetComponent(WheelCollider);
	wheel.forwardFriction.stiffness = 0.14;
	wheel.forwardFriction.asymptoteValue=5000;
	wheel.sidewaysFriction.asymptoteValue=5000;
	wheel.sidewaysFriction.stiffness = 0.02;
	wheel.suspensionSpring.spring=15000;
	wheel.suspensionSpring.damper=100;
	wheel.suspensionDistance = 0.01;
	
	if (Drive){
		wheel.forwardFriction.stiffness = 0.04;
		wheel.sidewaysFriction.stiffness = 0.06;
		wheel.forwardFriction.asymptoteValue=5000;
		wheel.sidewaysFriction.asymptoteValue=5000;
		wheel.suspensionSpring.spring=15000;
		wheel.suspensionSpring.damper=100;
		wheel.suspensionDistance = 0.01;
	}
}

function Update(){
	var wheel : WheelCollider = GetComponent(WheelCollider);
	wheel.sidewaysFriction.stiffness = 0.06 + transform.root.rigidbody.velocity.magnitude * 0.01; //this provides stabilization with speed
	MaxTurn = 45;
	
	if(MaxTurn < 5)
		MaxTurn = 5;
		
	if(Input.GetKey(KeyCode.UpArrow) && Drive && (throttle < torq)) 
		throttle += torq * Time.deltaTime;
	else if(Drive)
		throttle = throttle - (throttle * Time.deltaTime);
		
	if(Input.GetKey(KeyCode.UpArrow) && Drive && (torq > 0))
		torq -= Time.deltaTime * 25;
	else if(torq < torqMax)
		torq += Time.deltaTime * 40;
	
	if(Input.GetKey (KeyCode.DownArrow)) 
		brake += 1000 * Time.deltaTime;
	else 
		brake = brake - (brake * Time.deltaTime * 10);
	if(brake < 1)
		brake = 0;

	if(Input.GetKey (KeyCode.RightArrow) && Steering && (turn < MaxTurn))
		turn += 30 * Time.deltaTime;
	else if(Input.GetKey(KeyCode.LeftArrow) && Steering && (turn > -MaxTurn)) 
		turn -= 30 * Time.deltaTime;
	else if(Steering) 
		turn = turn - (turn * Time.deltaTime);
		
	collider.motorTorque = throttle;
	
	if(back) 
		collider.steerAngle = -turn; 
	else 
		collider.steerAngle = turn;
	
	collider.brakeTorque = brake;
}