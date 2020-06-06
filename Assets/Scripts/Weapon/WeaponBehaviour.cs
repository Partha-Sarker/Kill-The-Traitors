using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public abstract class WeaponBehaviour : MonoBehaviour
{
    public CinemachineFreeLook tpsCam;
    public Animator animator;
    public Transform playerTarget;
    [HideInInspector] public Transform spine;
    [HideInInspector] public Camera mainCam;
    [HideInInspector] public float defaultFOV;
    public bool activeStatus;

    private void Awake()
    {
        mainCam = Camera.main;
        defaultFOV = 40;
        spine = animator.GetBoneTransform(HumanBodyBones.Spine);
    }

    public abstract void OnRightKeyDown();
    public abstract void OnRightKeyHold();
    public abstract void OnRightKeyUp();
    public abstract void OnLeftKeyDown();
    public abstract void OnLeftKeykHold();
    public abstract void OnLeftKeyUp();
    public abstract void OnMiddleKeyDown();
    public abstract void OnMiddleKeyUp();
    public abstract void Equip();
    public abstract void Discard();
}
