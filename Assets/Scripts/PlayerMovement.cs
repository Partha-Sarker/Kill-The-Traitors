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
    private Vector3 lastMousePosition;
    private bool mouseMoving = false, isAimaing = false;

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
        //CheckMouseMovement();

        hInput = Input.GetAxis("Horizontal") * multiplier;
        vInput = Input.GetAxis("Vertical") * multiplier;

        if (Input.GetMouseButtonDown(1))
            animator.SetBool("aiming", isAimaing = true);
        if(Input.GetMouseButtonUp(1))
            animator.SetBool("aiming", isAimaing = false);


        if (Input.GetKey(KeyCode.LeftShift))
            multiplier = Mathf.Lerp(multiplier, 2, Time.deltaTime * sprintSwitchSMoothness);
        else
            multiplier = Mathf.Lerp(multiplier, 1, Time.deltaTime * sprintSwitchSMoothness);

        animator.SetFloat("vInput", vInput);
        animator.SetFloat("hInput", hInput);
    }

    private void LateUpdate()
    {
        if (vInput == 0 && !isAimaing)
            return;
        rotation = cam.transform.localRotation;
        rotation.x = transform.localRotation.x;
        rotation.z = transform.localRotation.z;
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerRotationSmoothness);
        //transform.localRotation = rotation;
    }

    private void CheckMouseMovement()
    {
        if (Input.mousePosition != lastMousePosition)
        {
            lastMousePosition = Input.mousePosition;
            mouseMoving = true;
        }
        else
            mouseMoving = false;
    }
}
