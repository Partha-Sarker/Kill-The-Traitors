using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThrowableManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject grenade;
    [SerializeField] private Transform throwingPosition;
    [SerializeField] private Vector3 throwingOffset;
    [SerializeField] private int force, randomRotation;
    private Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    public void Throw()
    {
        animator.SetTrigger("throw");
        //FindObjectOfType<AnimationManager>().OnTossGrenadeEnter();
        Vector3 rotation = transform.eulerAngles;
        rotation.y = cam.eulerAngles.y;
        transform.eulerAngles = rotation;
        GameObject tempGrenade = Instantiate(grenade, throwingPosition.position, throwingPosition.rotation);
        tempGrenade.GetComponent<Rigidbody>().AddTorque(
            new Vector3(Random.Range(
                -randomRotation, randomRotation), Random.Range(-randomRotation, randomRotation), Random.Range(-randomRotation, randomRotation)
                )
            );
        tempGrenade.GetComponent<Rigidbody>().AddForce(cam.forward * force + throwingOffset, ForceMode.VelocityChange);
    }
}
