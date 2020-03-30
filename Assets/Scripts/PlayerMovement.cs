using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private Camera cam;

    [SerializeField] private float playerRotationSmoothness = 10;
    [SerializeField] private float sprintSwitchSMoothness = 10;


    private float hInput, vInput, multiplier = 1;
    private Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        hInput = Input.GetAxis("Horizontal") * multiplier;
        vInput = Input.GetAxis("Vertical") * multiplier;

        if (Input.GetKey(KeyCode.LeftShift))
            multiplier = Mathf.Lerp(multiplier, 2, Time.deltaTime * sprintSwitchSMoothness);
        else
            multiplier = Mathf.Lerp(multiplier, 1, Time.deltaTime * sprintSwitchSMoothness);

        animator.SetFloat("vInput", vInput);
        animator.SetFloat("hInput", hInput);
    }

    private void LateUpdate()
    {
        if (vInput == 0)
            return;
        rotation = cam.transform.localRotation;
        rotation.x = transform.localRotation.x;
        rotation.z = transform.localRotation.z;
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerRotationSmoothness);
        //transform.localRotation = rotation;
    }
}
