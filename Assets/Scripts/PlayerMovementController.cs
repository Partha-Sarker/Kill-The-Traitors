using System.Collections;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Animator animator;
    public Transform cam;

    private Rigidbody rb;

    public bool isGrounded;

    [Range(0f, 1f)] public float horizontalJumpLimit;
    [SerializeField] private float jumpForce = 10, rotationHardness = 10;
    [SerializeField] private float inputLerpTime = .1f, minimumInputValue = .01f;

    private Vector3 force;

    private float hInput, vInput, rawHInput, rawVInput, speedMultiplier = 1;
    private Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rawHInput = Input.GetAxisRaw("Horizontal");
        rawVInput = Input.GetAxisRaw("Vertical");

        hInput = Mathf.Lerp(hInput, rawHInput, inputLerpTime);
        vInput = Mathf.Lerp(vInput, rawVInput, inputLerpTime);

        if(Mathf.Abs(hInput) < minimumInputValue && rawHInput == 0)
            hInput = 0;
        if(Mathf.Abs(vInput) < minimumInputValue && rawVInput == 0)
            vInput = 0;

        animator.SetFloat("vInput", vInput);
        animator.SetFloat("hInput", hInput);

        if (rawVInput != 0 || rawHInput != 0)
        {
            rotation = cam.rotation;
            rotation.x = transform.rotation.x;
            rotation.z = transform.rotation.z;
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationHardness);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.applyRootMotion = false;
            force.x = hInput * horizontalJumpLimit;
            force.y = 1;
            force.z = vInput * horizontalJumpLimit;
            rb.AddRelativeForce(force * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("jump");
        }
    }

}
