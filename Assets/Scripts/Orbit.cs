using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public enum TestMode { CopyLocalXRotation };
    public TestMode mode = TestMode.CopyLocalXRotation;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (mode == TestMode.CopyLocalXRotation)
        {
            //rotation = transform.localRotation;
            //rotation.x = target.localRotation.x;
            //transform.localRotation = rotation;
            transform.localRotation = Quaternion.Euler(target.localRotation.eulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
    }
}
