using UnityEngine;

// CameraManager: handles camera modes and toggling.
public class CameraManager : MonoBehaviour
{
    public Transform[] cameraPoints; // assign transforms representing camera positions relative to the vehicle
    public Camera mainCamera;
    public Transform followTarget; // usually the car root

    int currentIndex = 0;

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        ApplyCameraPoint(currentIndex);
    }

    void Update()
    {
        var ia = InputAdapter.Instance;
        if (ia != null && ia.CameraToggle)
        {
            CycleCamera();
        }

        // If we have a follow target and camera point is relative, update dynamically
        if (mainCamera != null && followTarget != null && cameraPoints != null && cameraPoints.Length > 0)
        {
            Transform pt = cameraPoints[currentIndex];
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, pt.position, 6f * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, pt.rotation, 8f * Time.deltaTime);
        }
    }

    public void CycleCamera()
    {
        if (cameraPoints == null || cameraPoints.Length == 0) return;
        currentIndex = (currentIndex + 1) % cameraPoints.Length;
        ApplyCameraPoint(currentIndex);
    }

    void ApplyCameraPoint(int idx)
    {
        if (cameraPoints == null || cameraPoints.Length == 0) return;
        Transform pt = cameraPoints[idx];
        if (pt == null) return;

        if (mainCamera != null)
        {
            mainCamera.transform.position = pt.position;
            mainCamera.transform.rotation = pt.rotation;
        }
    }
}
