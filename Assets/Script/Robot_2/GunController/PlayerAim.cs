using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAim : MonoBehaviour
{
    private WeaponChanger wpc;
    private Animator animator;
    private PlayerController playerController;

    public MultiAimConstraint multiAimConstraint;
    public Transform targetTransform;

    public Vector3 idlePos;
    public Vector3 idleRot;
    public Vector3 walkPos;
    public Vector3 walkRot;
    public Vector3 runPos;
    public Vector3 runRot;

    void Start()
    {
        wpc = GetComponentInChildren<WeaponChanger>();
        animator = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (wpc.equip)
        {
            if (Input.GetMouseButtonDown(1))
            {
                animator.SetBool("aiming", true);
                multiAimConstraint.weight = 1f;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                animator.SetBool("aiming", false);
                multiAimConstraint.weight = 0f;
            }
        }

        UpdateTargetTransform();
    }

    void UpdateTargetTransform()
    {
        switch (playerController.currentState)
        {
            case PlayerState.Idle:
                targetTransform.localPosition = idlePos;
                targetTransform.localEulerAngles = idleRot;
                break;

            case PlayerState.Walking:
                targetTransform.localPosition = walkPos;
                targetTransform.localEulerAngles = walkRot;
                break;

            case PlayerState.Running:
                targetTransform.localPosition = runPos;
                targetTransform.localEulerAngles = runRot;
                break;
        }
    }
}
