using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GunAndGrenade : WeaponBehaviour
{
    public enum GunMode { Auto, Single };
    public GunMode mode = GunMode.Auto;

    [SerializeField] private Transform player, grenadePos;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject hitWallImpact, hitBloodImpact, grenade, tempGrenade;

    [SerializeField] private float throwingForce = 5;
    [SerializeField] private Vector3 throwingOffset;

    [SerializeField] private float fovSwitchTime = .2f, aimFOV = 20, camAngle, coolDownTime = 2, lastActiveTime;
    [SerializeField] private float fireRate, fireStartDelay = 0, maxDistance = 10;
    [SerializeField] private int maximumAmmo = 30, currentAmmo;


    public LayerMask shootingMask;

    private bool isShooting = false, isAiming = false, isRifleDown = false, isReloading;

    private float nextTimeToFire = 0;

    private void Start()
    {
        currentAmmo = maximumAmmo;
    }

    private void LateUpdate()
    {
        if (activeStatus == false)
            return;

        if (Input.GetKeyDown(KeyCode.R))
            StartReloading();

        if(!isAiming && !isShooting)
        {
            if (!isRifleDown && (Time.time - lastActiveTime) > coolDownTime)
            {
                isRifleDown = true;
                animator.SetBool("rifleDown", true);
            }
        }
        else
        {
            if (isRifleDown)
            {
                isRifleDown = false;
                animator.SetBool("rifleDown", false);
            }
            lastActiveTime = Time.time;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && !isAiming && !isShooting && !isRifleDown)
        {
            isRifleDown = true;
            animator.SetBool("rifleDown", true);
        }

        if (!isRifleDown)
        {
            Quaternion camRotation = mainCam.transform.rotation;
            player.rotation = Quaternion.Euler(player.eulerAngles.x, camRotation.eulerAngles.y, player.eulerAngles.z);
            camAngle = camRotation.eulerAngles.x;
            spine.Rotate(player.transform.right, camAngle, Space.World);
        }


    }

    public override void Equip()
    {
        //animator.SetTrigger("switchToRifle");
        this.gameObject.SetActive(true);
        nextTimeToFire = Time.time;
        lastActiveTime = Time.time;
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
        isRifleDown = false;
        //animator.SetBool("isAiming", false);
        //animator.SetBool("isStrafing", false);
    }

    public override void OnLeftKeyDown()
    {
        StartShooting();
    }

    public override void OnLeftKeykHold()
    {
        if (mode == GunMode.Single)
            return;
        if (!activeStatus)
            return;
        Shoot();
    }

    public override void OnLeftKeyUp()
    {
        if (!activeStatus)
            return;
        StopShooting();
    }

    public override void OnRightKeyDown()
    {
        activeStatus = true;
        StartAiming();
    }

    public override void OnRightKeyHold()
    {
        return;
    }

    public override void OnRightKeyUp()
    {
        if (!activeStatus)
            return;
        StopAiming();
    }

    public override void OnMiddleKeyDown()
    {
        StartThrowingGrenade();        
    }
    public override void OnMiddleKeyUp()
    {
        return;
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
                hit.transform.GetComponent<EnemyController>().SetAware(playerTarget);
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
            StartReloading();
        }
    }

    private void StopShooting()
    {
        isShooting = false;
        StopAllCoroutines();
        if (!isAiming)
        {
            isShooting = false;
            StartCoroutine(ChangeFOVandSentivitySmoothly(tpsCam.m_Lens.FieldOfView, defaultFOV));
        }
    }

    private void StartAiming()
    {
        isAiming = true;
        StopAllCoroutines();
        StartCoroutine(ChangeFOVandSentivitySmoothly(tpsCam.m_Lens.FieldOfView, aimFOV));
    }

    private void StopAiming()
    {
        isAiming = false;
        StopAllCoroutines();
        if (!isShooting)
        {
            isShooting = false;
            StartCoroutine(ChangeFOVandSentivitySmoothly(tpsCam.m_Lens.FieldOfView, defaultFOV));
            return;
        }
    }

    private void StartThrowingGrenade()
    {
        animator.SetTrigger("throw");
        tempGrenade = Instantiate(grenade, grenadePos);
    }

    public void ThrowGrenade()
    {
        tempGrenade.GetComponent<Grenade>().StartThrowingPreparation();
        tempGrenade.GetComponent<Rigidbody>().AddForce(mainCam.transform.forward * throwingForce + throwingOffset, ForceMode.VelocityChange);
    }

    public void StartReloading()
    {
        if (currentAmmo == maximumAmmo)
            return;

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
