using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    [SerializeField] private PlayerMovementController controller;
    [SerializeField] private Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            controller.isGrounded = true;
            animator.SetBool("isGrounded", true);
            animator.applyRootMotion = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            controller.isGrounded = false;
            animator.SetBool("isGrounded", false);
        }
    }

}
