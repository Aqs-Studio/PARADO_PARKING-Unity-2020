using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class SmoothCamera2D : MonoBehaviour {
	
	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;
	public float x,y,z = 0.5f;
	//private Camera thisCam;
	void Start ()
	{
		gameObject.transform.parent = null;
		//thisCam = GetComponent<Camera>();

	}
	// Update is called once per frame
	void Update () 
	{
		if (target)
		{
//			Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
			Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(x, y, z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
			Quaternion rot = Quaternion.Euler(90, target.eulerAngles.y, 0);

        // Dampen towards the target rotation
        	transform.rotation = Quaternion.Slerp(transform.rotation, rot,  Time.deltaTime * dampTime*2);

			//for testing
			//transform.SetPositionAndRotation(Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime) , Quaternion.Slerp(transform.rotation, rot,  Time.deltaTime * dampTime*5) );

		}
		
	}


}