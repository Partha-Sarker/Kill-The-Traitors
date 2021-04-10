using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private GunAndGrenade weapon;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ThrowGrenade()
    {
        weapon.ThrowGrenade();
    }
}
