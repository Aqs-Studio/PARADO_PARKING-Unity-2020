using UnityEngine;
using System.Collections;
using System.ComponentModel.Design;


public class TouchCam : MonoBehaviour
{

	public Transform target;
    public float distance = 10.0f;
    public float height = 10.0f;


    public float xSpeed = 250f;
	public float ySpeed = 120f;
	
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	
	private float x = 0f;
	private float y = 0f;

	public float rotationSpeed;

	private float xx;
	private float yy;
    public float lookSpeed = 10;
    public float followSpeed = 10;
    public Vector3 offset;

    public static bool isDrag;
	public static bool isMousePressed;
	private float counter;
	private Vector3 mouseUpPosition;
	private Vector3 mouseDownPosition;
	private float xMouseMoved;
	private float yMouseMoved;



	private Vector3 position = new Vector3 ();
	Quaternion rotation;


	void DefaultCameraPose ()
	{
        target = GameObject.Find("Car_11_pysics(Clone)").transform;
        rotation = Quaternion.Euler (y, x, 0);
		position = rotation * new Vector3 (0.0f, height, -distance) + target.position;
	}

	void  Start ()
	{

    //    target = GameObject.FindGameObjectWithTag("Player").transform;
        isDrag = false;
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;

		DefaultCameraPose ();



       


    }
    void player_ins()
    {
      
    }

    public void LookAtTarget()
    {
        Vector3 _lookDirection = target.position - transform.position;
        Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookSpeed * Time.deltaTime);
    }

    void Update ()
	{
        

        if (Application.isMobilePlatform) {
			if (Input.touchCount > 1) {
				isDrag = false;
			}
		}
	}

    public void MoveToTarget()
    {
        Vector3 _targetPos = target.position + target.forward * offset.z + target.right * offset.x + target.up * offset.y;
        transform.position = Vector3.Lerp(transform.position, _targetPos, followSpeed * Time.deltaTime);
    }
    void  LateUpdate ()
	{

        LookAtTarget();
        MoveToTarget();

        if (Application.isMobilePlatform) {

          

			if (target && isDrag) {


           

                x += Input.touches [0].deltaPosition.x * xSpeed * 0.008f*Time.deltaTime;
				y -= Input.touches [0].deltaPosition.y * ySpeed * 0.008f;
				y = ClampAngle (y, yMinLimit, yMaxLimit);

				Quaternion rotation = Quaternion.Euler (y, x, 0);
				Vector3 position = rotation * new Vector3 (0f, height, -distance) + target.position;

				transform.rotation = rotation;
				transform.position = position;

			} else if (target) {
				x += Time.deltaTime * xSpeed * 0.02f * rotationSpeed;
				y = ClampAngle (y, yMinLimit, yMaxLimit);
				Quaternion rotation = Quaternion.Euler (y, x, 0);
				Vector3 position = rotation * new Vector3 (0.0f, height, -distance) + target.position;
				transform.rotation = rotation;
				transform.position = position;		
			}
		} else {


			if (target && isDrag) {

				x += Input.GetAxis ("Mouse X") * xSpeed * 0.02f;
				y -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;
			
				y = ClampAngle (y, yMinLimit, yMaxLimit);
			
				Quaternion rotation = Quaternion.Euler (y, x, 0);
				Vector3 position = rotation * new Vector3 (0f, height, -distance) + target.position;
			
				transform.rotation = rotation;
				transform.position = position;
			} else if (target) {
				x += Time.deltaTime * xSpeed * 0.02f * rotationSpeed;
				y = ClampAngle (y, yMinLimit, yMaxLimit);
				Quaternion	rotation = Quaternion.Euler (y, x, 0);
				Vector3 position = rotation * new Vector3 (0.0f, height, -distance) + target.position;
				transform.rotation = rotation;
				transform.position = position;		
			}

		}
	}

	static float ClampAngle (float angle, float min, float max)
	{

		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);

	}

	// Update is called once per frame
	void OnGUI ()
	{
		if (Application.isMobilePlatform) {
			if (Input.touchCount > 1) {
				isDrag = false;
			}


			if (Input.touchCount == 2) {
//				Touch touch = Input.GetTouch (0);
//				Touch touch1 = Input.GetTouch (1);
//				if (touch.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved) {
//					Vector2 curDist = touch.position - touch1.position;
//					Vector2 prevDist = (touch.position - touch.deltaPosition) - (touch1.position - touch1.deltaPosition);
//					float delta = curDist.magnitude - prevDist.magnitude;
//					distance = distance + delta;
//				}





			}
		}
		//Debug.Log (isDrag);
		if (Event.current.type == EventType.MouseDown) {
			if (Application.isMobilePlatform) {			
				mouseDownPosition = Input.touches [0].position;
			} else {
				mouseDownPosition = Input.mousePosition;
			}
			isMousePressed = true;
		} else if (Event.current.type == EventType.MouseDrag) {
			CancelInvoke ("StartCounting");
			counter = 0f;
			isDrag = true;
		} else if (Event.current.type == EventType.MouseUp) {
			isMousePressed = false;
			if (Application.isMobilePlatform) {				
				mouseUpPosition = Input.touches [0].position;
			} else {
				mouseUpPosition = Input.mousePosition;
			}
			xMouseMoved = Mathf.Abs (mouseUpPosition.x - mouseDownPosition.x);
			yMouseMoved = Mathf.Abs (mouseUpPosition.y - mouseDownPosition.y);
			//if the makes a small drag, consider it click
			if ((xMouseMoved < 5f) || (yMouseMoved < 5f)) {
				isDrag = false;
			} else {
				InvokeRepeating ("StartCounting", 0.02f, 0.02f);
			}
		}
	}

	//we need this method so that Raycast won't be activated if we release the mouse button or the swipe, only on touches, not on drag
	void StartCounting ()
	{
		counter += Time.deltaTime;
		if (counter > 0.01f) {
			isDrag = false;
		}
	}
	
}