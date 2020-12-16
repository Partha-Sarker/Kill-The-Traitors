using UnityEngine;

public class TPSCamController : MonoBehaviour
{
    [SerializeField] private Transform CameraTarget;

    [SerializeField] private float initialMouseSensitivity = 5;
    private float currentMouseSensitivity;

    [SerializeField] private int camClampAngle = 40;
    //[SerializeField] private float targetDistance = 1f, targetHeight = 1f, targetOffset = 1f;
    [SerializeField] private Vector3 targetOffset = new Vector3(1, 1.6f, 1);

    [SerializeField] private LayerMask camCollision;

    [SerializeField] private Vector3 camPosition, camCenterPosition, targetPosition, targetCenterPosition;
    private Quaternion rotation;

    public float x = 0.0f, y = 0.0f;

    // Use this for initialization
    void Start()
    {
        ResetMouseSensitivity();
        Vector3 Angles = transform.eulerAngles;
        x = Angles.x;
        y = Angles.y;
    }

    void LateUpdate()
    {
        SetDesiredCamPosition();

        transform.position = camPosition;
    }

    private void SetDesiredCamPosition()
    {
        targetPosition = CameraTarget.position;
        targetPosition.y += targetOffset.y;

        x += Input.GetAxis("Mouse X") * currentMouseSensitivity;
        y += Input.GetAxis("Mouse Y") * currentMouseSensitivity * -1;
        y = ClampAngle(y, -camClampAngle, camClampAngle);

        if (Mathf.Abs(x) > 360)
            x %= 360;

        rotation = Quaternion.Euler(y, x, 0);

        camCenterPosition = CameraTarget.position - (rotation * Vector3.forward * targetOffset.z);
        camCenterPosition.y += targetOffset.y;

        transform.rotation = rotation;
        transform.position = camCenterPosition;

        camPosition = camCenterPosition + (transform.right * targetOffset.x);

        transform.position = camPosition;

        targetCenterPosition = camPosition + (rotation * Vector3.forward * targetOffset.z);

        Debug.DrawLine(targetPosition, camPosition);
        //Debug.DrawLine(targetCenterPosition, camPosition);
        //Debug.DrawLine(camCenterPosition, targetPosition);
    }

    private void HandleCamCollision()
    {
        if (Physics.Linecast(targetPosition, camPosition, out RaycastHit hit, camCollision))
        {
            camPosition = hit.point;
        }
        //else if (Physics.Linecast(targetCenterPosition, camPosition, out RaycastHit targetCenterHit, camCollision))
        //{
        //    camPosition = targetCenterHit.point;
        //}
        //if (Physics.Linecast(targetPosition, camCenterPosition, out RaycastHit camCenterHit, camCollision))
        //{
        //    camPosition += camCenterHit.point + (transform.right * offset.x);
        //}
    }

    public float GetCurrentMouseSensitivity()
    {
        return currentMouseSensitivity;
    }

    public float GetDefaultMouseSensitivity()
    {
        return initialMouseSensitivity;
    }

    public void ResetMouseSensitivity()
    {
        currentMouseSensitivity = initialMouseSensitivity;
    }

    public void SetMouseSensitivity(float mouseSensitivity)
    {
        currentMouseSensitivity = mouseSensitivity;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }
}
