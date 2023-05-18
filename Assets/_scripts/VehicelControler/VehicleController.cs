using UnityEngine;
using System;

[Serializable]
public enum DriveType
{
	RearWheelDrive,
	FrontWheelDrive,
	AllWheelDrive
}

public class VehicleController : MonoBehaviour
{
	public static VehicleController instance;
    [Tooltip("Maximum steering angle of the wheels")]
	public float maxAngle = 30f;
	[Tooltip("Maximum torque applied to the driving wheels")]
	public float maxTorque = 400f;
	[Tooltip("Maximum brake torque applied to the driving wheels")]
	public float brakeTorque = 75000f;
	//[Tooltip("If you need the visual wheels to be attached automatically, drag the wheel shape here.")]
	//public GameObject wheelShape;

	[Tooltip("The vehicle's speed when the physics engine can use different amount of sub-steps (in m/s).")]
	public float criticalSpeed = 5f;
	[Tooltip("Simulation sub-steps when the speed is above critical.")]
	public int stepsBelow = 5;
	[Tooltip("Simulation sub-steps when the speed is below critical.")]
	public int stepsAbove = 1;

	[Tooltip("The vehicle's drive type: rear-wheels drive, front-wheels drive or all-wheels drive.")]
	public DriveType driveType;

    public WheelCollider[] m_Wheels;

	public GameObject BrakesLights;
	public GameObject ReverseLights;


	public GameObject cameraRev;

	public float steerVal, torqueVal, handBrakeValue;
	private bool isAppliedBrakes = false;
	public static bool brakeRev;

	public float TempBrakeForce;


	public Rigidbody rb;
	[HideInInspector]
	//public float angle,torque,handBrake;

	// Find all the WheelColliders down in the hierarchy.
	void Start()
	{
		instance = this;
		brakeRev = false;
		steerVal= torqueVal=handBrakeValue=0;

		rb = GetComponent<Rigidbody>();


		cameraRev=GameObject.FindWithTag("cam");
		// m_Wheels = GetComponentsInChildren<WheelCollider>();
		// for (int i = 0; i < m_Wheels.Length; ++i) 
		// {
		// 	var wheel = m_Wheels [i];
		// 	// Create wheel shapes only when needed.
		// 	if (wheelShape != null)
		// 	{
		// 		var ws = Instantiate (wheelShape);
		// 		ws.transform.parent = wheel.transform;
		// 	}
		// }
	}

	// This is a really simple approach to updating wheels.
	// We simulate a rear wheel drive car and assume that the car is perfectly symmetric at local zero.
	// This helps us to figure our which wheels are front ones and which are rear.
	void Update()
	{
		m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);
		#if UNITY_EDITOR
		steerVal = Input.GetAxis("Horizontal");
		torqueVal = Input.GetAxis("Vertical");
		handBrakeValue = Input.GetKey(KeyCode.X) ? brakeTorque : 0;
		 #endif

		foreach (WheelCollider wheel in m_Wheels)
		{
			// A simple car where front wheels steer while rear ones drive.
			if (wheel.transform.localPosition.z > 0)
			{
				wheel.steerAngle = steerVal*maxAngle;
            //	Debug.Log("steer "+steerVal);
			}

			if (wheel.transform.localPosition.z < 0)
			{
				wheel.brakeTorque = handBrakeValue*brakeTorque*2;
				if(handBrakeValue <= 0)
				isAppliedBrakes = false;
				else
				isAppliedBrakes = true;
			}

			if (wheel.transform.localPosition.z < 0 && driveType != DriveType.FrontWheelDrive)
			{
				if(!isAppliedBrakes)
				wheel.motorTorque = torqueVal*maxTorque;
				else
				{
					wheel.motorTorque = 0;
					wheel.brakeTorque = handBrakeValue*brakeTorque*2;
					//Debug.Log("brake");	
				}
			}

			if (wheel.transform.localPosition.z >= 0 && driveType != DriveType.RearWheelDrive)
			{
				if(!isAppliedBrakes)
				wheel.motorTorque = torqueVal*maxTorque;
				else
				wheel.motorTorque = 0;				
			}
			// Update visual wheels if any.
			// if (wheelShape) 
			// {
				Quaternion q;
				Vector3 p;
				wheel.GetWorldPose (out p, out q);

				// Assume that the only child of the wheelcollider is the wheel shape.
				Transform shapeTransform = wheel.transform.GetChild (0);
				//shapeTransform.position = p;
				//shapeTransform.rotation = q;
				shapeTransform.SetPositionAndRotation(p, q);
		// 	}
		}
	}

    public void ToggleBrakes(bool flag)
    {
			
        BrakesLights.SetActive(flag);
//		if(ButtonInput.chk)
//		{
//			rb.drag = 10f;
//		}
		InvokeRepeating ("brakestop",0.1f,0.1f);
//		if(!ButtonInput.chk)
//		{
//			rb.drag = 0.16f;
//
//		}
	}

    public void ToggleReverse(bool flag)
    {

        ReverseLights.SetActive(flag);

    }
//	public void BrakePress()
//	{
//		VehicleController.instance.rb.drag = 5f;
//		Debug.Log ("brakedonnnn");
//	}

	public void brakestop()
	{

		rb.drag += TempBrakeForce;
	
	

	}

	public void CancelBrakeInvoke()
	{
		CancelInvoke ("brakestop");
	}
}
