using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    public bool isRifleUp = false, isAiming = false, isShooting = false, shouldShoot = false, isTargetVisible = false;

    public float aimAngle = 0, rotationDelay = .5f;

    public Transform target, fireOrigin;
    private Transform spine, rightShoulder;

    public EnemyGun gun;

    public enum EnemyState { idle, alert, aware };
    public EnemyState currentState = EnemyState.idle;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spine = animator.GetBoneTransform(HumanBodyBones.Spine);
        rightShoulder = animator.GetBoneTransform(HumanBodyBones.RightShoulder);
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.I))
            SetIdle();
        if (Input.GetKeyDown(KeyCode.C))
            SetAlert(target);
        if (Input.GetKeyDown(KeyCode.K))
            SetAware(target);

        if (shouldShoot)
            Aim();
    }


    public void SetIdle()
    {
        if (currentState == EnemyState.idle)
            return;

        ResetEnemy();
    }

    public void SetAlert(Transform target)
    {
        if (currentState == EnemyState.alert)
            return;
        ResetEnemy();

        currentState = EnemyState.alert;
        isAiming = true;
        if(target != null)
        {
            this.target = target;
            RotateTowardsTransform(target);
        }
        animator.SetBool("isAlert", true);
    }

    public void SetAware(Transform target)
    {
        if (currentState == EnemyState.aware)
            return;
        ResetEnemy();

        shouldShoot = true;
        if(target != null)
        {
            this.target = target;
        }
        isAiming = true;
        animator.SetBool("isAiming", true);
    }
    private void Aim()
    {
        if (target != null)
        {
            RotateTowardsTransform(target);
            Vector3 lookDirection = target.position - rightShoulder.position;
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            aimAngle = lookRotation.eulerAngles.x;
            gun.Shoot(target);
        }
        spine.Rotate(transform.right, aimAngle, Space.World);
    }

    private void Fire()
    {
        Vector3 fireDirection = target.position - fireOrigin.position;
        fireDirection = Quaternion.LookRotation(fireDirection).eulerAngles;
        fireDirection.y = transform.eulerAngles.y;
        fireDirection.z = transform.eulerAngles.z;
        fireOrigin.eulerAngles = fireDirection;


        Vector3 rayDirection = (fireOrigin.forward) * Vector3.Distance(target.position, fireOrigin.position);
        Debug.DrawRay(fireOrigin.position, rayDirection);
    }

    private void RotateTowardsTransform(Transform target)
    {
        Vector3 lookDirection = target.position - transform.position;
        lookDirection.y = 0;
        Vector3 lookRotation = Quaternion.LookRotation(lookDirection).eulerAngles;
        transform.DORotate(lookRotation, rotationDelay);
    }

    private void ResetEnemy()
    {
        isRifleUp = false;
        isAiming = false;
        isShooting = false;
        shouldShoot = false;
        isTargetVisible = false;
        animator.SetBool("isAiming", false);
        animator.SetBool("isAlert", false);
        currentState = EnemyState.idle;
        spine.Rotate(transform.right, 0, Space.World);
    }
}
