using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class HeadControlledRotation : MonoBehaviour
{
    ARFace arFace;
    ARFaceManager faceManager;

    [Header("Rotation Sensitivity")]
    public float yawMultiplier = 1.2f;
    public float pitchMultiplier = 1.0f;

    [Header("Rotation Limits")]
    public float maxYaw = 40f;
    public float maxPitch = 30f;

    [Header("Smoothing")]
    public float smoothSpeed = 6f;

    Quaternion initialRotation;

    void Awake()
    {
        faceManager = FindObjectOfType<ARFaceManager>();
    }

    void Start()
    {
        initialRotation = transform.rotation;
    }

    void OnEnable()
    {
        if (faceManager != null)
            faceManager.facesChanged += OnFacesChanged;
    }

    void OnDisable()
    {
        if (faceManager != null)
            faceManager.facesChanged -= OnFacesChanged;
    }

    void OnFacesChanged(ARFacesChangedEventArgs args)
    {
        if (arFace == null && args.added.Count > 0)
        {
            arFace = args.added[0]; // first detected face
        }
    }

    void Update()
    {
        if (arFace == null) return;

        Vector3 faceEuler = arFace.transform.rotation.eulerAngles;

        float yaw = NormalizeAngle(faceEuler.y) * yawMultiplier;
        float pitch = NormalizeAngle(faceEuler.x) * pitchMultiplier;

        yaw = Mathf.Clamp(yaw, -maxYaw, maxYaw);
        pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);

        Quaternion targetRotation =
            initialRotation *
            Quaternion.Euler(-pitch, -yaw, 0f);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * smoothSpeed
        );
    }

    float NormalizeAngle(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}
