using UnityEngine;


public class SafeArea : MonoBehaviour
{
    private RectTransform panel;
    private Rect lastSafeArea = new(0, 0, 0, 0);

    void Awake()
    {
        panel = GetComponent<RectTransform>();
        Refresh();
    }

    void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        Rect safeArea = Screen.safeArea;

        if (safeArea != lastSafeArea)
            ApplySafeArea(safeArea);
    }

    private void ApplySafeArea(Rect safeArea)
    {
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        panel.anchorMin = anchorMin;
        panel.anchorMax = anchorMax;

        lastSafeArea = safeArea;

        Debug.Log("Applied Safe Area: " + safeArea.ToString());
    }
}
