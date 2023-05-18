using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{

    public static FollowCam Instance;

    public Transform objectToFollow;

    public Vector3 offset;
    public float followSpeed = 10;
    public float lookSpeed = 10;

    void Start()
    {
        Instance = this;
        Invoke("player_ins", 2f);

    }
    void player_ins()
    {
        objectToFollow = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void FixedUpdate()
    {
        LookAtTarget();
        MoveToTarget();
    }

    public void LookAtTarget()
    {
        Vector3 _lookDirection = objectToFollow.position - transform.position;
        Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookSpeed * Time.deltaTime);
    }

    public void MoveToTarget()
    {
        Vector3 _targetPos = objectToFollow.position +
                             objectToFollow.forward * offset.z +
                             objectToFollow.right * offset.x +
                             objectToFollow.up * offset.y;
        transform.position = Vector3.Lerp(transform.position, _targetPos, followSpeed * Time.deltaTime);
    }



}
