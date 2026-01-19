using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    public Transform CameraRig; // Main Camera
    public Transform lookTarget;      // Camera look object

    void Update()
    {
        if (CameraRig != null && lookTarget != null)
        {
            // Camera always looks at LookTarget
                CameraRig.LookAt(lookTarget.position);
        }
    }
}
