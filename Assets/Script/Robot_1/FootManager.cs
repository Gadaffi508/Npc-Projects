using UnityEngine;
using UnityEngine.AI;

public class FootManager : MonoBehaviour
{
    public FootStepper leftFoot;
    public FootStepper rightFoot;

    public Transform leftTarget;
    public Transform rightTarget;

    public NavMeshAgent agent; // Artýk NavMesh kullanýyoruz

    public float stepThreshold = 0.25f;
    public float moveThreshold = 0.01f;

    private bool isLeftTurn = true;
    private Vector3 lastBodyPos;

    void Start()
    {
        lastBodyPos = agent.transform.position;
    }

    void Update()
    {
        Vector3 bodyDelta = agent.transform.position - lastBodyPos;
        float bodySpeed = bodyDelta.magnitude / Time.deltaTime;

        lastBodyPos = agent.transform.position;

        if (bodySpeed < moveThreshold) return;
        if (leftFoot.IsStepping || rightFoot.IsStepping) return;

        if (isLeftTurn)
        {
            if (!leftFoot.IsStepping)
            {
                Vector3 adjustedTarget = GetAdjustedTarget(leftTarget, 0.3f); // ileriye 0.3 birim at
                leftFoot.StepTo(adjustedTarget);
                isLeftTurn = false;
            }
        }
        else
        {
            if (!rightFoot.IsStepping)
            {
                Vector3 adjustedTarget = GetAdjustedTarget(rightTarget, 0.3f);
                rightFoot.StepTo(adjustedTarget);
                isLeftTurn = true;
            }
        }

    }

    private Vector3 GetAdjustedTarget(Transform target, float forwardOffset)
    {
        Vector3 forward = agent.transform.forward;
        return target.position + forward * forwardOffset;
    }


}
