using System;
using UnityEngine;

public class DriftCamera : MonoBehaviour
{
    //[Serializable]
    // public class AdvancedOptions
    // {
    //     public bool updateCameraInUpdate;
    //     public bool updateCameraInFixedUpdate = true;
    //     public bool updateCameraInLateUpdate;
    //     public KeyCode switchViewKey = KeyCode.Space;
    // }
   // public static DriftCamera instance;
    public float smoothing = 6f;
    public Transform lookAtTarget;
    public Transform positionTarget;
   // public Transform topView;
   // public AdvancedOptions advancedOptions;
//[HideInInspector]
   // public bool m_ShowingTopView = false;
    void Start ()
    {
       // instance = this;
         Invoke("SetTarget",0.1f);
    }
    void SetTarget ()
    {
        lookAtTarget =  GameObject.Find("CameraLookAt").transform;
        positionTarget = GameObject.Find("CameraPos").transform;
    }
    // private void FixedUpdate ()
    // {
    //     if(advancedOptions.updateCameraInFixedUpdate)
    //         UpdateCamera ();
    // }

    private void Update ()
    {
       // if (Input.GetKeyDown (advancedOptions.switchViewKey))
         //   m_ShowingTopView = !m_ShowingTopView;
        //if(advancedOptions.updateCameraInUpdate)
            UpdateCamera ();
    }

    // private void LateUpdate ()
    // {
    //     if(advancedOptions.updateCameraInLateUpdate)
    //         UpdateCamera ();
    // }

    private void UpdateCamera ()
    {
       // if (m_ShowingTopView)
       // {
      //      transform.position = topView.position;
           // transform.rotation = topView.rotation;
       // }
       // else
       // {
          if(lookAtTarget && positionTarget)
          {
            transform.position = Vector3.Lerp(transform.position, positionTarget.position, Time.deltaTime * smoothing);
            transform.LookAt(lookAtTarget);
          }

       // }
    }
}
