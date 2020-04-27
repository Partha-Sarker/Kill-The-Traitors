using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject grenade;
    [SerializeField] private Transform throwingPosition;
    [SerializeField] private Vector3 throwingOffset;
    [SerializeField] private int force;
    private Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    public void Throw()
    {
        animator.SetTrigger("throw");
        //FindObjectOfType<AnimationManager>().OnTossGrenadeEnter();
        GameObject tempGrenade = Instantiate(grenade, throwingPosition.position, throwingPosition.rotation);
        tempGrenade.GetComponent<Rigidbody>().AddForce(cam.forward * force + throwingOffset, ForceMode.VelocityChange);
        Destroy(tempGrenade, 4);
    }
}
