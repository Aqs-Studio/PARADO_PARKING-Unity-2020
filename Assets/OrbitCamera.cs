using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OrbitCamera : MonoBehaviour {

    public Transform m_Target;

    // Distance
    private float m_Distance = 10.0f;
    public float m_MinDistance = 5.0f;
    public float m_MaxDistance = 25.0f;

    //TouchRect
    public Rect rectTouch;
    public Texture touchImage;

    // Orbit speed
    public float m_XSpeed = 250.0f;
    public float m_YSpeed = 120.0f;

    // Zoom speed
    public float m_ZoomSpeed = 5.0f;

    // Orbit inversion
    public bool m_XInvert = false;
    public bool m_YInvert = false;

    // Zoom inversion
    public bool m_ZoomInvert = false;

    // y angle limit
    public float m_YMinLimit = -20f;
    public float m_YMaxLimit = 70f;

    // Angles
    private float m_X = 0.0f;
    private float m_Y = 0.0f;

    // Orbit Sensitivity
    private float m_OrbitSpeedDelayTime = 0.0f;
    public float m_OrbitSpeedMultiplier = 2.0f;

    // Two fingers touch zoom
    private Vector2 m_CurrTouch1 = Vector2.zero;
    private Vector2 m_LastTouch1 = Vector2.zero;
    private Vector2 m_CurrTouch2 = Vector2.zero;
    private Vector2 m_LastTouch2 = Vector2.zero;
    private float m_CurrDist = 0.0f;
    private float m_LastDist = 0.0f;
    public enum ZoomMethod
    {
        PinchZoom = 1,
        SlideZoom = 2
    }
    public ZoomMethod m_ZoomMethod = ZoomMethod.PinchZoom;
    private bool pinchOnce;
    private bool scrollOnce;



    // Use this for initialization
    void Start () {
        

        if (PlayerPrefs.GetInt("ShowTutorial", 1) == 1)
        {
            pinchOnce = false;
            scrollOnce = false;
        }
        else
        {
            pinchOnce = true;
            scrollOnce = true;
        }

        rectTouch = new Rect(Screen.width * 0.125f, Screen.height * 0.46f, Screen.width * 0.75f, Screen.height * 0.54f);
        
        if (!m_Target)
        {
            m_Target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        this.gameObject.transform.LookAt(m_Target);
        var angles = transform.eulerAngles;
        m_X = angles.y;
        m_Y = angles.x;

        // Check Min and Max distance.
        if (m_MaxDistance < m_MinDistance)
            m_MaxDistance = m_MinDistance + 1;
        if (m_MinDistance > m_MaxDistance)
            m_MinDistance = m_MaxDistance + 1;

        // Set start position according to distance
        bool ShouldUpdateZoom = false;
        m_Distance = Vector3.Distance(this.transform.position, m_Target.transform.position);

        if (m_Distance < m_MinDistance)
        {
            m_Distance = m_MinDistance;
            ShouldUpdateZoom = true;
        }
        else if (m_Distance > m_MaxDistance)
        {
            m_Distance = m_MaxDistance;
            ShouldUpdateZoom = true;
        }
        if (ShouldUpdateZoom == true)
        {
            UpdatePosition();
        }

        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
    }
	void OnGUI ()
    {
        if (!pinchOnce)
        {
            GUI.DrawTexture(new Rect(rectTouch.x, Screen.height * 0.02f, rectTouch.width, rectTouch.height), touchImage);
        }


    }
    // Update is called once per frame
    void Update () {
        
        if (m_Target)
        {
            bool ShouldUpdateZoom = false;
            bool ShouldUpdateOrbit = false;

            ////////////////////////////////////////////////
            // ZOOM
            ////////////////////////////////////////////////

            // Touch or mouse flag for zoom
            if (Input.touchCount == 2 && rectTouch.Contains(Input.GetTouch(1).position))
            {

                ShouldUpdateZoom = true;

                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    // Touch moved
                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    {
                        if (i == 0)
                        {
                            m_CurrTouch1 = touch.position;
                            m_LastTouch1 = m_CurrTouch1 - touch.deltaPosition;
                        }
                        else
                        {
                            m_CurrTouch2 = touch.position;
                            m_LastTouch2 = m_CurrTouch2 - touch.deltaPosition;
                        }
                    }
                }

                m_CurrDist = Vector2.Distance(m_CurrTouch1, m_CurrTouch2);
                m_LastDist = Vector2.Distance(m_LastTouch1, m_LastTouch2);

                // Calculate the zoom magnitude
                float delta = m_LastDist - m_CurrDist;

                // Invert zoom
                if (m_ZoomInvert == true)
                    delta *= -1;

                m_Distance += delta * m_ZoomSpeed * 0.01f;
                if (m_Distance < m_MinDistance)
                {
                    m_Distance = m_MinDistance;
                }
                else if (m_Distance > m_MaxDistance)
                {
                    m_Distance = m_MaxDistance;
                }

                //For Tutorial
                if (!pinchOnce)
                {
                    pinchOnce = true;
                    //TutorialScript.Instance.ResumeGame("5");
                 //   GameManager.GM.Brake = false;
                }
            }
            // reset touch
            else if (Input.touchCount < 2)
            {
                m_CurrDist = 0.0f;
                m_LastDist = 0.0f;
            }
          
            ////////////////////////////////////////////////
            // ORBIT
            ////////////////////////////////////////////////

            // Touch or mouse flag for orbit
            bool OrbitByTouch = false;

            // Orbit by touch
            if (Input.touchCount == 1 && rectTouch.Contains(Input.GetTouch(0).position))
            {
                OrbitByTouch = true;

                // Get touch
                Touch touch = Input.GetTouch(0);

                // Touch began
                if (touch.phase == TouchPhase.Began)
                {
                    m_OrbitSpeedDelayTime = 0.0f;
                }
                // Touch ended or canceld
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    m_OrbitSpeedDelayTime = 0.0f;
                }
                // Touch moved
                else if (touch.phase == TouchPhase.Moved)
                {
                    ShouldUpdateOrbit = true;

                    float TouchDeltaX = touch.deltaPosition.x;
                    float TouchDeltaY = touch.deltaPosition.y;

                    // Invert X and Y axis
                    if (m_XInvert)
                        TouchDeltaX *= -1;
                    if (m_YInvert)
                        TouchDeltaY *= -1;

                    m_X += touch.deltaPosition.x * m_XSpeed * 0.005f * m_OrbitSpeedDelayTime;
                    m_Y -= touch.deltaPosition.y * m_YSpeed * 0.005f * m_OrbitSpeedDelayTime;

                    //For Tutorials
                    if (!scrollOnce && pinchOnce)
                    {
                        scrollOnce = true;


                        //TutorialScript.Instance.tempforScrollFinger = true;
                       // TutorialScript.Instance.ResumeGame("6");
                        

                    }
                }

                
            }

            // Orbit by mouse
            if (OrbitByTouch == false && rectTouch.Contains(Input.mousePosition))
            {
                // Reset orbit speed when left mouse down
                if (Input.GetMouseButtonDown(0))
                {
                    m_OrbitSpeedDelayTime = 0.0f;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    m_OrbitSpeedDelayTime = 0.0f;
                }
                if (Input.GetMouseButton(0))
                {
                    ShouldUpdateOrbit = true;

                    float MouseAxisX = Input.GetAxis("Mouse X");
                    float MouseAxisY = Input.GetAxis("Mouse Y");

                    // Invert X and Y axis
                    if (m_XInvert)
                        MouseAxisX *= -1;
                    if (m_YInvert)
                        MouseAxisY *= -1;

                    m_X += MouseAxisX * m_XSpeed * 0.02f * m_OrbitSpeedDelayTime;
                    m_Y -= MouseAxisY * m_YSpeed * 0.02f * m_OrbitSpeedDelayTime;

                }
                //For Tutorials
                //if (!scrollOnce && pinchOnce)
                //{
                //    scrollOnce = true;
                //    TutorialScript.Instance.ResumeGame("6");
                //}
            }

            ////////////////////////////////////////////////
            // UPDATE POSITON
            ////////////////////////////////////////////////

            if (ShouldUpdateZoom || ShouldUpdateOrbit)
            {

            }
            UpdatePosition();
        }
    }

    void UpdatePosition()
    {
        m_OrbitSpeedDelayTime += Time.deltaTime * m_OrbitSpeedMultiplier;
        if (m_OrbitSpeedDelayTime > 1.0f)
            m_OrbitSpeedDelayTime = 1.0f;

        m_Y = ClampAngle(m_Y, m_YMinLimit, m_YMaxLimit);

        Quaternion rotation = Quaternion.Euler(m_Y, m_X, 0);

        Vector3 direction = new Vector3(0.0f, 0.0f, -m_Distance);
        Vector3 position = (rotation * direction) + m_Target.position;

        transform.rotation = rotation;
        transform.position = position;
    }

    // Clamp angle around 0 to 360 degree
    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    // Set zooming method
    public void SetZoomMethod(ZoomMethod zoomMethod)
    {
        m_ZoomMethod = zoomMethod;
    }
}
