using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public Transform idleTransform, aimingTransform, rightHand, player, gunHolder;
    public Transform leftHandRigPos, rightHandRigPos, idleLeftHandRigPos, idleRightHandRigPos, aimingLeftHandRigPos, aimingRightHandRigPos;
    public float switchTime = .3f;
    public string name;

    public void Aim()
    {
        transform.parent = player;
        transform.localPosition = aimingTransform.localPosition;
        transform.localRotation = aimingTransform.localRotation;
        gunHolder.rotation = Quaternion.identity;
        transform.parent = gunHolder;
        gunHolder.localRotation = Quaternion.Euler(Camera.main.transform.eulerAngles.x, 0, 0);
    }

    public void Idle()
    {
        transform.parent = rightHand;
        transform.localPosition = idleTransform.localPosition;
        transform.localRotation = idleTransform.localRotation;
    }
}
