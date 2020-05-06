using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Gun : IWeapon
{
    public enum GunMode { Auto, Single };
    public GunMode mode = GunMode.Auto;

    [SerializeField] private Transform player;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject hitImpact;
    [SerializeField] private Transform orbit;
    [SerializeField] private Rig rightArmRig;
    [SerializeField] private Rig headRig;


    private GameObject tempHitImpact;

    [SerializeField] private float fovSwitchTime = .2f;
    [SerializeField] private float rightArmRigSwitchTime = .1f, headRigSwitchTime = .2f;
    [SerializeField] private float fireRate, damage, force, fireStartDelay = 0, maxDistance = 100, pushBackAmount, pushBackTime;

    private bool isShooting = false, isAiming = false;

    private float nextTimeToFire = 0;

    public override void Equip()
    {
        this.gameObject.SetActive(true);
        rigBuilder.enabled = true;
        animator.SetTrigger("switchToRifle");
        //rigBuilder.layers[0].active = true;
        //rigBuilder.layers[1].active = true;
        //rigBuilder.layers[2].active = true;
        ResetGun();
    }

    public override void Discard()
    {
        this.gameObject.SetActive(false);
        rigBuilder.enabled = false;
        //rigBuilder.layers[0].active = false;
        //rigBuilder.layers[1].active = false;
        //rigBuilder.layers[2].active = false;
        ResetGun();
    }

    private void ResetGun()
    {
        activeStatus = false;
        isShooting = false;
        isAiming = false;
        animator.SetBool("isStrafing", false);
        tpsCam.m_Lens.FieldOfView = 40;
    }

    public override void OnLeftClickDown()
    {
        activeStatus = true;
        StartShooting();
    }

    public override void OnLeftClickHold()
    {
        if (!activeStatus)
            return;
        Shoot();
    }

    public override void OnLeftClickUp()
    {
        if (!activeStatus)
            return;
        StopShooting();
    }

    public override void OnRightClickDown()
    {
        activeStatus = true;
        StartAiming();
    }

    public override void OnRightClickHold()
    {
        return;
    }

    public override void OnRightClickUp()
    {
        if (!activeStatus)
            return;
        StopAiming();
    }

    public override void OnCamPosRotChanged(Vector3 position, Quaternion rotation)
    {
        if(isShooting || isAiming)
        {
            player.rotation = Quaternion.Euler(player.eulerAngles.x, rotation.eulerAngles.y, player.eulerAngles.z);
            orbit.localRotation = Quaternion.Euler(rotation.eulerAngles.x, 0, 0);
        }
    }


    private void StartShooting()
    {
        isShooting = true;
        nextTimeToFire = Time.time + fireStartDelay;
        if (!isAiming)
        {
            StopAllCoroutines();
            animator.SetBool("isAiming", true);
            animator.SetBool("isStrafing", true);
            StartCoroutine(ChangeRightArmRigWeightSmoothly(rightArmRig.weight, 1));
            StartCoroutine(ChangeHeadRigWeightSmoothly(rightArmRig.weight, 1));
            StartCoroutine(ChangeFieldOfViewSmoothly(tpsCam.m_Lens.FieldOfView, 30));
        }
        Shoot();
    }
    private void Shoot()
    {
        if (Time.time < nextTimeToFire)
            return;
        DOTween.Sequence().Append(orbit.DOLocalMoveZ(pushBackAmount, pushBackTime)).Append(orbit.DOLocalMoveZ(0, pushBackTime));
        //DOTween.Sequence().Append(orbit.DOLocalMove(orbitPos + orbit.forward * pushBackAmount, pushBackTime)).Append(orbit.DOLocalMove(orbitPos, pushBackTime));

        muzzleFlash.Play();
        nextTimeToFire = Time.time + 1 / fireRate;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out RaycastHit hit, maxDistance))
        {
            //print(hit.transform.name);
            tempHitImpact = Instantiate(hitImpact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(tempHitImpact, 3);
        }
    }

    private void StopShooting()
    {
        isShooting = false;
        StopAllCoroutines();
        if (!isAiming)
        {
            animator.SetBool("isAiming", isShooting = false);
            animator.SetBool("isStrafing", false);
            StartCoroutine(ChangeRightArmRigWeightSmoothly(rightArmRig.weight, 0));
            StartCoroutine(ChangeHeadRigWeightSmoothly(rightArmRig.weight, 0));
            StartCoroutine(ChangeFieldOfViewSmoothly(tpsCam.m_Lens.FieldOfView, 40));
        }
    }

    private void StartAiming()
    {
        isAiming = true;
        StopAllCoroutines();
        if (!isShooting)
        {
            animator.SetBool("isAiming", true);
            animator.SetBool("isStrafing", true);
            StartCoroutine(ChangeRightArmRigWeightSmoothly(rightArmRig.weight, 1));
            StartCoroutine(ChangeHeadRigWeightSmoothly(rightArmRig.weight, 1));
        }
        StartCoroutine(ChangeFieldOfViewSmoothly(tpsCam.m_Lens.FieldOfView, 20));
    }

    private void StopAiming()
    {
        isAiming = false;
        StopAllCoroutines();
        if (!isShooting)
        {
            animator.SetBool("isAiming", isShooting = false);
            animator.SetBool("isStrafing", false);
            StartCoroutine(ChangeRightArmRigWeightSmoothly(rightArmRig.weight, 0));
            StartCoroutine(ChangeHeadRigWeightSmoothly(rightArmRig.weight, 0));
            StartCoroutine(ChangeFieldOfViewSmoothly(tpsCam.m_Lens.FieldOfView, 40));
            return;
        }
        StartCoroutine(ChangeFieldOfViewSmoothly(tpsCam.m_Lens.FieldOfView, 30));
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
