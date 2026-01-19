using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SmoothParallaxCamera : MonoBehaviour
{
    public ARFaceManager faceManager;

    [Header("Movement Settings")]
    public float sensitivity = 0.15f;
    public float smoothTime = 0.2f;
    public float maxOffset = 0.25f;

    Vector3 velocity;
    Vector3 targetPos;

    void LateUpdate()
    {
        if (faceManager.trackables.count == 0) return;

        foreach (var face in faceManager.trackables)
        {
            Vector3 facePos = face.transform.localPosition;

            targetPos = new Vector3(
                Mathf.Clamp(-facePos.x * sensitivity, -maxOffset, maxOffset),
                Mathf.Clamp(-facePos.y * sensitivity, -maxOffset, maxOffset),
                0
            );
            break;
        }

        transform.localPosition = Vector3.SmoothDamp(
            transform.localPosition,
            targetPos,
            ref velocity,
            smoothTime
        );
    }
}
