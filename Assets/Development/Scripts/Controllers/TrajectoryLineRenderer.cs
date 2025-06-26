using UnityEngine;

[ExecuteInEditMode]
public class TrajectoryLineRenderer : MonoBehaviour
{
    private Transform startPoint;
    private Transform endPoint;

    private LineRenderer lineRenderer;

    private Vector3 previousStartPoint;
    private Vector3 previousEndPoint;

    private void Awake()
    {
        startPoint = transform.Find("Spawn Point");
        endPoint = transform.Find("Intercept Point");

        if (startPoint == null || endPoint == null)
        {
            Debug.LogError("StartPoint or EndPoint child object not found. Please ensure they exist under this GameObject.");
            enabled = false; // Stop executing this script because a required child object was not found
            return;
        }

        if (!TryGetComponent(out lineRenderer))
        {
            Debug.LogError("LineRenderer component not found on this GameObject.");
            enabled = false; // Stop executing this script because the LineRenderer is missing
            return;
        }

        UpdateLinePositions();
    }

    private void Update()
    {
        // Only update LineRenderer when positions change
        if (startPoint.position != previousStartPoint || endPoint.position != previousEndPoint)
        {
            UpdateLinePositions();
        }
    }

    private void UpdateLinePositions()
    {
        // Update position data
        previousStartPoint = startPoint.position;
        previousEndPoint = endPoint.position;

        // Set LineRenderer positions
        lineRenderer.SetPosition(0, previousStartPoint);
        lineRenderer.SetPosition(1, previousEndPoint);
    }

    private void OnValidate()
    {
        // Find child objects in edit mode for updates
        if (startPoint == null || endPoint == null)
        {
            startPoint = transform.Find("StartPoint");
            endPoint = transform.Find("EndPoint");
        }

        if (!TryGetComponent(out lineRenderer))
        {
            Debug.LogError("LineRenderer component not found on this GameObject.");
            return;
        }

        if (startPoint != null && endPoint != null)
        {
            UpdateLinePositions();
        }
    }
}
