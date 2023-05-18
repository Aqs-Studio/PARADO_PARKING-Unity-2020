using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NWH.VehiclePhysics;

public class SimpleCarController_new : MonoBehaviour
{
    public static SimpleCarController_new Instance;

    private float m_horizontalInput;
    private float m_verticalInput = 0;
    private float m_verticalInputbrake;
    private float m_steeringAngle;
    private float m_breakInput;

    public WheelCollider WheelColl_FR, WheelColl_FL;
    public WheelCollider WheelColl_BR, WheelColl_BL;

    public Transform Wheel_FR, Wheel_FL;
    public Transform Wheel_BR, Wheel_BL;

    [Space]
    [Space]

    public float MaxSteerAngle;
    public float MotorForce;
    public float BrakeForce;
    public float TurnForce;
    public float Drag;
    [Space]
    public Transform COM;
    [HideInInspector]
    public Rigidbody r_body;

    private float MaxMotorForce;
    private float MinMotorForce;
    private float MotorForceIncrement;
    public float CurrentSpeed;

    private float Tilt_x;

    public bool HitOnce;
    void Start()
    {
        Instance = this;
        HitOnce = false;
        r_body = GetComponent<Rigidbody>();
        MinMotorForce = MotorForce;
        MaxMotorForce = MotorForce * 10.5f;
        MotorForceIncrement = MotorForce / 5200f;//2000
        r_body = GetComponent<Rigidbody>();
        r_body.centerOfMass = COM.localPosition;

    }

    private void FixedUpdate()
    {
        if (HitOnce)
        {
            return;
        }
        GetInput();
        Steer();
        UpdateWheelPoses();
        //if(park1 && park2 && park3 && park4)
        //{
        //    if(r_body.velocity.x>= -0.15f && r_body.velocity.x <= 0.15f && r_body.velocity.z>= -0.15f && r_body.velocity.z <= 0.15f )
        //    {

        //    }
        //}

    }
    public void Steer_By_PaddleControl()
    {
        if (parado_manger.GM.Left || Input.GetKey(KeyCode.LeftArrow))
        {
            if (m_horizontalInput > -1)
                m_horizontalInput -= TurnForce / 80;  // /100   // *0.01f
            //m_verticalInput = 0.7f;
        }
        else if (parado_manger.GM.Right || Input.GetKey(KeyCode.RightArrow))
        {
            //m_verticalInput = 0.7f;
            if (m_horizontalInput < 1)
                m_horizontalInput += TurnForce / 80f;   //   / 100f  * 0.01f
        }
        else
        {
            if (m_horizontalInput > (TurnForce / 100f * 1.3f))  //    (TurnForce / 100f * 5)   0.05   //  (TurnForce *0.01f * 5) )
                m_horizontalInput -= (TurnForce / 100f * 1.3f);    // TurnForce / 100f * 5; 
            else if (m_horizontalInput < -(TurnForce / 100f * 1.3f))     //(TurnForce / 100f * 5)
                m_horizontalInput += (TurnForce / 100f * 1.3f);    //TurnForce / 100f * 5;
            else
                m_horizontalInput = 0;
        }

    }

    public void Steer_By_SteeringWheel()
    {
        m_horizontalInput = SteeringWheel.wheelAngle / 540;
    }

    public void Steer_By_Tilt()
    {
        float tilt_Val = Input.acceleration.x;
        if (tilt_Val < -0.1f || tilt_Val > 0.1f)
            m_horizontalInput = Mathf.Clamp(tilt_Val * 6f, -1, 1);//tilt_Val*5f
        else
            m_horizontalInput = 0;
    }

    public void GetInput()
    {
        if (parado_manger.GM.Brake || Input.GetKey(KeyCode.DownArrow))
        {
            WheelColl_BR.brakeTorque = BrakeForce;    //10*BrakeForce;
            WheelColl_BL.brakeTorque = BrakeForce;
            WheelColl_FL.brakeTorque = BrakeForce;
            WheelColl_FR.brakeTorque = BrakeForce;
            r_body.drag = Drag;
            //r_body.isKinematic = true;
            // AudioManager.instance.StopAccelerate();
        }
        else if (parado_manger.GM.Accelerate || Input.GetKey(KeyCode.UpArrow))
        {
            WheelColl_BR.brakeTorque = 0;
            WheelColl_BL.brakeTorque = 0;
            WheelColl_FL.brakeTorque = 0;
            WheelColl_FR.brakeTorque = 0;

            r_body.drag = 0;
            m_verticalInput = 0.7f;

            if (MotorForce < MaxMotorForce)
                MotorForce += MotorForceIncrement;
            // AudioManager.instance.Accelerate();
            WheelColl_BL.suspensionDistance = 0.30f;
            WheelColl_BR.suspensionDistance = 0.30f;
        }
        else
        {
            WheelColl_BL.suspensionDistance = 0.39f;
            WheelColl_BR.suspensionDistance = 0.39f;
            r_body.drag = 0.5f;
            WheelColl_BR.brakeTorque = 750;
            WheelColl_BL.brakeTorque = 750;
            WheelColl_FL.brakeTorque = 750;
            WheelColl_FR.brakeTorque = 750;

            if (MotorForce > MinMotorForce)
            {

                MotorForce -= MotorForceIncrement * 30;//
            }
            // AudioManager.instance.StopAccelerate();
        }
        //***************************************************************************************************************************************

        if (parado_manger.GM.AssignedControl == parado_manger.GameControl.Steering)
        {
            Steer_By_SteeringWheel();
        }
        else if (parado_manger.GM.AssignedControl == parado_manger.GameControl.Paddle)
        {
            Steer_By_PaddleControl();
        }
        else
        {
            Steer_By_Tilt();
        }
        WheelColl_FL.motorTorque = m_verticalInput * MotorForce * parado_manger.GM.Direction;
        WheelColl_FR.motorTorque = m_verticalInput * MotorForce * parado_manger.GM.Direction;
        WheelColl_BR.motorTorque = m_verticalInput * MotorForce * parado_manger.GM.Direction;
        WheelColl_BL.motorTorque = m_verticalInput * MotorForce * parado_manger.GM.Direction;
    }

    private void Steer()
    {
        m_steeringAngle = MaxSteerAngle * m_horizontalInput;
        WheelColl_FR.steerAngle = m_steeringAngle;
        WheelColl_FL.steerAngle = m_steeringAngle;
    }
    private void CalculateSpeed()
    {
        // CurrentSpeed = Mathf.RoundToInt(2 * 22 / 7 * WheelColl_FL.radius * WheelColl_FL.rpm * 60 / 1000);

        // if (CurrentSpeed >= 0 && CurrentSpeed < 30)
        // {
        //     MotorForce = MaxMotorForce;
        // }

        // else if (CurrentSpeed >= 30)
        // {
        //     MotorForce = 300;
        // }
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(WheelColl_FR, Wheel_FR);
        UpdateWheelPose(WheelColl_FL, Wheel_FL);
        UpdateWheelPose(WheelColl_BR, Wheel_BR);
        UpdateWheelPose(WheelColl_BL, Wheel_BL);
    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }
    public void OnCollisionEnter(Collision coll)
    {
        if (!HitOnce)
        {


            if (coll.gameObject.tag == "Lethal" || coll.gameObject.tag == "BigLehtal" || coll.gameObject.tag == "MovingLethal")
            {
                parado_manger.GM.Accelerate = false;
                if (PlayerPrefs.GetInt("FailAds") < 2)
                {
                    PlayerPrefs.SetInt("FailAds", PlayerPrefs.GetInt("FailAds") + 1);
                }
                WheelColl_BR.brakeTorque = BrakeForce * 1000;    //10*BrakeForce;
                WheelColl_BL.brakeTorque = BrakeForce * 1000;
                r_body.drag = 50;
                r_body.constraints = RigidbodyConstraints.FreezeAll;
                // AudioManager.instance.StopAccelerate();
                parado_manger.GM.Accelerate = false;

                HitOnce = true;
             //   parado_manger.GM.OnHit(coll.gameObject);
            }
        }
    }
    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "front" )
        {
            Debug.Log("hiitteded");
         
            PlayerPrefs.SetInt("front", 1);
            game_manager.instance.ChangeRingColors();
          
            // AudioManager.instance.StopAccelerate();
          //  this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            if (PlayerPrefs.GetInt("WinAds") < 2)
            {
                PlayerPrefs.SetInt("WinAds", PlayerPrefs.GetInt("WinAds") + 1);
            }

            //    parado_manger.GM.Game_Controls.SetActive(false);

            //  parado_manger.GM.win_Splash.SetActive(true);
            //   int rand = (int)Random.Range(0, parado_manger.GM.win_Quotes.Length);
            // rand = (int)Mathf.Clamp(rand, 0, parado_manger.GM.win_Quotes.Length - 1);
            //   parado_manger.GM.win_Splash_label.text = parado_manger.GM.win_Quotes[rand];
            //   StartCoroutine(LevelCompleteDelay());
            
        }

        if (coll.gameObject.tag == "back")
        {
            Debug.Log("hiitteded_back");
            PlayerPrefs.SetInt("back", 1);
           
            game_manager.instance.ChangeRingColors();

            // AudioManager.instance.StopAccelerate();
            // this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            if (PlayerPrefs.GetInt("WinAds") < 2)
            {
                PlayerPrefs.SetInt("WinAds", PlayerPrefs.GetInt("WinAds") + 1);
            }

            //    parado_manger.GM.Game_Controls.SetActive(false);

            //  parado_manger.GM.win_Splash.SetActive(true);
            //   int rand = (int)Random.Range(0, parado_manger.GM.win_Quotes.Length);
            // rand = (int)Mathf.Clamp(rand, 0, parado_manger.GM.win_Quotes.Length - 1);
            //   parado_manger.GM.win_Splash_label.text = parado_manger.GM.win_Quotes[rand];
            //  StartCoroutine(LevelCompleteDelay());
        }


        if (coll.gameObject.tag == "ParkPointy")
        {
            // AudioManager.instance.StopAccelerate();
            coll.GetComponentInParent<Animator>().enabled = true;
            coll.GetComponent<BoxCollider>().enabled = false;
         //   coll.gameObject.GetComponent<PlatformColor>().parentRender.material.color = Color.green;
            this.gameObject.transform.parent = coll.transform;
            this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
            this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
         //   GameManager.GM.Game_Controls.SetActive(false);
            StartCoroutine(WakeUp_Player(coll.gameObject));
        }
        if (coll.gameObject.tag == "WayPoint")
        {
            Debug.Log("WayPointChecked");
         //   GameManager.GM.isCheckPointTrigger = true;
         //   GameManager.GM.currentSpwanPoint = coll.gameObject;
        }

    }
    IEnumerator LevelCompleteDelay()
    {
        yield return new WaitForSeconds(4f);
        //    GameManager.GM.win_Splash.SetActive(false);
        //  GameManager.GM.Level_Complete();
        Application.LoadLevel(1);
      
    }
    IEnumerator WakeUp_Player(GameObject rend)
    {
        yield return new WaitForSeconds(2f);
        this.gameObject.transform.parent = null;
        this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
       // rend.gameObject.GetComponent<PlatformColor>().parentRender.material.color = Color.white;
      //  GameManager.GM.Game_Controls.SetActive(true);
    }

}
