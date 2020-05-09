using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public abstract class IWeapon : MonoBehaviour
{
    public CinemachineVirtualCamera tpsCam;
    public Animator animator;
    public RigBuilder rigBuilder;
    [HideInInspector] public Camera mainCam;
    public TPSCamController camController;
    [HideInInspector] public float defaultFOV, defaultMouseSensitivity;
    public bool activeStatus;

    private void Awake()
    {
        mainCam = Camera.main;
        camController = tpsCam.GetComponent<TPSCamController>();
        defaultFOV = 40;
        defaultMouseSensitivity = camController.GetDefaultMouseSensitivity();
    }

    public abstract void OnRightClickDown();
    public abstract void OnRightClickHold();
    public abstract void OnRightClickUp();
    public abstract void OnLeftClickDown();
    public abstract void OnLeftClickHold();
    public abstract void OnLeftClickUp();
    public abstract void Equip();
    public abstract void Discard();
    public abstract void OnCamPosRotChanged(Vector3 position, Quaternion rotation);
}
