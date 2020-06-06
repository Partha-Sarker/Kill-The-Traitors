using DG.Tweening;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private bool targetVisibility = false, playerIsOutside = false, playerIsInside = false;

    public float visionLimit = 30, rotationDelay = .5f, stateCooldown = 3;
    private float aimAngle = 0, lastVisibleTime, lastPlayerZoneChangeTime, lastStateChangeTime;

    public Transform target;
    private Transform spine, rightShoulder, head;

    public EnemyGun enemyGun;

    public enum EnemyState { idle, looking, alert, aware };
    public EnemyState currentState = EnemyState.idle;

    private Animator animator;

    [SerializeField] private float testFloat1;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spine = animator.GetBoneTransform(HumanBodyBones.Spine);
        rightShoulder = animator.GetBoneTransform(HumanBodyBones.RightShoulder);
        head = animator.GetBoneTransform(HumanBodyBones.Head);
    }

    private void LateUpdate()
    {
        if(currentState == EnemyState.looking)
            Look();
        if (currentState == EnemyState.alert)
            Search();
        else if (currentState == EnemyState.aware)
            Aim();

        if(currentState != EnemyState.idle && !playerIsInside)
        {
            CheckCooldown();
        }
    }

    public void OnTargetEnter(Collider target)
    {
        if (!target.CompareTag("Target"))
            return;

        if (!playerIsOutside)
        {
            print("Target on outside zone!");
            playerIsOutside = true;
            SetLooking(target.transform);
            return;
        }

        if (!playerIsInside)
        {
            print("Target on inside zone!");
            playerIsInside = true;
            SetAlert(target.transform);
            return;
        }
    }

    public void OnTargetExit(Collider target)
    {
        if (!target.CompareTag("Target"))
            return;

        lastPlayerZoneChangeTime = Time.time;
        if (playerIsInside)
        {
            playerIsInside = false;
            print("Target on outside zone!");
        }
        else if (playerIsOutside)
        {
            playerIsOutside = false;
            print("Target has escaped enemy zone!");
        }
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
        this.target = target;
        RotateTowardsTarget();
        animator.SetBool("isAlert", true);
    }

    public void SetAware(Transform target)
    {
        if (currentState == EnemyState.aware)
            return;
        ResetEnemy();

        this.target = target;
        RotateTowardsTarget();
        currentState = EnemyState.aware;
        this.target = target;
        animator.SetBool("isAiming", true);
    }

    public void SetLooking(Transform target)
    {
        if (currentState == EnemyState.looking)
            return;
        ResetEnemy();

        currentState = EnemyState.looking;
        this.target = target;
    }

    private void CheckCooldown()
    {
        if(Time.time - lastVisibleTime > stateCooldown && Time.time - lastStateChangeTime > stateCooldown)
        {
            if (!playerIsOutside)
                SetIdle();
            else
                SetLooking(target);
        }
    }

    private void Search()
    {
        if (IsTargetVisible())
        {
            SetAware(target);
        }
    }

    private void Look()
    {
        if (!IsTargetVisible())
            return;

        Vector3 targetDir = (target.position - transform.position).normalized;
        float dot = Vector3.Dot(targetDir, transform.forward);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        testFloat1 = angle;
        if(angle < visionLimit)
        {
            SetAware(target);
        }
    }

    private void Aim()
    {
        if (target != null && IsTargetVisible())
        {
            RotateTowardsTarget();
            Vector3 lookDirection = target.position - rightShoulder.position;
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            aimAngle = lookRotation.eulerAngles.x;
            enemyGun.Shoot(target);
        }
        spine.Rotate(transform.right, aimAngle, Space.World);
    }

    private bool IsTargetVisible()
    {
        if (Physics.Raycast(head.position, target.position - head.position, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("Target") || hit.transform.CompareTag("Player"))
            {
                targetVisibility = true;
                lastVisibleTime = Time.time;
            }
            else
            {
                targetVisibility = false;
            }
        }
        return targetVisibility;
    }

    private void RotateTowardsTarget()
    {
        Vector3 lookDirection = target.position - transform.position;
        lookDirection.y = 0;
        Vector3 lookRotation = Quaternion.LookRotation(lookDirection).eulerAngles;
        transform.DORotate(lookRotation, rotationDelay);
    }

    private void ResetEnemy()
    {
        lastStateChangeTime = Time.time;
        targetVisibility = false;
        animator.SetBool("isAiming", false);
        animator.SetBool("isAlert", false);
        currentState = EnemyState.idle;
        spine.Rotate(transform.right, 0, Space.World);
        if (target != null)
            RotateTowardsTarget();
    }
}
