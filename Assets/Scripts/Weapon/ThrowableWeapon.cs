using UnityEngine;
using System.Collections;

public class ThrowableWeapon : IWeapon
{
    [SerializeField] private GameObject throwingObject;
    private GameObject tempThrowingObject;
    [SerializeField] private Transform throwingObjectParent;
    [SerializeField] private Transform player;
    [SerializeField] private float coolDown = 3f, nextTimeToThrow = 0, fov = 25;

    [SerializeField] private Vector3 throwingOffset;
    [SerializeField] private int force, randomRotation;
    [SerializeField] private float fovSwitchTime = .2f;

    public override void OnCamPosRotChanged(Vector3 position, Quaternion rotation)
    {
        player.rotation = Quaternion.Euler(player.eulerAngles.x, rotation.eulerAngles.y, player.eulerAngles.z);
    }

    public override void Discard()
    {
        animator.SetBool("isStrafing", false);
        StartCoroutine(ChangeFieldOfViewSmoothly(tpsCam.m_Lens.FieldOfView, 40));
        if (tempThrowingObject != null)
            Destroy(tempThrowingObject);
        ResetThrowable();
    }

    public override void Equip()
    {
        animator.SetTrigger("switchToGrenade");
        StartCoroutine(ChangeFieldOfViewSmoothly(tpsCam.m_Lens.FieldOfView, fov));
        tpsCam.m_Lens.FieldOfView = fov;
        animator.SetBool("isStrafing", true);
        ResetThrowable();
    }

    private void ResetThrowable()
    {
        rigBuilder.enabled = false;
    }

    public override void OnLeftClickDown()
    {
        if(Time.time > nextTimeToThrow)
        {
            activeStatus = true;
            InstantiateThrowable();
        }
    }

    private void InstantiateThrowable()
    {
        animator.SetTrigger("throwing");
        tempThrowingObject = Instantiate(throwingObject, throwingObjectParent);
    }

    public void Throw()
    {
        if(activeStatus && tempThrowingObject != null)
        {
            tempThrowingObject.GetComponent<IThrowable>().StartThrowingPreparation();
            Rigidbody rb = tempThrowingObject.GetComponent<Rigidbody>();
            rb.AddTorque(
                new Vector3(Random.Range(
                    -randomRotation, randomRotation), Random.Range(-randomRotation, randomRotation), Random.Range(-randomRotation, randomRotation)
                    )
                );
            rb.AddForce(mainCam.transform.forward * force + throwingOffset, ForceMode.VelocityChange);
            activeStatus = false;
        }
    }

    public void SetActiveStatus(bool status)
    {
        activeStatus = status;
    }

    public override void OnLeftClickHold()
    {
        return;
    }

    public override void OnLeftClickUp()
    {
        if (activeStatus && tempThrowingObject != null)
        {
            animator.SetTrigger("throw");
        }
    }

    public override void OnRightClickDown()
    {
        return;
    }

    public override void OnRightClickHold()
    {
        return;
    }

    public override void OnRightClickUp()
    {
        return;
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
}
