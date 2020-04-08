using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera tpsCam;
    private Transform target;
    [SerializeField] private Transform player;
    [SerializeField] private Transform orbit;
    [SerializeField] private float rotationMultiplier = 10;
    private float hMouse, vMouse;
    private bool isShooting = false, isAiming = false;
    private Vector3 testEulerAngle1;
    private float testFloat1;

    // Start is called before the first frame update
    void Start()
    {
        tpsCam = GetComponent<CinemachineVirtualCamera>();
        target = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        hMouse = Input.GetAxis("Mouse X");
        vMouse = Input.GetAxis("Mouse Y");
        if (Input.GetMouseButtonDown(0))
        {
            isShooting = true;
            testFloat1 = transform.eulerAngles.y - player.eulerAngles.y;
            transform.RotateAround(target.position, Vector3.up, -testFloat1);
            player.Rotate(Vector3.up, testFloat1);


            //testEulerAngle1 = orbit.localEulerAngles;
            //testEulerAngle1.x = transform.localEulerAngles.x;
            //orbit.localEulerAngles = testEulerAngle1;
            //transform.parent = orbit;
            //transform.localEulerAngles = Vector3.zero;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isShooting = false;
            //transform.parent = target;
        }
        if (isShooting)
        {

            //player.Rotate(Vector3.up, hMouse * rotationMultiplier);
            return;
        }
        transform.RotateAround(target.position, Vector3.up, hMouse * rotationMultiplier);
        transform.RotateAround(target.position, -Vector3.right, vMouse * rotationMultiplier);
        
    }
}
