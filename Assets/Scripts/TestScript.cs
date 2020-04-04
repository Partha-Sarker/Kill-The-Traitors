using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public enum TestMode {CopyLocalXRotation, DrawRay};
    public TestMode mode = TestMode.CopyLocalXRotation;
    public Transform target;
    private Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == TestMode.CopyLocalXRotation)
        {
            rotation = transform.localRotation;
            rotation.x = target.localRotation.x;
            transform.localRotation = rotation;
        }
        if (mode == TestMode.DrawRay)
        {
            Debug.DrawRay(transform.position, transform.forward * 100, Color.green);
        }
    }
}
