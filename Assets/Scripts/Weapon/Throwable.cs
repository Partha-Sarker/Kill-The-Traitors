using UnityEngine;

public class Throwable : IWeapon
{
    [SerializeField] private GameObject throwingObject;
    private GameObject tempThrowingObject;
    [SerializeField] private Transform throwingObjectParent;
    [SerializeField] private Transform player;
    [SerializeField] private float coolDown = 3f, nextTimeToThrow = 0, fov = 25;

    [SerializeField] private Vector3 throwingOffset;
    [SerializeField] private int force, randomRotation;


    public override void OnCamPosRotChanged(Vector3 position, Quaternion rotation)
    {
        player.rotation = Quaternion.Euler(player.eulerAngles.x, rotation.eulerAngles.y, player.eulerAngles.z);
    }

    public override void Discard()
    {
        animator.SetBool("isStrafing", false);
        tpsCam.m_Lens.FieldOfView = 40;
        ResetThrowable();
    }

    public override void Equip()
    {
        animator.SetTrigger("switchToGrenade");
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
        print("throwing");
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
}
