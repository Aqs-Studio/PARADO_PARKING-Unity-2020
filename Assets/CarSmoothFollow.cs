// DecompilerFi decompiler from Assembly-CSharp.dll class: CarSmoothFollow
using UnityEngine;
using System.Collections;
using System.ComponentModel.Design;

public class CarSmoothFollow : MonoBehaviour
{
    public enum ThemeTyp
    {
        T1,
        T2,
        T3,
        T4,
        T5,
        T6,
        T7
    }

    public Transform target;

    public float distance = 20f;

    public float height = 5f;

    public float heightDamping = 2f;

    public float leftright;

    public float lookAtHeight;

    public Rigidbody parentRigidbody;

    public float rotationSnapTime = 0.3f;

    public float distanceSnapTime;

    public float distanceMultiplier;

    [Header("Theme Type")]
    public ThemeTyp thmType;

    private Vector3 lookAtVector;

    private float usedDistance;

    private float wantedRotationAngle;

    private float wantedHeight;

    private float currentRotationAngle;

    private float currentHeight;

    private Quaternion currentRotation;

    private Vector3 wantedPosition;

    private float yVelocity;

    private float zVelocity;

    private Transform carBody;

    private Transform newPoint;

    private void Start()
    {
        if (thmType == ThemeTyp.T1)
        {
            target = GameObject.Find("Car_11_pysics(Clone)").transform;
        }
        else if (thmType == ThemeTyp.T2)
        {
            target = GameObject.Find("mitsubishi").transform;
        }
        else if (thmType == ThemeTyp.T3)
        {
            target = GameObject.Find("bmw").transform;
        }
        else if (thmType == ThemeTyp.T4)
        {
            target = GameObject.Find("Dodge").transform;
        }
        else if (thmType == ThemeTyp.T5)
        {
            target = GameObject.Find("car5").transform;
        }
        else if (thmType == ThemeTyp.T6)
        {
            target = GameObject.Find("car6pick").transform;
        }
        else if (thmType == ThemeTyp.T7)
        {
            target = GameObject.Find("car7vosvos").transform;
        }
        carBody = target;
        parentRigidbody = target.GetComponent<Rigidbody>();
        lookAtVector = new Vector3(0f, lookAtHeight, 0f);
    }

    private void LateUpdate()
    {
        Vector3 position = target.position;
        wantedHeight = position.y + height;
        Vector3 position2 = base.transform.position;
        currentHeight = position2.y;
        Vector3 eulerAngles = target.eulerAngles;
        wantedRotationAngle = eulerAngles.y + leftright;
        Vector3 eulerAngles2 = base.transform.eulerAngles;
        currentRotationAngle = eulerAngles2.y;
        currentRotationAngle = Mathf.SmoothDampAngle(currentRotationAngle, wantedRotationAngle, ref yVelocity, rotationSnapTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
        wantedPosition = target.position;
        wantedPosition.y = currentHeight;
        usedDistance = Mathf.SmoothDampAngle(usedDistance, distance + parentRigidbody.velocity.magnitude * distanceMultiplier, ref zVelocity, distanceSnapTime);
        wantedPosition += Quaternion.Euler(0f, currentRotationAngle, 0f) * new Vector3(0f, 0f, 0f - usedDistance);
        base.transform.position = wantedPosition;
        base.transform.LookAt(target.position + lookAtVector);
    }

    public void SetLook(Transform _newTarget)
    {
    }
}

