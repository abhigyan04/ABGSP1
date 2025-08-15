using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector3 offset;
    private Camera mainCamera;
    private GameObject[] snapTargets;
    [SerializeField] private GameObject[] targetNodes;
    private readonly float snapRange = 0.5f;
    private Vector3 originalPos;

    void Start()
    {
        mainCamera = Camera.main; // Cache the main camera
        snapTargets = GameObject.FindGameObjectsWithTag("TargetNode");
        targetNodes = snapTargets.OrderBy(x => x.transform.position.x).ToArray();
    }


    void OnMouseDown()
    {
        originalPos = transform.position;
        // Calculate the offset between the mouse position and the GameObject's position
        offset = transform.position - GetMouseWorldPos();
        //isDragging = true;
    }


    void OnMouseDrag()
    {
        // Update the position of the GameObject as the mouse is dragged
        transform.position = GetMouseWorldPos() + offset;
    }


    private void OnMouseUp()
    {
        //isDragging = false;
        GameObject closestTarget = FindClosestSnapTarget();
        if(closestTarget != null)
        {
            transform.position = closestTarget.transform.position;
        }
        else
        {
            transform.position = originalPos;
        }
    }


    private GameObject FindClosestSnapTarget()
    {
        GameObject closest = null;
        float minDistance = snapRange;
        foreach (GameObject target in targetNodes)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < minDistance)
            {
                closest = target;
                minDistance = distance;
            }
        }
        return closest;
    }


    private Vector3 GetMouseWorldPos()
    {
        // Convert the mouse position to world coordinates
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mainCamera.WorldToScreenPoint(gameObject.transform.position).z;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }
}
