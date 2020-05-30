using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GunAndGrenade : WeaponBehaviour
{
    public enum GunMode { Auto, Single };
    public GunMode mode = GunMode.Auto;

    [SerializeField] private Transform player;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject hitWallImpact, hitBloodImpact;

    [SerializeField] private float shootMouseSensitivity = 4, aimMouseSensitivity = 3;
    [SerializeField] private float fovSwitchTime = .2f, aimFOV = 20, camAngle, aimRotaionLerpTime = .1f;
    [SerializeField] private float fireRate, damage, force, fireStartDelay = 0, maxDistance = 10, reloadDelay = .5f;
    [SerializeField] private int maximumAmmo = 30, currentAmmo;


    public LayerMask shootingMask;

    private bool isShooting = false, isAiming = false, isReloading;

    private float nextTimeToFire = 0;

    private void Start()
    {
        currentAmmo = maximumAmmo;
    }

    private void LateUpdate()
    {
        if (activeStatus == false)
            return;

        Quaternion camRotation = mainCam.transform.rotation;
        player.rotation = Quaternion.Euler(player.eulerAngles.x, camRotation.eulerAngles.y, player.eulerAngles.z);

        camAngle = camRotation.eulerAngles.x;
        spine.Rotate(player.transform.right, camAngle, Space.World);


    }

    public override void Equip()
    {
        this.gameObject.SetActive(true);
        //animator.SetTrigger("switchToRifle");
        nextTimeToFire = Time.time;
        StartCoroutine(ChangeFOVandSentivitySmoothly(tpsCam.m_Lens.FieldOfView, defaultFOV));
        activeStatus = true;
        if (currentAmmo == 0)
            StartReloading();
        ResetGun();
    }

    public override void Discard()
    {
        this.gameObject.SetActive(false);
        activeStatus = false;
        ResetGun();
    }

    private void ResetGun()
    {
        isShooting = false;
        isAiming = false;
        //animator.SetBool("isAiming", false);
        //animator.SetBool("isStrafing", false);
    }

    public override void OnLeftClickDown()
    {
        StartShooting();
    }

    public override void OnLeftClickHold()
    {
        if (mode == GunMode.Single)
            return;
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


    private void StartShooting()
    {
        if (Time.time < nextTimeToFire)
            return;
        isShooting = true;
        nextTimeToFire = Time.time + fireStartDelay;
        if (!isAiming)
        {
            StopAllCoroutines();
            //animator.SetBool("isAiming", true);
            //animator.SetBool("isStrafing", true);
        }
        Shoot();
    }
    private void Shoot()
    {
        if (Time.time < nextTimeToFire || isReloading || currentAmmo <= 0)
            return;

        animator.SetTrigger("shoot");
        muzzleFlash.Play();
        nextTimeToFire = Time.time + 1 / fireRate;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out RaycastHit hit, maxDistance, shootingMask))
        {
            GameObject tempHitImpact;
            if (hit.transform.CompareTag("Enemy"))
            {
                tempHitImpact = Instantiate(hitBloodImpact, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                tempHitImpact = Instantiate(hitWallImpact, hit.point, Quaternion.LookRotation(hit.normal));
            }
            Destroy(tempHitImpact, 3);
        }
        currentAmmo--;
        if (currentAmmo == 0)
        {
            Invoke("StartReloading", reloadDelay);
        }
    }

    private void StopShooting()
    {
        isShooting = false;
        StopAllCoroutines();
        if (!isAiming)
        {
            isShooting = false;
            //animator.SetBool("isAiming", isShooting = false);
            //animator.SetBool("isStrafing", false);
            StartCoroutine(ChangeFOVandSentivitySmoothly(tpsCam.m_Lens.FieldOfView, defaultFOV));
        }
    }

    private void StartAiming()
    {
        isAiming = true;
        StopAllCoroutines();
        if (!isShooting)
        {
            //animator.SetBool("isAiming", true);
            //animator.SetBool("isStrafing", true);
        }
        StartCoroutine(ChangeFOVandSentivitySmoothly(tpsCam.m_Lens.FieldOfView, aimFOV));
    }

    private void StopAiming()
    {
        isAiming = false;
        StopAllCoroutines();
        if (!isShooting)
        {
            isShooting = false;
            //animator.SetBool("isAiming", isShooting = false);
            //animator.SetBool("isStrafing", false);
            StartCoroutine(ChangeFOVandSentivitySmoothly(tpsCam.m_Lens.FieldOfView, defaultFOV));
            return;
        }
    }

    public void StartReloading()
    {
        isReloading = true;
        animator.SetTrigger("reload");
    }

    public void StopReloading()
    {
        isReloading = false;
        currentAmmo = maximumAmmo;
    }

    IEnumerator ChangeFOVandSentivitySmoothly(float fovSource, float fovTarget)
    {
        float startTime = Time.time;
        while (Time.time < startTime + fovSwitchTime)
        {
            tpsCam.m_Lens.FieldOfView = Mathf.Lerp(fovSource, fovTarget, (Time.time - startTime) / fovSwitchTime);
            yield return null;
        }
        tpsCam.m_Lens.FieldOfView = fovTarget;
    }

}
