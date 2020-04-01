using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform target;
    //[SerializeField] private float rotationSpeed = 5;

    //[HideInInspector] public bool isAiming = false;

    private Animator anim;
    private Transform chest;
    //private Quaternion targetRotation;

    void Start()
    {
        anim = GetComponent<Animator>();
        chest = anim.GetBoneTransform(HumanBodyBones.Head);
    }

    void LateUpdate()
    {
        if (anim.GetBool("isAiming"))
            chest.LookAt(target.position);
        //{
        //    targetRotation = Quaternion.LookRotation(target.transform.position, chest.position);
        //    chest.rotation = Quaternion.Slerp(chest.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //}
    }
}
