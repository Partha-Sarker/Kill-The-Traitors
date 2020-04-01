using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform target;
    public enum orbitAround {up, right, forward };
    public orbitAround axis = orbitAround.right;
    public float rotation;
    private Transform camTransform;

    private void Start()
    {
        camTransform = Camera.main.transform;
    }

    void Update()
    {
        // Spin the object around the world origin at 20 degrees/second.
        //rotation = camTransform.localRotation.x - transform.localRotation.x;

        if(axis == orbitAround.up)
            transform.RotateAround(target.position, Vector3.up, rotation);
        else if(axis == orbitAround.right)
            transform.RotateAround(target.position, Vector3.right, rotation);
        else
            transform.RotateAround(target.position, Vector3.forward, rotation);
    }

}
