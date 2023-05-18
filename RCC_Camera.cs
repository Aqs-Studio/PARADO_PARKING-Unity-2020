using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class RCC_Camera : MonoBehaviour
{
    public static RCC_Camera instance;
    // Currently rendering?
    public bool isRendering = true;
    // The target we are following transform and rigidbody.
    public GameObject playerCar;
    private Rigidbody playerRigid;
    private float playerSpeed = 0f;
    private Vector3 playerVelocity = new Vector3(0f, 0f, 0f);

    public Camera thisCam;          // Camera is not attached to this main gameobject. Camera is parented to pivot gameobject. Therefore, we can apply additional position and rotation changes.
    public GameObject pivot;        // Pivot center of the camera. Used for making offsets and collision movements.

    // Camera Modes.
    public CameraMode cameraMode;
    public enum CameraMode { TPS, FPS, WHEEL, FIXED, CINEMATIC, TOP }
    public CameraMode lastCameraMode;

    public bool useTopCameraMode = false;               // Shall we use top camera mode?
    public bool useHoodCameraMode = true;               // Shall we use hood camera mode?
    public bool useOrbitInTPSCameraMode = false;    // Shall we use orbit control in TPS camera mode?
    public bool useOrbitInHoodCameraMode = false;   // Shall we use orbit control in hood camera mode?
    public bool useWheelCameraMode = true;              // Shall we use wheel camera mode?
    public bool useFixedCameraMode = false;             // Shall we use fixed camera mode?
    public bool useCinematicCameraMode = false;     // Shall we use cinematic camera mode?
    public bool useOrthoForTopCamera = false;           // Shall we use ortho in top camera mode?
    public bool useOcclusion = true;                            // Shall we use camera occlusion?
    public LayerMask occlusionLayerMask = -1;

    public bool useAutoChangeCamera = false;            // Shall we change camera mode by auto?
    private float autoChangeCameraTimer = 0f;
    public LayerMask RCCLayer;

    public Vector3 topCameraAngle = new Vector3(45f, 45f, 0f);      // If so, we will use this Vector3 angle for top camera mode.

    private float distanceOffset = 0f;
    public float maximumZDistanceOffset = 10f;      // Distance offset for top camera mode. Related with vehicle speed. If vehicle speed is higher, camera will move to front of the vehicle.
    public float topCameraDistance = 100f;              // Top camera height / distance.

    // Used for smooth camera movements. Smooth camera movements are only used for TPS and Top camera mode.
    private Vector3 targetPosition, lastFollowerPosition = Vector3.zero;
    private Vector3 lastTargetPosition = Vector3.zero;

    // Used for resetting orbit values when direction of the vehicle has been changed.
    private int direction = 1;
    private int lastDirection = 1;

    public float TPSDistance = 6f;              // The distance for TPS camera mode.
    public float TPSHeight = 2f;                    // The height we want the camera to be above the target for TPS camera mode.
    public float TPSHeightDamping = 10f;    // Height movement damper.
    public float TPSRotationDamping = 5f;   // Rotation movement damper.
    public float TPSTiltMaximum = 15f;      // Maximum tilt angle related with rigidbody local velocity.
    public float TPSTiltMultiplier = 2f;        // Tilt angle multiplier.
    private float TPSTiltAngle = 0f;            // Current tilt angle.
    public float TPSYawAngle = 0f;          // Yaw angle.
    public float TPSPitchAngle = 7f;            // Pitch angle.

    internal float targetFieldOfView = 60f; // Camera will adapt its field of view to this target field of view. All field of views below this line will feed this value.

    public float TPSMinimumFOV = 50f;           // Minimum field of view related with vehicle speed.
    public float TPSMaximumFOV = 70f;           // Maximum field of view related with vehicle speed.
    public float hoodCameraFOV = 60f;           // Hood field of view.
    public float wheelCameraFOV = 60f;          // Wheel field of view.
    public float minimumOrtSize = 10f;          // Minimum ortho size related with vehicle speed.
    public float maximumOrtSize = 20f;          // Maximum ortho size related with vehicle speed.

    internal int cameraSwitchCount = 0;

    private Vector3 collisionVector = Vector3.zero;             // Collision vector.
    private Vector3 collisionPos = Vector3.zero;                    // Collision position.
    private Quaternion collisionRot = Quaternion.identity;  // Collision rotation.

    private float index = 0f;               // Used for sinus FOV effect after hard crashes. 

    private Quaternion orbitRotation = Quaternion.identity;     // Orbit rotation.

    // Orbit X and Y inputs.
    internal float orbitX = 0f;
    internal float orbitY = 0f;

    // Minimum and maximum Orbit X, Y degrees.
    public float minOrbitY = -20f;
    public float maxOrbitY = 80f;

    //	Orbit X and Y speeds.
    public float orbitXSpeed = 7.5f;
    public float orbitYSpeed = 5f;
    private float orbitResetTimer = 0f;

    // Calculate the current rotation angles for TPS mode.
    private Quaternion currentRotation = Quaternion.identity;
    private Quaternion wantedRotation = Quaternion.identity;
    private float currentHeight = 0f;
    private float wantedHeight = 0f;

    public delegate void onBCGCameraSpawned(GameObject BCGCamera);
    public static event onBCGCameraSpawned OnBCGCameraSpawned;

    void Awake()
    {
        // Getting Camera.
        thisCam = GetComponentInChildren<Camera>();
    }

    void OnEnable()
    {

        // Calling this event when BCG Camera spawned.
        if (OnBCGCameraSpawned != null)
            OnBCGCameraSpawned(gameObject);


        // Past positions used for proper smooting related with speed.
        lastFollowerPosition = transform.position;
        lastTargetPosition = transform.position;

    }
    void Start()
    {
        instance = this;
        playerCar = GameObject.FindGameObjectWithTag("Player");
    }

    void RCC_CarControllerV3_OnRCCPlayerCollision(GameObject playerCar, Collision collision)
    {

        Collision(collision);

    }

    void GetTarget()
    {

        // Return if we don't have the player vehicle.
        if (!playerCar)
            return;

        // If player vehicle has RCC_CameraConfig, distance and height will be adjusted.
        if (playerCar.GetComponent<RCC_CameraConfig>())
        {

            TPSDistance = playerCar.GetComponent<RCC_CameraConfig>().distance;
            TPSHeight = playerCar.GetComponent<RCC_CameraConfig>().height;

        }

        // Getting rigid of the player vehicle.
        playerRigid = playerCar.GetComponent<Rigidbody>();

        // Getting camera modes from the player vehicle.

        ResetCamera();

        // Setting transform and position to player vehicle when switched camera target.
        //		transform.position = playerCar.transform.position;
        //		transform.rotation = playerCar.transform.rotation * Quaternion.AngleAxis(10f, Vector3.right);

    }

    public void SetTarget(GameObject player)
    {

        playerCar = GameObject.FindGameObjectWithTag("Player");
        GetTarget();

    }

    public void RemoveTarget()
    {

        transform.SetParent(null);

        playerCar = null;
        playerRigid = null;

    }

    void Update()
    {
        // If it's active, enable the camera. If it's not, disable the camera.
        if (!isRendering)
        {

            if (thisCam.gameObject.activeInHierarchy)
                thisCam.gameObject.SetActive(false);

            return;

        }
        else
        {

            if (!thisCam.gameObject.activeInHierarchy)
                thisCam.gameObject.SetActive(true);

        }

        // Early out if we don't have the player vehicle.
        if (!playerCar || !playerRigid)
        {

            GetTarget();
            return;

        }

        // Speed of the vehicle (smoothed).
        playerSpeed = Mathf.Lerp(playerSpeed, playerCar.GetComponent<SimpleCarController>().CurrentSpeed, Time.deltaTime * 5f);

        // Velocity of the vehicle.
        playerVelocity = playerCar.transform.InverseTransformDirection(playerRigid.velocity);

        // Used for sinus FOV effect after hard crashes. 
        if (index > 0)
            index -= Time.deltaTime * 5f;

        // Lerping current field of view to target field of view.
        thisCam.fieldOfView = Mathf.Lerp(thisCam.fieldOfView, targetFieldOfView, Time.deltaTime * 5f);

    }

    void LateUpdate()
    {

        // Early out if we don't have the player vehicle.
        if (!playerCar || !playerRigid)
            return;

        // Even if we have the player vehicle and it's disabled, return.
        if (!playerCar.gameObject.activeSelf)
            return;

        // Run the corresponding method with choosen camera mode.
        switch (cameraMode)
        {

            case CameraMode.TPS:
                TPS();
                if (useOrbitInTPSCameraMode)
                    ORBIT();
                break;

            case CameraMode.FPS:
                FPS();
                if (useOrbitInHoodCameraMode)
                    ORBIT();
                break;

            case CameraMode.WHEEL:
                WHEEL();
                break;

            case CameraMode.FIXED:
                break;

            case CameraMode.CINEMATIC:
                break;

            case CameraMode.TOP:
                TOP();
                break;

        }

        if (lastCameraMode != cameraMode)
            ResetCamera();

        lastCameraMode = cameraMode;
        autoChangeCameraTimer += Time.deltaTime;


        if (useAutoChangeCamera && autoChangeCameraTimer > 10)
        {

            autoChangeCameraTimer = 0f;
            ChangeCamera();

        }

    }

    // Change camera by increasing camera switch counter.
    public void ChangeCamera()
    {

        // Increasing camera switch counter at each camera changing.
        cameraSwitchCount++;

        // We have 6 camera modes at total. If camera switch counter is greater than maximum, set it to 0.
        if (cameraSwitchCount >= 6)
            cameraSwitchCount = 0;

        switch (cameraSwitchCount)
        {

            case 0:
                cameraMode = CameraMode.TPS;
                break;

            case 1:


            case 2:


            case 3:


            case 4:


            case 5:
                if (useTopCameraMode)
                {
                    cameraMode = CameraMode.TOP;
                }
                else
                {
                    ChangeCamera();
                }
                break;

        }

    }

    // Change camera by directly setting it to specific mode.
    public void ChangeCamera(CameraMode mode)
    {

        cameraMode = mode;

    }

    void FPS()
    {

        // Assigning orbit rotation, and transform rotation.
        transform.rotation = Quaternion.Lerp(transform.rotation, playerCar.transform.rotation * orbitRotation, Time.deltaTime * 5f);
        thisCam.transform.localRotation = Quaternion.Lerp(thisCam.transform.localRotation, Quaternion.Euler(new Vector3(0f, Mathf.Clamp(playerVelocity.x * 1f, -25f, 25f), 0f)), Time.deltaTime * 3f);

    }

    void WHEEL()
    {

        if (useOcclusion && Occluding(playerCar.transform.position))
            ChangeCamera(CameraMode.TPS);

    }

    void TPS()
    {

        if (lastDirection != GameManager.GM.Direction)
        {

            direction = GameManager.GM.Direction;
            orbitX = 0f;
            orbitY = 0f;

        }

        lastDirection = GameManager.GM.Direction;

        // Calculate the current rotation angles for TPS mode.
        wantedRotation = playerCar.transform.rotation * Quaternion.AngleAxis((direction == 1 ? 0 : 180) + (useOrbitInTPSCameraMode ? orbitX : 0), Vector3.up);
        wantedRotation = wantedRotation * Quaternion.AngleAxis((useOrbitInTPSCameraMode ? orbitY : 0), Vector3.right);

        if (Input.GetKey(KeyCode.B))
            wantedRotation = wantedRotation * Quaternion.AngleAxis((180), Vector3.up);

        // Wanted height.
        wantedHeight = playerCar.transform.position.y + TPSHeight;

        // Damp the height.
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, TPSHeightDamping * Time.fixedDeltaTime);

        // Damp the rotation around the y-axis.
        if (Time.time > 1)
            currentRotation = Quaternion.Lerp(currentRotation, wantedRotation, TPSRotationDamping * Time.deltaTime);
        else
            currentRotation = wantedRotation;

        // Rotates camera by Z axis for tilt effect.
        TPSTiltAngle = Mathf.Lerp(0f, TPSTiltMaximum * Mathf.Clamp(-playerVelocity.x, -1f, 1f), Mathf.Abs(playerVelocity.x) / 50f);
        TPSTiltAngle *= TPSTiltMultiplier;

        // Set the position of the camera on the x-z plane to distance meters behind the target.
        targetPosition = playerCar.transform.position;
        targetPosition -= (currentRotation) * Vector3.forward * (TPSDistance * Mathf.Lerp(1f, .75f, (playerRigid.velocity.magnitude * 3.6f) / 100f));
        targetPosition += Vector3.up * (TPSHeight * Mathf.Lerp(1f, .75f, (playerRigid.velocity.magnitude * 3.6f) / 100f));

        // SMOOTHED.
        //		transform.position = SmoothApproach(pastFollowerPosition, pastTargetPosition, targetPosition, Mathf.Clamp(10f, Mathf.Abs(playerSpeed / 2f), Mathf.Infinity));
        // RAW.
        transform.position = targetPosition;

        thisCam.transform.localPosition = Vector3.Lerp(thisCam.transform.localPosition, new Vector3(TPSTiltAngle / 10f, 0f, 0f), Time.deltaTime * 3f);

        // Always look at the target.
        transform.LookAt(playerCar.transform);
        transform.eulerAngles = new Vector3(currentRotation.eulerAngles.x + (TPSPitchAngle * Mathf.Lerp(1f, .75f, (playerRigid.velocity.magnitude * 3.6f) / 100f)), transform.eulerAngles.y, -Mathf.Clamp(TPSTiltAngle, -TPSTiltMaximum, TPSTiltMaximum) + TPSYawAngle);

        // Past positions used for proper smooting related with speed.
        lastFollowerPosition = transform.position;
        lastTargetPosition = targetPosition;

        // Collision positions and rotations that affects pivot of the camera.
        collisionPos = Vector3.Lerp(new Vector3(collisionPos.x, collisionPos.y, collisionPos.z), Vector3.zero, Time.unscaledDeltaTime * 5f);

        if (Time.deltaTime != 0)
            collisionRot = Quaternion.Lerp(collisionRot, Quaternion.identity, Time.deltaTime * 5f);

        // Lerping position and rotation of the pivot to collision.
        pivot.transform.localPosition = Vector3.Lerp(pivot.transform.localPosition, collisionPos, Time.deltaTime * 10f);
        pivot.transform.localRotation = Quaternion.Lerp(pivot.transform.localRotation, collisionRot, Time.deltaTime * 10f);

        // Lerping targetFieldOfView from TPSMinimumFOV to TPSMaximumFOV related with vehicle speed.
        targetFieldOfView = Mathf.Lerp(TPSMinimumFOV, TPSMaximumFOV, Mathf.Abs(playerSpeed) / 150f);

        // Sinus FOV effect on hard crashes.
        targetFieldOfView += (5f * Mathf.Cos(index));

        if (useOcclusion)
            OccludeRay(playerCar.transform.position);

    }


    void TOP()
    {

        // Early out if we don't have the player vehicle.
        if (!playerCar || !playerRigid)
            return;

        // Setting proper targetPosition for top camera mode.
        targetPosition = playerCar.transform.position;
        targetPosition += playerCar.transform.rotation * Vector3.forward * distanceOffset;

        // Setting ortho or perspective?
        thisCam.orthographic = useOrthoForTopCamera;

        distanceOffset = Mathf.Lerp(0f, maximumZDistanceOffset, Mathf.Abs(playerSpeed) / 100f);
        targetFieldOfView = Mathf.Lerp(minimumOrtSize, maximumOrtSize, Mathf.Abs(playerSpeed) / 100f);
        thisCam.orthographicSize = targetFieldOfView;

        // Assigning position and rotation.
        transform.position = SmoothApproach(lastFollowerPosition, lastTargetPosition, targetPosition, Mathf.Clamp(.1f, Mathf.Abs(playerSpeed / 2f), Mathf.Infinity));
        transform.rotation = Quaternion.Euler(topCameraAngle);

        // Past positions used for proper smooting related with speed.
        lastFollowerPosition = transform.position;
        lastTargetPosition = targetPosition;

        // Pivot position.
        pivot.transform.localPosition = new Vector3(0f, 0f, -topCameraDistance);

    }
    void HideTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(thisCam.transform.position, thisCam.transform.TransformDirection(Vector3.forward), out hit, 2))
        {
            Debug.DrawRay(thisCam.transform.position, thisCam.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if (hit.collider.gameObject.layer == 10)
            {
                Debug.Log("Hit the Cantainer");
                hit.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    void ORBIT()
    {

        // Clamping Y.
        orbitY = Mathf.Clamp(orbitY, minOrbitY, maxOrbitY);

        orbitRotation = Quaternion.Euler(orbitY, orbitX, 0f);

        if (playerSpeed > 10f && Mathf.Abs(orbitX) > 1f)
            orbitResetTimer += Time.deltaTime;

        if (playerSpeed > 10f && orbitResetTimer >= 2f)
        {

            orbitX = 0f;
            orbitY = 0f;
            orbitResetTimer = 0f;

        }

    }

    public void OnDrag(PointerEventData pointerData)
    {

        // Receiving drag input from UI.
        orbitX += pointerData.delta.x * orbitXSpeed * .02f;
        orbitY -= pointerData.delta.y * orbitYSpeed * .02f;

        orbitResetTimer = 0f;

    }


    public void Collision(Collision collision)
    {

        // If it's not enable or camera mode is TPS, return.
        if (!enabled || !isRendering)
            return;

        // Local relative velocity.
        Vector3 colRelVel = collision.relativeVelocity;
        colRelVel *= 0f - Mathf.Abs(Vector3.Dot(transform.up, collision.contacts[0].normal));

        float cos = Mathf.Abs(Vector3.Dot(collision.contacts[0].normal, colRelVel.normalized));

        if (colRelVel.magnitude * cos >= 5f)
        {

            collisionVector = transform.InverseTransformDirection(colRelVel) / (30f);

            collisionPos -= collisionVector * 5f;
            collisionRot = Quaternion.Euler(new Vector3(-collisionVector.z * 10f, -collisionVector.y * 10f, -collisionVector.x * 10f));
            targetFieldOfView = thisCam.fieldOfView - Mathf.Clamp(collision.relativeVelocity.magnitude, 0f, 15f);
            index = Mathf.Clamp((colRelVel.magnitude * cos) * 50f, 0f, 10f);

        }

    }

    private void ResetCamera()
    {

        TPSTiltAngle = 0f;

        collisionPos = Vector3.zero;
        collisionRot = Quaternion.identity;

        thisCam.transform.localPosition = Vector3.zero;
        thisCam.transform.localRotation = Quaternion.identity;

        pivot.transform.localPosition = collisionPos;
        pivot.transform.localRotation = collisionRot;

        lastFollowerPosition = transform.position;
        lastTargetPosition = targetPosition;

        orbitX = 0f;
        orbitY = 0f;

        thisCam.orthographic = false;

        switch (cameraMode)
        {

            case CameraMode.TPS:
                transform.SetParent(null);
                targetFieldOfView = TPSMaximumFOV;
                break;

            case CameraMode.TOP:
                transform.SetParent(null);
                targetFieldOfView = minimumOrtSize;
                pivot.transform.localPosition = Vector3.zero;
                pivot.transform.localRotation = Quaternion.identity;
                targetPosition = playerCar.transform.position;
                targetPosition += playerCar.transform.rotation * Vector3.forward * distanceOffset;
                transform.position = playerCar.transform.position;
                lastFollowerPosition = playerCar.transform.position;
                lastTargetPosition = targetPosition;
                break;

        }

    }

    // Used for smooth position lerping.
    private Vector3 SmoothApproach(Vector3 pastPosition, Vector3 pastTargetPosition, Vector3 targetPosition, float delta)
    {

        if (Time.timeScale == 0 || float.IsNaN(delta) || float.IsInfinity(delta) || delta == 0 || pastPosition == Vector3.zero || pastTargetPosition == Vector3.zero || targetPosition == Vector3.zero)
            return transform.position;

        float t = (Time.deltaTime * delta) + .00001f;
        Vector3 v = (targetPosition - pastTargetPosition) / t;
        Vector3 f = pastPosition - pastTargetPosition + v;
        Vector3 l = targetPosition - v + f * Mathf.Exp(-t);

#if UNITY_2017_1_OR_NEWER
        if (l != Vector3.negativeInfinity && l != Vector3.positiveInfinity && l != Vector3.zero)
            return l;
        else
            return transform.position;
#else
		return l;
#endif

    }

    public void ToggleCamera(bool state)
    {

        // Enabling / disabling activity.
        isRendering = state;

    }

    void OccludeRay(Vector3 targetFollow)
    {

        //declare a new raycast hit.
        RaycastHit wallHit = new RaycastHit();

        //linecast from your player (targetFollow) to your cameras mask (camMask) to find collisions.
        //		if (Physics.Linecast (targetFollow, transform.position, out wallHit, ~(1 << LayerMask.NameToLayer(RCCSettingsInstance.RCCLayer)))) {
        //			
        //			if (!wallHit.collider.isTrigger && !wallHit.transform.IsChildOf (playerCar.transform)) {
        //
        //				//the x and z coordinates are pushed away from the wall by hit.normal.
        //				//the y coordinate stays the same.
        //				Vector3 occludedPosition = new Vector3 (wallHit.point.x + wallHit.normal.x * .2f, wallHit.point.y + wallHit.normal.y * .2f, wallHit.point.z + wallHit.normal.z * .2f);
        //
        //				transform.position = occludedPosition;
        //
        //			}
        //
        //		}

        if (Physics.Linecast(targetFollow, transform.position, out wallHit, occlusionLayerMask))
        {

            if (!wallHit.collider.isTrigger && !wallHit.transform.IsChildOf(playerCar.transform))
            {

                //the x and z coordinates are pushed away from the wall by hit.normal.
                //the y coordinate stays the same.
                Vector3 occludedPosition = new Vector3(wallHit.point.x + wallHit.normal.x * .2f, wallHit.point.y + wallHit.normal.y * .2f, wallHit.point.z + wallHit.normal.z * .2f);

                transform.position = occludedPosition;

            }

        }

    }

    bool Occluding(Vector3 targetFollow)
    {

        //declare a new raycast hit.
        RaycastHit wallHit = new RaycastHit();

        //linecast from your player (targetFollow) to your cameras mask (camMask) to find collisions.
        if (Physics.Linecast(targetFollow, transform.position, out wallHit, ~(1 << RCCLayer)))
        {

            if (!wallHit.collider.isTrigger && !wallHit.transform.IsChildOf(playerCar.transform))
                return true;

        }

        return false;

    }

    void OnDisable()
    {

        // RCC_CarControllerV3.OnRCCPlayerCollision -= RCC_CarControllerV3_OnRCCPlayerCollision;

    }

}