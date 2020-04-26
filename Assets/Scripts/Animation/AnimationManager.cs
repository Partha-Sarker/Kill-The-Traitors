using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private Rig rightArmRig;
    [SerializeField] private Rig leftArmRig;
    [SerializeField] private Rig headRig;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            OnRifleUpEnter();
    }

    public void OnRifleDownEnter()
    {
        rightArmRig.weight = 0;
        leftArmRig.weight = 1;
        headRig.weight = 0;
        print("Rifle going down");
    }

    public void OnRifleUpEnter()
    {
        rightArmRig.weight = 1;
        leftArmRig.weight = 1;
        headRig.weight = 1;
        print("Rifle going up");
    }

    public void OnTossGrenadeEnter()
    {
        rightArmRig.weight = 0;
        leftArmRig.weight = 0;
        headRig.weight = 0;
        print("tossing grenade");
    }
}
