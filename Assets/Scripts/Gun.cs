using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public Transform idleTransform, aimingTransform, rightHand, player;
    public float switchTime = .3f;
    public string name;

    public void Aim()
    {
        transform.parent = player;
        transform.localPosition = aimingTransform.localPosition;
        transform.localRotation = aimingTransform.localRotation;
        //StopAllCoroutines();
        //StartCoroutine(AimSmoothly(transform, aimingTransform));
    }

    public void Idle()
    {
        transform.parent = rightHand;
        transform.localPosition = idleTransform.localPosition;
        transform.localRotation = idleTransform.localRotation;
        //StopAllCoroutines();
        //StartCoroutine(IdleSmoothly(transform, idleTransform));
    }

    //IEnumerator AimSmoothly(Transform source, Transform target)
    //{
    //    float startTime = Time.time;
    //    while (Time.time < startTime + switchTime)
    //    {
    //        transform.localPosition = Vector3.Lerp(source.localPosition, target.localPosition, (Time.time - startTime) / switchTime);
    //        transform.localRotation = Quaternion.Lerp(source.localRotation, target.localRotation, (Time.time - startTime) / switchTime);
    //        yield return null;
    //    }
    //    transform.localPosition = target.localPosition;
    //    transform.localRotation = target.localRotation;
    //}

    //IEnumerator IdleSmoothly(Transform source, Transform target)
    //{
    //    transform.parent = rightHand;
    //    float startTime = Time.time;
    //    while (Time.time < startTime + switchTime)
    //    {
    //        transform.localPosition = Vector3.Lerp(source.localPosition, target.localPosition, (Time.time - startTime) / switchTime);
    //        transform.localRotation = Quaternion.Lerp(source.localRotation, target.localRotation, (Time.time - startTime) / switchTime);
    //        yield return null;
    //    }
    //    transform.localPosition = target.localPosition;
    //    transform.localRotation = target.localRotation;
    //}
}
