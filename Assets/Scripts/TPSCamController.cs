using UnityEngine;

public class TPSCamController : MonoBehaviour
{
    [SerializeField] private WeaponManager weaponManager;

    [SerializeField] private Transform CameraTarget;

    [SerializeField] private float initialMouseSensitivity = 5, currentMouseSensitivity;

    [SerializeField] private int camClampAngle = 40;
    [SerializeField] private float distance = 3f;

    private Vector3 position;
    private Quaternion rotation;

    private float x = 0.0f;
    private float y = 0.0f;

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
        x += Input.GetAxis("Mouse X") * currentMouseSensitivity;
        y += Input.GetAxis("Mouse Y") * currentMouseSensitivity * -1;
        y = ClampAngle(y, -camClampAngle, camClampAngle);

        rotation = Quaternion.Euler(y, x, 0);
        position = CameraTarget.position - (rotation * Vector3.forward * distance);

        //if (Physics.Linecast(CameraTarget.position, position, out RaycastHit collisionHit))
        //{
        //    position = collisionHit.point;
        //}

        transform.rotation = rotation;
        transform.position = position;

        if (weaponManager.currentWeapon != null)
            weaponManager.currentWeapon.OnCamPosRotChanged(position, rotation);
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
