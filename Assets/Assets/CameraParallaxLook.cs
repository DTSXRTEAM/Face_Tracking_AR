using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CameraParallaxLook : MonoBehaviour
{
    public Transform cameraRig;
    public Transform lookTarget;
    public ARFaceManager faceManager;

#if UNITY_EDITOR
    public Transform simulatedFace;
#endif

    [Header("Movement")]
    public float sensitivity = 2.5f;
    public float smoothTime = 0.15f;
    public float maxOffset = 0.35f;

    Vector3 velocity;

    void LateUpdate()
    {
        // ✅ Initialize to safe defaults
        float x = 0f;
        float y = 0f;
        bool hasInput = false;

#if UNITY_EDITOR
        if (simulatedFace != null)
        {
            x = -simulatedFace.localPosition.x * sensitivity;
            y = -simulatedFace.localPosition.y * sensitivity;
            hasInput = true;
        }
#else
        if (faceManager != null)
        {
            foreach (var face in faceManager.trackables)
            {
                Vector3 faceLocal =
                    Camera.main.transform.InverseTransformPoint(face.transform.position);

                x = -faceLocal.x * sensitivity;
                y = -faceLocal.y * sensitivity;
                hasInput = true;
                break;
            }
        }
#endif

        if (!hasInput) return;

        // Clamp movement
        x = Mathf.Clamp(x, -maxOffset, maxOffset);
        y = Mathf.Clamp(y, -maxOffset, maxOffset);

        Vector3 target = new Vector3(x, y, 0); // ❌ no forward/back

        cameraRig.localPosition = Vector3.SmoothDamp(
            cameraRig.localPosition,
            target,
            ref velocity,
            smoothTime
        );

        // Always look at center
        transform.LookAt(lookTarget);
    }
}
