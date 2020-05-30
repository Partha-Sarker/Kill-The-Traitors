using System.Collections;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Animator animator;
    public Transform cam;

    [SerializeField] private float rotationHardness = 10;
    [SerializeField] private float inputLerpTime = .1f, minimumInputValue = .01f;


    private float hInput, vInput, rawHInput, rawVInput, speedMultiplier = 1;
    private Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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
    }

    private void LateUpdate()
    {
        //if (rawHInput != 0 || rawVInput != 0)
        //{
        //    rotation = cam.rotation;
        //    rotation.x = transform.rotation.x;
        //    rotation.z = transform.rotation.z;
        //    transform.rotation = rotation;
        //    //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSmoothness);
        //}
    }
}
