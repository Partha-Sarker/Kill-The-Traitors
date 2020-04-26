using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IThrowable : MonoBehaviour
{
    public GameObject grenade;
    public Transform grenadePosition;
    public float grenadeForce;
    public Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    public abstract void Throw();
}
