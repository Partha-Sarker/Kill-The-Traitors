using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TPSCamController : MonoBehaviour
{
    [SerializeField] private Transform CameraTarget;
    [SerializeField] private Transform player;
    [SerializeField] private Transform orbit;
    [SerializeField] private GunAuto gun;
    [SerializeField] private GameObject gunObject;
    [SerializeField] private RigBuilder rigBuilder;

    [SerializeField] private int mouseXSpeedMod = 5;
    [SerializeField] private int mouseYSpeedMod = 5;
    [SerializeField] private float distance = 3f;
    [SerializeField] private float fovSwitchTime = .2f;
    [SerializeField] private float aimSwitchTime = .2f;

    [SerializeField] private Animator animator;

    private enum WeaponType { gun, grenade };
    private WeaponType currentWeapon = WeaponType.gun;

    private CinemachineVirtualCamera tpsCam;
    private Vector3 position;
    private Quaternion rotation;

    [SerializeField] private Rig rightArmRig;
    [SerializeField] private Rig headRig;

    private float x = 0.0f;
    private float y = 0.0f;
    //private bool isShooting = false, isAiming = false;
    private bool isAiming = false;

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
        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (currentWeapon == WeaponType.gun)
            {
                currentWeapon = WeaponType.grenade;
                animator.SetTrigger("switchWeapon");
                animator.SetBool("isStrafing", true);
                gunObject.SetActive(false);
                rigBuilder.layers[0].active = false;
                rigBuilder.layers[1].active = false;
                rigBuilder.layers[2].active = false;
            }
            else
            {
                currentWeapon = WeaponType.gun;
                animator.SetTrigger("switchWeapon");
                animator.SetBool("isStrafing", false);
                gunObject.SetActive(true);
                rigBuilder.layers[0].active = true;
                rigBuilder.layers[1].active = true;
                rigBuilder.layers[2].active = true;
            }
        }

        if(currentWeapon == WeaponType.grenade)
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("throwing");
            }
            else if (Input.GetMouseButtonUp(0))
            {
                animator.SetTrigger("throw");
            }
        }

        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                isAiming = gun.StartFiring();
            }
            else if (Input.GetMouseButton(0))
            {
                gun.Shoot();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isAiming = gun.StopFiring();
            }

            if (Input.GetMouseButtonDown(1))
            {
                isAiming = gun.StartAiming();
            }

            else if (Input.GetMouseButtonUp(1))
            {
                isAiming = gun.StopAiming();
            }
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

        //if (isShooting || isAiming)
        //{
        //    player.rotation = Quaternion.Euler(player.eulerAngles.x, x, player.eulerAngles.z);
        //    orbit.localRotation = Quaternion.Euler(y, 0, 0);
        //}

        if (isAiming)
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
