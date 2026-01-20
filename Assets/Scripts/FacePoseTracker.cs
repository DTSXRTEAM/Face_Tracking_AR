using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FacePoseTracker : MonoBehaviour
{
    [Header("Face Input")]
    public ARFaceManager faceManager;

    public Transform CameraRig; // Main Camera

    public Transform ArDefaultFace; 

    public bool editorCheck;
    public float sensitivity = 1f;

    public float smoothTime = 0.12f;

    //private float maxOffset;

    private Vector3 faceLocal;
    private Vector3 velocity;


    // Update is called once per frame
    void Update()
    {
        if (editorCheck)
        {
            faceLocal = ArDefaultFace.transform.localPosition;

            //CameraRig.localPosition = new Vector3(faceLocal.x, faceLocal.y, 0);

            ApplyMovement();
            return;
        }


        if (faceManager == null)
        {
            return;
        }

        foreach (var face in faceManager.trackables)
        {
            faceLocal = face.transform.localPosition;
            break;
        }

        ApplyMovement();

        void ApplyMovement()
        {
            float x = Mathf.Clamp(faceLocal.x * sensitivity, -0.9f, 0.9f);
            float y = Mathf.Clamp(faceLocal.y * sensitivity, -0.8f, 0.8f);

            Vector3 targetPos = new Vector3(x, y, 0f);

            CameraRig.localPosition = Vector3.SmoothDamp(CameraRig.localPosition, targetPos, ref velocity, smoothTime);
        }
    }
}
