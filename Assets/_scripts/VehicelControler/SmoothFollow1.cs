using UnityEngine;
using System.Collections;

public class SmoothFollow1 : MonoBehaviour {

	// The target we are following
	public static SmoothFollow1 instance;
	public Transform target;
	// The distance in the x-z plane to the target
	public float distance = 2.0f;
	// the height we want the camera to be above the target
	public float height = 5.0f;
	// How much we 
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
	//public bool objectStop=false;
	
	// Place the script in the Camera-Control group in the component menu
	[AddComponentMenu("Camera-Control/Smooth Follow")]
	void Start(){
	instance = this;
	//Invoke("GetTarget",0.3f);
	//Invoke("decreaseDist",7.5f);
	}
	// void decreaseDist(){
	// 	objectStop=true;
	// }
       void GetTarget ()
        {
           // target = GameObject.FindGameObjectWithTag("Player").transform;
        }
	
	void Update () {
		// Early out if we don't have a target
		if (!target) return;
		
		// Calculate the current rotation angles
		float wantedRotationAngle = target.eulerAngles.y;
		float wantedHeight = target.position.y + height;
		
		float currentRotationAngle = transform.eulerAngles.y;
		float currentHeight = transform.position.y;
		
		// Damp the rotation around the y-axis
		// if(objectStop && distance>0.2){
		// 	distance-=0.1f;
		// }
		//if(objectStop && height>2){
		//	height-=0.1f;
		//}
//		if(objectStop && heightDamping>0.2){
//			heightDamping-=0.1f;
//		}
		currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
		
		// Damp the height
		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
		
		// Convert the angle into a rotation
		var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
		
		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		transform.position = target.position;
		transform.position -= currentRotation * Vector3.forward * distance;
		
		// Set the height of the camera
		transform.position = new Vector3(transform.position.x,currentHeight,transform.position.z);
		
		// Always look at the target
		transform.LookAt(target);
	}
}
