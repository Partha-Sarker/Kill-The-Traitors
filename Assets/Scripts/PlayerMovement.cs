using System.Collections;
using Cinemachine;
using UnityEngine.Animations.Rigging;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private Camera cam;

    [SerializeField] private float playerRotationSmoothness = 10;
    [SerializeField] private float animationTransitionTime = 1;
    [SerializeField] private float fovSwitchTime = .2f;
    [SerializeField] private float aimSwitchTime = .2f;
    [SerializeField] private Rig rightArmRig;
    [SerializeField] private Rig headRig;
    [SerializeField] private CinemachineFreeLook CMCam;


    private float hInput, vInput, multiplier = 1;
    private Quaternion rotation;
    private bool isAimaing = false;

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
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("isAiming", isAimaing = true);
            StartCoroutine(ChangeRigWeightSmoothly(0, 1));
            StartCoroutine(ChangeFieldOfViewSmoothly(40, 20));
            //LookAtTarget.isAiming = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool("isAiming", isAimaing = false);
            StartCoroutine(ChangeRigWeightSmoothly(1, 0));
            StartCoroutine(ChangeFieldOfViewSmoothly(20, 40));
            //LookAtTarget.isAiming = false;
        }


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            multiplier = 2;
            //StartCoroutine(ChangeMultiplierSmoothly(multiplier, 2));
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            multiplier = 1;
            //StartCoroutine(ChangeMultiplierSmoothly(multiplier, 1));
        }

        hInput = Input.GetAxisRaw("Horizontal") * multiplier;
        vInput = Input.GetAxisRaw("Vertical") * multiplier;

        animator.SetFloat("vInput", vInput, animationTransitionTime, Time.deltaTime);
        animator.SetFloat("hInput", hInput, animationTransitionTime, Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (vInput == 0 && !isAimaing)
            return;
        rotation = cam.transform.localRotation;
        rotation.x = transform.localRotation.x;
        rotation.z = transform.localRotation.z;
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerRotationSmoothness);
    }

    IEnumerator ChangeRigWeightSmoothly(float source, float target)
    {
        float startTime = Time.time;
        while (Time.time < startTime + aimSwitchTime)
        {
            rightArmRig.weight = Mathf.Lerp(source, target, (Time.time - startTime) / aimSwitchTime);
            headRig.weight = rightArmRig.weight;
            yield return null;
        }
        rightArmRig.weight = target;
        headRig.weight = target;
    }

    IEnumerator ChangeFieldOfViewSmoothly(float source, float target)
    {
        float startTime = Time.time;
        while (Time.time < startTime + fovSwitchTime)
        {
            CMCam.m_Lens.FieldOfView = Mathf.Lerp(source, target, (Time.time - startTime) / fovSwitchTime);
            yield return null;
        }
        CMCam.m_Lens.FieldOfView = target;
    }

    //IEnumerator ChangeMultiplierSmoothly(float source, float target)
    //{
    //    float startTime = Time.time;
    //    while (Time.time < startTime + sprintSwithTime)
    //    {
    //        multiplier = Mathf.Lerp(source, target, (Time.time - startTime) / sprintSwithTime);
    //        yield return null;
    //    }
    //    multiplier = target;
    //}


}
