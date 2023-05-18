using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam_1 : MonoBehaviour
{

    public static FollowCam_1 Instance;

    public Transform objectToFollow;

    public Vector3 offset;
    public float followSpeed = 10;
    public float lookSpeed = 10;
    //public float wallPush = 0.7f;
    //public float distFromTarget = 2;
    //public float heightFromTarget = 2f;
    //public MeshRenderer[] carTargets;

    //public float moveSpeed = 5;
    //public float evenCloserDistanceToPlayer = 1;
    //public float closestDistanceToPlayer = 2;
    //MeshRenderer targetRenderer;
    //public bool changeTransparency = true;

    public float returnSpeed = 9;

    public LayerMask collisionMask;

    void Start()
    {
        
        Instance = this;
        //switch (objectToFollow.gameObject.name)
        //{
        //    case "PoliceCar":
        //        targetRenderer = carTargets[0];
        //        break;
        //    case "Diesel":
        //        targetRenderer = carTargets[1];
        //        break;
        //    case "Lembo":
        //        targetRenderer = carTargets[2];
        //        break;
        //    case "Pardo":
        //        targetRenderer = carTargets[3];
        //        break;
        //}

        Invoke("player_ins", 4f);
    }


    void player_ins()
    {
        objectToFollow = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void LateUpdate()
    {
        LookAtTarget();
        MoveToTarget();
        //CollisionCheck(objectToFollow.position - (transform.forward * distFromTarget + objectToFollow.up * heightFromTarget));
    }

    public void LookAtTarget()
    {
        Vector3 _lookDirection = objectToFollow.position - transform.position;
        Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookSpeed * Time.deltaTime);
    }

    public void MoveToTarget()
    {
        Vector3 _targetPos = objectToFollow.position + objectToFollow.forward * offset.z + objectToFollow.right * offset.x + objectToFollow.up * offset.y;
        transform.position = Vector3.Lerp(transform.position, _targetPos, followSpeed * Time.deltaTime);
    }
        private void WallCheck()
    {

        Ray ray = new Ray(objectToFollow.position, -objectToFollow.forward);
        RaycastHit hit;

        if (Physics.SphereCast(ray, 0.2f, out hit, 0.7f, collisionMask))
        {
            // pitchLock = true;
        }
        else
        {
            // pitchLock = false;
        }

    }
    //private void CollisionCheck(Vector3 retPoint)
    //{

    //    RaycastHit hit;

    //    if (Physics.Linecast(objectToFollow.position, retPoint, out hit, collisionMask))
    //    {
    //        Vector3 norm = hit.normal * wallPush;
    //        Vector3 p = hit.point + norm;
    //        TransparencyCheck();
    //        if (Vector3.Distance(Vector3.Lerp(transform.position, p, moveSpeed * Time.deltaTime), objectToFollow.position) <= evenCloserDistanceToPlayer)
    //        {
    //        }
    //        else
    //        {
    //            transform.position = Vector3.Lerp(transform.position, p, moveSpeed * Time.deltaTime);
    //        }
    //        return;
    //    }
    //    FullTransparency();
    //    transform.position = Vector3.Lerp(transform.position, retPoint, returnSpeed * Time.deltaTime);
    //}
    //private void TransparencyCheck()
    //{

    //    if (changeTransparency)
    //    {
    //        if (Vector3.Distance(transform.position, objectToFollow.position) <= closestDistanceToPlayer)
    //        {
    //            Color temp = targetRenderer.sharedMaterial.color;
    //            temp.a = Mathf.Lerp(temp.a, 0.2f, moveSpeed * Time.deltaTime);
    //            targetRenderer.sharedMaterial.color = temp;
    //        }
    //        else
    //        {
    //            if (targetRenderer.sharedMaterial.color.a <= 0.99f)
    //            {
    //                Color temp = targetRenderer.sharedMaterial.color;
    //                temp.a = Mathf.Lerp(temp.a, 1, moveSpeed * Time.deltaTime);
    //                targetRenderer.sharedMaterial.color = temp;
    //            }
    //        }
    //    }
    //}
    //private void FullTransparency()
    //{
    //    if (changeTransparency)
    //    {
    //        if (targetRenderer.sharedMaterial.color.a <= 0.99f)
    //        {

    //            Color temp = targetRenderer.sharedMaterial.color;
    //            temp.a = Mathf.Lerp(temp.a, 1, moveSpeed * Time.deltaTime);

    //            targetRenderer.sharedMaterial.color = temp;

    //        }
    //    }
    //}
}