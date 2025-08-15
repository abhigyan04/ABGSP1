using UnityEngine;

public class CameraAdjuster : MonoBehaviour
{
    public float targetAspect = 9f / 16f; // Target aspect ratio (e.g., 16:9 portrait)

    void Start()
    {
        Camera camera = GetComponent<Camera>();
        float currentAspect = (float)Screen.width / Screen.height;
        camera.orthographicSize *= (targetAspect / currentAspect);
    }
}
