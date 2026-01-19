using UnityEngine;

public class EditorFaceSimulator : MonoBehaviour
{
    public float range = 0.3f;
    public float speed = 2f;

    void Update()
    {
        float x = (Input.mousePosition.x / Screen.width - 0.5f) * 2f;
        float y = (Input.mousePosition.y / Screen.height - 0.5f) * 2f;

        transform.localPosition = new Vector3(
            x * range,
            y * range,
            0
        );
    }
}
