using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NWH.VehiclePhysics;
using UnityEngine.UI;

public class SimpleCarController : MonoBehaviour
{
    public static SimpleCarController Instance;


    private float m_horizontalInput;
    private float m_verticalInput = 0;
    private float m_verticalInputbrake;
    private float m_steeringAngle;
    private float m_breakInput;

    public Transform COM;
    public WheelCollider WheelColl_FR, WheelColl_FL;
    public WheelCollider WheelColl_BR, WheelColl_BL;

    public Transform Wheel_FR, Wheel_FL;
    public Transform Wheel_BR, Wheel_BL;


    public Material DummyMaterial;
    [Space]
    [Space]

    public float MaxSteerAngle;
    public float MotorForce;
    public float BrakeForce;
    public float TurnForce;
    public float turn_f_return;
    public float Drag;


    [Space]
    [Space]

    [HideInInspector]
    public Rigidbody r_body;

    private float MaxMotorForce;


    [Space]
    [Space]
    public float CurrentSpeed;

    private float Tilt_x;


    public int COUNTER_HIT;
    public bool HitOnce;
    public Text hit_counter_text;

    public GameObject reverse_lights;
    public GameObject break_lights;
    public float thurst;
    
    void Start()
    {

        parado_manger.GM.Direction = 1;

        Instance = this;
        HitOnce = false;
        COUNTER_HIT = 0;

        MaxMotorForce = MotorForce;

        r_body = GetComponent<Rigidbody>();
        r_body.centerOfMass = COM.localPosition;

       
       


    }

    void Update()
    {
      //  hit_counter_text.text = COUNTER_HIT.ToString();

        if(parado_manger.GM.break_bool == true)
        {
            break_lights.SetActive(true);
            Debug.Log("tru_-bveee");
            
           

        }
        else
        {
            break_lights.SetActive(false);
        }

        if(parado_manger.GM.Reverse_bool == true)
        {
            reverse_lights.SetActive(true);
        }else
        {
            reverse_lights.SetActive(false);
        }
        
    }

    private void FixedUpdate()
    {
        if (HitOnce)
        {
            return;
        }
        CalculateSpeed();
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

    private void CalculateSpeed()
    {
        CurrentSpeed = Mathf.RoundToInt(2 * 22 / 7 * WheelColl_FL.radius * WheelColl_FL.rpm * 60 / 1000);

        //if (CurrentSpeed >= 5 && CurrentSpeed < 15)
        //{
        //    //MaxSteerAngle = 30;
        //    //if (FollowCam.Instance.followSpeed > 2)
        //    //    FollowCam.Instance.followSpeed -= 0.1f;
        //    MotorForce = MaxMotorForce;
        //}


        //if (CurrentSpeed >= 15)
        //{
        //    //MaxSteerAngle = 15;
        //    //if(FollowCam.Instance.followSpeed<=4)
        //    //    FollowCam.Instance.followSpeed += 0.1f;
        //    MotorForce = 300;
        //}
        //else if (CurrentSpeed <= 15)
        //{
        //    MotorForce = 5000;
        //}
    }

    public void Steer_By_PaddleControl()
    {
        if (parado_manger.GM.Left || Input.GetKey(KeyCode.LeftArrow))
        {
            if (m_horizontalInput > -1)
                m_horizontalInput -= TurnForce / 100;  // /100   // *0.01f
            CalculateSpeed();
            ////m_verticalInput = 0.7f;
            //print("left");
            ////Turn speed slow
            //if (CurrentSpeed >= 20)
            //{
            //  ///  CurrentSpeed =- 15;
            //    print("slowwwwwwww");
            //}
        }
        else if (parado_manger.GM.Right || Input.GetKey(KeyCode.RightArrow))
        {
            //m_verticalInput = 0.7f;
            if (m_horizontalInput < 1)
                m_horizontalInput += TurnForce / 100f;   //   / 100f  * 0.01f
            CalculateSpeed();
            //print("right");
            //Turn speed slow
            //if (CurrentSpeed >= 20)
            //{
            //    CurrentSpeed = -15;
            //    print("slowwwwwwww");
            //}
        }
        else
        {
            if (m_horizontalInput > (turn_f_return / 100f))  //    (TurnForce / 100f * 5)   0.05   //  (TurnForce *0.01f * 5) )
                m_horizontalInput -= (turn_f_return / 100f);    // TurnForce / 100f * 5; 
            else if (m_horizontalInput < -(turn_f_return / 100f))     //(TurnForce / 100f * 5)
                m_horizontalInput += (turn_f_return / 100f);    //TurnForce / 100f * 5;
            else
                m_horizontalInput = 0;
        }

    }

    public void Steer_By_SteeringWheel()
    {
        m_horizontalInput = SteeringWheels.wheelAngle / 540;
    }

    public void Steer_By_Tilt()
    {
        float tilt_Val = Input.acceleration.x;
        if (tilt_Val < -0.01f || tilt_Val > 0.01f)
            m_horizontalInput = Mathf.Clamp(tilt_Val * 5f, -1, 1);
        else
            m_horizontalInput = 0;
    }

    public void GetInput()
    {
        if (parado_manger.GM.Brake || Input.GetKey(KeyCode.DownArrow))
        {
            WheelColl_BR.brakeTorque = BrakeForce;    //10*BrakeForce;
            WheelColl_BL.brakeTorque = BrakeForce;

         //   WheelColl_FR.brakeTorque = BrakeForce;    //10*BrakeForce;
        //    WheelColl_FL.brakeTorque = BrakeForce;

            
//            AudioManager.instance.StopAccelerate();

            r_body.drag = Drag;
            //r_body.isKinematic = true;
            //AudioManager.instance.StopAccelerate(); 
            GameObject.Find("BL").GetComponent<WheelCollider>().suspensionDistance = 0.50f;
            GameObject.Find("BR").GetComponent<WheelCollider>().suspensionDistance = 0.50f;
        }
        else if (parado_manger.GM.Accelerate || Input.GetKey(KeyCode.UpArrow))
        {
           
            WheelColl_BR.brakeTorque = 0;
            WheelColl_BL.brakeTorque = 0;
            WheelColl_FR.brakeTorque = 0;
            WheelColl_FL.brakeTorque = 0;
            r_body.drag = 0;
            m_verticalInput = 1;
            print("move");
            GameObject.Find("BL").GetComponent<WheelCollider>().suspensionDistance = 0.25f;
            GameObject.Find("BR").GetComponent<WheelCollider>().suspensionDistance = 0.25f;

            //            AudioManager.instance.Accelerate();

            //AudioManager.instance.Accelerate();
        }
        else
        {
            GameObject.Find("BL").GetComponent<WheelCollider>().suspensionDistance = 0.39f;
            GameObject.Find("BR").GetComponent<WheelCollider>().suspensionDistance = 0.39f;
            r_body.drag = 1.2f;
            WheelColl_BR.brakeTorque = 0;
            WheelColl_BL.brakeTorque = 0;
            WheelColl_FR.brakeTorque = 0;
            WheelColl_FL.brakeTorque = 0;
            m_verticalInput = 0f;

//            AudioManager.instance.StopAccelerate();

            //AudioManager.instance.StopAccelerate();    
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



      
        WheelColl_BR.motorTorque = m_verticalInput * MotorForce*parado_manger.GM.Direction;
        WheelColl_BL.motorTorque = m_verticalInput * MotorForce* parado_manger.GM.Direction;
       
        //  WheelColl_FR.motorTorque = m_verticalInput * MotorForce* parado_manger.GM.Direction;
        //   WheelColl_FL.motorTorque = m_verticalInput * MotorForce* parado_manger.GM.Direction;


    }

    private void Steer()
    {
        m_steeringAngle = MaxSteerAngle * m_horizontalInput;
        WheelColl_FR.steerAngle = m_steeringAngle;
        WheelColl_FL.steerAngle = m_steeringAngle;
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
        
        if (coll.gameObject.tag == "Lethal" || coll.gameObject.tag == "BigLehtal" || coll.gameObject.tag == "MovingLethal")
        {
            // parado_manger.GM.OnHit(coll.gameObject);
              COUNTER_HIT++;
              print(COUNTER_HIT+"counter_hit");
            if(COUNTER_HIT==3)
            {
            //   parado_manger.GM.Level_fialed_after_15();
            }
            
        }

        if (PlayerPrefs.GetInt("CurrentLevel") <= 14 ) {
        if (!HitOnce)
        {
            if (coll.gameObject.tag == "Lethal" || coll.gameObject.tag == "BigLehtal" || coll.gameObject.tag == "MovingLethal")
            {
                PlayerPrefs.SetInt("FailAds", PlayerPrefs.GetInt("FailAds") + 1);
                WheelColl_BR.brakeTorque = BrakeForce * 1000;    //10*BrakeForce;
                WheelColl_BL.brakeTorque = BrakeForce * 1000;
                WheelColl_FR.brakeTorque = BrakeForce * 1000;    //10*BrakeForce;
                WheelColl_FL.brakeTorque = BrakeForce * 1000;
              parado_manger.GM.Accelerate = false;
                HitOnce = true;
           //   parado_manger.GM.OnHit(coll.gameObject);
                r_body.isKinematic = true;
//                AudioManager.instance.EngineSound.pitch = AudioManager.instance.MinimumPitch;
            }
        }

        }

        

   

    }
   






        void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "FinishPoint")
        {
            PlayerPrefs.SetInt("WinAds", PlayerPrefs.GetInt("WinAds") + 1);
       //     if (RCC_Camera.instance != null)
           
            r_body.isKinematic = true;
          
//            AudioManager.instance.EngineSound.pitch = AudioManager.instance.MinimumPitch;

        }

        if (coll.gameObject.tag == "RespawnPoint")
        {

          parado_manger.GM.RespawnPoint = coll.gameObject.transform;
            //Destroy(coll.gameObject);

        }
    }
    

    public void FreeTheCar()
    {
        WheelColl_BR.brakeTorque = 0;    //10*BrakeForce;
        WheelColl_BL.brakeTorque = 0;
        WheelColl_FR.brakeTorque = 0;    //10*BrakeForce;
        WheelColl_FL.brakeTorque = 0;


        r_body.isKinematic = false;
        r_body.drag = 0;


        HitOnce = false;

    }

}


