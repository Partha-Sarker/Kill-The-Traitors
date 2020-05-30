using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public abstract class WeaponBehaviour : MonoBehaviour
{
    public CinemachineFreeLook tpsCam;
    public Animator animator;
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

    public abstract void OnRightClickDown();
    public abstract void OnRightClickHold();
    public abstract void OnRightClickUp();
    public abstract void OnLeftClickDown();
    public abstract void OnLeftClickHold();
    public abstract void OnLeftClickUp();
    public abstract void Equip();
    public abstract void Discard();
}
