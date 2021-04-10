using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    private Camera cam;

    [SerializeField] private float playerRotationSmoothness = 10;
    [SerializeField] private float animationTransitionTime = 1;


    private float hInput, vInput, speedMultiplier = 1;
    private Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speedMultiplier = 2;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speedMultiplier = 1;
        }

        hInput = Input.GetAxisRaw("Horizontal") * speedMultiplier;
        vInput = Input.GetAxisRaw("Vertical") * speedMultiplier;

        animator.SetFloat("vInput", vInput, animationTransitionTime, Time.deltaTime);
        animator.SetFloat("hInput", hInput, animationTransitionTime, Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (vInput != 0 || hInput !=0)
        {
            rotation = cam.transform.rotation;
            rotation.x = transform.rotation.x;
            rotation.z = transform.rotation.z;
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerRotationSmoothness);
        }
    }


    //[SerializeField] private float aimRotationMultiplier = 10;
    //[SerializeField] private float aimRotationSmoothness = 10;
    //[SerializeField] private float fovSwitchTime = .2f;
    //[SerializeField] private float aimSwitchTime = .2f;
    //[SerializeField] private float upperAimLimit = -40;
    //[SerializeField] private float lowerAimLimit = 40;
    //[SerializeField] private float aimSwitchCamOffset = .6f;
    //[SerializeField] private Rig rightArmRig;
    //[SerializeField] private Rig headRig;
    //private Vector3 eulerRotation;
    //[SerializeField] private bool isShooting = false;
    //private float hMouse, vMouse, tpsCamYValue, test1, test2;

    //IEnumerator ChangeRigWeightSmoothly(float source, float target)
    //{
    //    float startTime = Time.time;
    //    while (Time.time < startTime + aimSwitchTime)
    //    {
    //        rightArmRig.weight = Mathf.Lerp(source, target, (Time.time - startTime) / aimSwitchTime);
    //        headRig.weight = rightArmRig.weight;
    //        yield return null;
    //    }
    //    rightArmRig.weight = target;
    //    headRig.weight = target;
    //}

    //IEnumerator ChangeFieldOfViewSmoothly(float source, float target)
    //{
    //    float startTime = Time.time;
    //    while (Time.time < startTime + fovSwitchTime)
    //    {
    //        CMCam.m_Lens.FieldOfView = Mathf.Lerp(source, target, (Time.time - startTime) / fovSwitchTime);
    //        yield return null;
    //    }
    //    CMCam.m_Lens.FieldOfView = target;
    //}

    //IEnumerator ChangeMultiplierSmoothly(float source, float target)
    //{
    //    float startTime = Time.time;
    //    while (Time.time < startTime + sprintSwithTime)
    //    {
    //        multiplier = Mathf.Lerp(source, target, (Time.time - startTime) / sprintSwithTime);
    //        yield return null;
    //    }
    //    multiplier = target;
    //}


    //if (Input.GetMouseButtonDown(0))
    //{
    //    rotation = cam.transform.localRotation;
    //    rotation.x = transform.localRotation.x;
    //    rotation.z = transform.localRotation.z;
    //    transform.rotation = rotation;


    //    eulerRotation = orbit.localRotation.eulerAngles;
    //    eulerRotation.x = cam.transform.localRotation.eulerAngles.x;
    //    orbit.transform.localEulerAngles = eulerRotation;


    //    animator.SetBool("isAiming", isShooting = true);
    //    StartCoroutine(ChangeRigWeightSmoothly(rightArmRig.weight, 1));
    //}

    //else if (Input.GetMouseButtonUp(0))
    //{
    //    tpsCamYValue = orbit.localEulerAngles.x;
    //    if (tpsCamYValue > 180)
    //        tpsCamYValue -= 360;


    //    test1 = Mathf.Abs(39.133f) + Mathf.Abs(-37.655f);
    //    test2 = tpsCamYValue + Mathf.Abs(39.133f);
    //    tpsCamYValue = Mathf.Clamp(test2, 0, test1);
    //    tpsCamYValue /= test1;
    //    print(test1 + " " + test2 + " " + tpsCamYValue);
    //    //print(tpsCamYValue);

    //    tpsCam.m_XAxis.Value = transform.localRotation.eulerAngles.y;
    //    tpsCam.m_YAxis.Value = tpsCamYValue;

    //    animator.SetBool("isAiming", isShooting = false);
    //    StartCoroutine(ChangeRigWeightSmoothly(rightArmRig.weight, 0));
    //}

    //if (isShooting)
    //{
    //    transform.Rotate(Vector3.up, hMouse * aimRotationMultiplier);

    //    eulerRotation = orbit.localEulerAngles;
    //    if (eulerRotation.x > 180)
    //        eulerRotation.x -= 360;

    //    if (eulerRotation.x < upperAimLimit && vMouse > 0)
    //        return;
    //    if (eulerRotation.x > lowerAimLimit && vMouse < 0)
    //        return;

    //    orbit.Rotate(-Vector3.right, vMouse * aimRotationMultiplier);
    //    return;
    //}

    //hMouse = Mathf.Lerp(hMouse, Input.GetAxisRaw("Mouse X"), aimRotationSmoothness * Time.deltaTime);
    //vMouse = Mathf.Lerp(vMouse, Input.GetAxisRaw("Mouse Y"), aimRotationSmoothness * Time.deltaTime);
}
