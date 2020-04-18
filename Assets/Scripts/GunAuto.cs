using System.Collections;
using DG.Tweening;
using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GunAuto : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CinemachineVirtualCamera tpsCam;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject hitImpact;
    [SerializeField] private Transform orbit;
    [SerializeField] private Rig rightArmRig;
    [SerializeField] private Rig headRig;

    private Camera mainCam;
    private GameObject tempHitImpact;

    [SerializeField] private float fovSwitchTime = .2f;
    [SerializeField] private float rightArmRigSwitchTime = .1f, headRigSwitchTime = .2f;
    [SerializeField] private float fireRate, damage, force, fireStartDelay, maxDistance = 100, pushBackAmount, pushBackTime;

    private bool isShooting = false, isAiming = false;

    private float nextTimeToFire = 10;

    private void Start()
    {
        mainCam = Camera.main;
    }

    public bool StartFiring()
    {
        isShooting = true;
        if (!isAiming)
        {
            StopAllCoroutines();
            nextTimeToFire = Time.time + fireStartDelay;
            animator.SetBool("isAiming", true);
            StartCoroutine(ChangeRightArmRigWeightSmoothly(rightArmRig.weight, 1));
            StartCoroutine(ChangeHeadRigWeightSmoothly(rightArmRig.weight, 1));
            StartCoroutine(ChangeFieldOfViewSmoothly(tpsCam.m_Lens.FieldOfView, 30));
        }
        else
            nextTimeToFire = Time.time;
        return true;
    }

    public bool StopFiring()
    {
        isShooting = false;
        StopAllCoroutines();
        if (!isAiming)
        {
            animator.SetBool("isAiming", isShooting = false);
            StartCoroutine(ChangeRightArmRigWeightSmoothly(rightArmRig.weight, 0));
            StartCoroutine(ChangeHeadRigWeightSmoothly(rightArmRig.weight, 0));
            StartCoroutine(ChangeFieldOfViewSmoothly(tpsCam.m_Lens.FieldOfView, 40));
            return false;
        }
        return true;
    }

    public bool StartAiming()
    {
        isAiming = true;
        StopAllCoroutines();
        if (!isShooting)
        {
            animator.SetBool("isAiming", true);
            StartCoroutine(ChangeRightArmRigWeightSmoothly(rightArmRig.weight, 1));
            StartCoroutine(ChangeHeadRigWeightSmoothly(rightArmRig.weight, 1));
        }
        StartCoroutine(ChangeFieldOfViewSmoothly(tpsCam.m_Lens.FieldOfView, 20));
        return true;
    }

    public bool StopAiming()
    {
        isAiming = false;
        StopAllCoroutines();
        if (!isShooting)
        {
            animator.SetBool("isAiming", isShooting = false);
            StartCoroutine(ChangeRightArmRigWeightSmoothly(rightArmRig.weight, 0));
            StartCoroutine(ChangeHeadRigWeightSmoothly(rightArmRig.weight, 0));
            StartCoroutine(ChangeFieldOfViewSmoothly(tpsCam.m_Lens.FieldOfView, 40));
            return false;
        }
        StartCoroutine(ChangeFieldOfViewSmoothly(tpsCam.m_Lens.FieldOfView, 30));
        return true;
    }

    public void Shoot()
    {
        if (Time.time < nextTimeToFire)
            return;
        DOTween.Sequence().Append(orbit.DOLocalMoveZ(pushBackAmount, pushBackTime)).Append(orbit.DOLocalMoveZ(0, pushBackTime));
        muzzleFlash.Play();
        animator.SetTrigger("shoot");
        nextTimeToFire = Time.time + 1 / fireRate;
        if(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out RaycastHit hit, maxDistance))
        {
            print(hit.transform.name);
            tempHitImpact = Instantiate(hitImpact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(tempHitImpact, 3);
        }

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

    IEnumerator ChangeRightArmRigWeightSmoothly(float source, float target)
    {
        float startTime = Time.time;
        while (Time.time < startTime + rightArmRigSwitchTime)
        {
            rightArmRig.weight = Mathf.Lerp(source, target, (Time.time - startTime) / rightArmRigSwitchTime);
            yield return null;
        }
        rightArmRig.weight = target;
    }

    IEnumerator ChangeHeadRigWeightSmoothly(float source, float target)
    {
        float startTime = Time.time;
        while (Time.time < startTime + headRigSwitchTime)
        {
            headRig.weight = Mathf.Lerp(source, target, (Time.time - startTime) / headRigSwitchTime);
            yield return null;
        }
        headRig.weight = target;
    }
}
