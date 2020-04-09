using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TPSCamController : MonoBehaviour
{
    [SerializeField] private Transform CameraTarget;
    [SerializeField] private Transform player;
    [SerializeField] private Transform orbit;

    [SerializeField] private int mouseXSpeedMod = 5;
    [SerializeField] private int mouseYSpeedMod = 5;
    [SerializeField] private float distance = 3f;
    [SerializeField] private float fovSwitchTime = .2f;
    [SerializeField] private float aimSwitchTime = .2f;

    [SerializeField] private Animator animator;

    private CinemachineVirtualCamera tpsCam;
    private Vector3 position;
    private Quaternion rotation;

    [SerializeField] private Rig rightArmRig;
    [SerializeField] private Rig headRig;

    private float x = 0.0f;
    private float y = 0.0f;
    private bool isShooting = false, isAiming = false;

    // Use this for initialization
    void Start()
    {
        tpsCam = GetComponent<CinemachineVirtualCamera>();
        Vector3 Angles = transform.eulerAngles;
        x = Angles.x;
        y = Angles.y;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isShooting = true;
            if (!isAiming)
            {
                animator.SetBool("isAiming", true);
                StartCoroutine(ChangeRigWeightSmoothly(rightArmRig.weight, 1));
                //rightArmRig.weight = 1;
                //headRig.weight = 1;
            }
        }

        else if (Input.GetMouseButtonUp(0))
        {
            isShooting = false;
            if (!isAiming)
            {
                animator.SetBool("isAiming", isShooting = false);
                StartCoroutine(ChangeRigWeightSmoothly(rightArmRig.weight, 0));
                //rightArmRig.weight = 0;
                //headRig.weight = 0;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            isAiming = true;
            if (!isShooting)
            {
                animator.SetBool("isAiming", true);
                StartCoroutine(ChangeRigWeightSmoothly(rightArmRig.weight, 1));
                //rightArmRig.weight = 1;
                //headRig.weight = 1;
            }
            StartCoroutine(ChangeFieldOfViewSmoothly(tpsCam.m_Lens.FieldOfView, 20));
        }

        else if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            if (!isShooting)
            {
                animator.SetBool("isAiming", isShooting = false);
                StartCoroutine(ChangeRigWeightSmoothly(rightArmRig.weight, 0));
                //rightArmRig.weight = 0;
                //headRig.weight = 0;
            }
            StartCoroutine(ChangeFieldOfViewSmoothly(tpsCam.m_Lens.FieldOfView, 40));
        }

    }

    void LateUpdate()
    {
        x += Input.GetAxis("Mouse X") * mouseXSpeedMod;
        y += Input.GetAxis("Mouse Y") * mouseYSpeedMod * -1;
        y = ClampAngle(y, -30, 30);

        rotation = Quaternion.Euler(y, x, 0);
        position = CameraTarget.position - (rotation * Vector3.forward * distance);

        transform.rotation = rotation;
        transform.position = position;
        if (isShooting || isAiming)
        {
            player.rotation = Quaternion.Euler(player.eulerAngles.x, x, player.eulerAngles.z);
            orbit.localRotation = Quaternion.Euler(y, 0, 0);
        }
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

    IEnumerator ChangeFieldOfViewSmoothly(float source, float target)
    {
        float startTime = Time.time;
        while (Time.time < startTime + fovSwitchTime)
        {
            tpsCam.m_Lens.FieldOfView = Mathf.Lerp(source, target, (Time.time - startTime) / fovSwitchTime);
            yield return null;
        }
        tpsCam.m_Lens.FieldOfView = target;
    }

    IEnumerator ChangeRigWeightSmoothly(float source, float target)
    {
        float startTime = Time.time;
        while (Time.time < startTime + aimSwitchTime)
        {
            rightArmRig.weight = Mathf.Lerp(source, target, (Time.time - startTime) / aimSwitchTime);
            headRig.weight = rightArmRig.weight;
            yield return null;
        }
        rightArmRig.weight = target;
        headRig.weight = target;
    }
}
