using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayerMovementController : MonoBehaviour
{
    private Animator animator;
    public Transform cam;
    public enum MovementState { walking, strafing, crouching, proning, still };
    public MovementState currentMovementState = MovementState.walking;

    private Rigidbody rb;

    public bool isGrounded;

    [Range(0f, 1f)] public float horizontalJumpLimit;
    [SerializeField] private float moveSpeed = 5, jumpForce = 10, rotationHardness = 10;
    [SerializeField] private float inputLerpTime = .1f, minimumInputValue = .01f, maximumInputValue = .999f;

    private Vector3 force;

    private float hInput, vInput, rawHInput, rawVInput, finalHInput, finalVInput, speedMultiplier = 1;
    public float relativeAngle, rotationTime = .2f;
    private Vector3 rotation;

    public Vector2 smoothInput;

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

        DOTween.To(() => hInput, x => hInput = x, rawHInput, inputLerpTime);
        DOTween.To(() => vInput, x => vInput = x, rawVInput, inputLerpTime);

        RoundInputValues();

        smoothInput = new Vector2(hInput, vInput);
        if (smoothInput.magnitude > 1)
            smoothInput = smoothInput.normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            DOTween.To(() => speedMultiplier, x => speedMultiplier = x, 2, .3f);
        else if(Input.GetKeyUp(KeyCode.LeftShift))
            DOTween.To(() => speedMultiplier, x => speedMultiplier = x, 1, .3f);

        animator.SetFloat("motionSpeed", smoothInput.magnitude * speedMultiplier);


        animator.SetFloat("vInput", vInput * speedMultiplier);
        animator.SetFloat("hInput", hInput * speedMultiplier);

        RotatePlayer();

        MovePlayer();

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

    }

    private void RoundInputValues()
    {
        if (Mathf.Abs(hInput) < minimumInputValue && rawHInput == 0)
            hInput = 0;
        if (Mathf.Abs(vInput) < minimumInputValue && rawVInput == 0)
            vInput = 0;

        if (Mathf.Abs(hInput) > maximumInputValue && Mathf.Abs(rawHInput) == 1)
            hInput = Mathf.Round(hInput);
        if (Mathf.Abs(vInput) > maximumInputValue && Mathf.Abs(rawVInput) == 1)
            vInput = Mathf.Round(vInput);
    }

    private void RotatePlayer()
    {
        if (smoothInput.magnitude < .2f || !isGrounded)
            return;

        relativeAngle = Mathf.Atan2(smoothInput.x, smoothInput.y) * Mathf.Rad2Deg;
        rotation = Vector3.zero;
        if (currentMovementState == MovementState.walking)
            rotation.y = cam.eulerAngles.y + relativeAngle;
        else if (currentMovementState == MovementState.strafing)
            rotation.y = cam.eulerAngles.y;

        transform.DORotate(rotation, rotationTime);
    }

    private void MovePlayer()
    {
        if (smoothInput.magnitude < .1f || !isGrounded)
            return;
        rotation = cam.eulerAngles;
        rotation.x = 0;
        Vector3 camForward =Quaternion.Euler(rotation) * Vector3.forward;
        Vector3 moveDirection = Quaternion.AngleAxis(relativeAngle, transform.up) * camForward;
        moveDirection = moveDirection.normalized;
        Vector3 moveVelocity = moveDirection * moveSpeed * smoothInput.magnitude * speedMultiplier;
        moveVelocity.y = rb.velocity.y;
        rb.velocity = moveVelocity;

        //Debug.DrawLine(transform.position, transform.position + moveDirection);
    }

    private void Jump()
    {
        if (!isGrounded)
            return;
        if (currentMovementState == MovementState.still)
            return;

        force.x = smoothInput.x * horizontalJumpLimit;
        force.y = 1;
        force.z = smoothInput.y * horizontalJumpLimit;
        rb.AddRelativeForce(force * jumpForce, ForceMode.Impulse);
        animator.SetTrigger("jump");
    }

    public void SetWalking()
    {
        currentMovementState = MovementState.walking;
    }

    public void SetStafing()
    {
        currentMovementState = MovementState.strafing;
    }

    public void SetStill()
    {
        currentMovementState = MovementState.still;
    }

}
