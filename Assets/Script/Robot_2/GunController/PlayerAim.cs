using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAim : MonoBehaviour
{
    private WeaponChanger wpc;
    private Animator animator;
    private PlayerController playerController;

    public MultiAimConstraint multiAimConstraint;
    public Transform targetTransform;



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
    }


}
