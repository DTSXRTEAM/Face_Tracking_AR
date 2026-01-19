using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FaceDrivenObjectMove : MonoBehaviour
{
    [Header("Face Input")]
    public ARFaceManager faceManager;

    [Header("Move Target")]
    public Transform objectRoot;      // MoveTarget (face-driven)

    [Header("Camera Control")]
    public Transform cameraBase;      // Fixed reference
    public Transform cameraTransform; // Main Camera
    public Transform lookTarget;      // Camera look object

    [Header("Movement")]
    public float sensitivity = 2.5f;
    public float smoothTime = 0.12f;
    public float maxOffset = 1.0f;

    Vector3 velocity;
    Vector3 currentOffset;

    // Delta tracking (multi-face safe)
    Vector3 lastFaceLocalPos;
    bool hasLastFacePos = false;

    void LateUpdate()
    {
        if (faceManager == null || objectRoot == null) return;

        bool faceFound = false;
        Vector3 faceLocal = Vector3.zero;

        // ✅ Use ANY available face (no single-face lock)
        foreach (var face in faceManager.trackables)
        {
            faceLocal = face.transform.localPosition;
            faceFound = true;
            break;
        }

        if (!faceFound)
        {
            hasLastFacePos = false;
            return;
        }

        // ✅ First frame → establish baseline (NO jump)
        if (!hasLastFacePos)
        {
            lastFaceLocalPos = faceLocal;
            hasLastFacePos = true;
            return;
        }

        // ✅ Delta-based movement (NO offset when face changes)
        Vector3 delta = faceLocal - lastFaceLocalPos;
        lastFaceLocalPos = faceLocal;

        currentOffset.x += delta.x * sensitivity;
        currentOffset.y += delta.y * sensitivity;

        currentOffset.x = Mathf.Clamp(currentOffset.x, -maxOffset, maxOffset);
        currentOffset.y = Mathf.Clamp(currentOffset.y, -maxOffset, maxOffset);
        currentOffset.z = 0;

        // ✅ Move the object (MoveTarget)
        objectRoot.localPosition = Vector3.SmoothDamp(
            objectRoot.localPosition,
            currentOffset,
            ref velocity,
            smoothTime
        );

        // ===============================
        // 🎥 CAMERA CALCULATION (FINAL)
        // ===============================

        if (cameraTransform != null && cameraBase != null)
        {
            // Camera moves ONLY based on objectRoot
            cameraTransform.position =
                cameraBase.position + objectRoot.localPosition;

            // Camera always looks at LookTarget
            if (lookTarget != null)
            {
                cameraTransform.LookAt(lookTarget.position);
            }
        }
    }
}
