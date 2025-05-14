using UnityEngine;
using UnityEngine.AI;

public class FootManager : MonoBehaviour
{
    public FootStepper leftFoot;
    public FootStepper rightFoot;

    public Transform leftTarget;
    public Transform rightTarget;

    public NavMeshAgent agent;

    public float stepThreshold = 0.25f;
    public float moveThreshold = 0.01f;
    public float forwardStepOffset = 0.3f;

    private Vector3 lastBodyPos;
    private bool isLeftTurn = true;

    void Start()
    {
        lastBodyPos = agent.transform.position;
    }

    void Update()
    {
        Vector3 bodyDelta = agent.transform.position - lastBodyPos;
        float bodySpeed = bodyDelta.magnitude / Time.deltaTime;
        lastBodyPos = agent.transform.position;

        if (bodySpeed < moveThreshold)
            return;

        // Sadece bir ayaða izin veriyoruz
        if (leftFoot.IsStepping || rightFoot.IsStepping)
            return;

        if (isLeftTurn)
        {
            float dist = Vector3.Distance(leftFoot.transform.position, leftTarget.position);
            if (dist > stepThreshold)
            {
                Vector3 adjusted = GetStepPosition(leftTarget);
                leftFoot.StepTo(adjusted, 1f); // ileri
                isLeftTurn = false;
            }
        }
        else
        {
            float dist = Vector3.Distance(rightFoot.transform.position, rightTarget.position);
            if (dist > stepThreshold)
            {
                Vector3 adjusted = GetStepPosition(rightTarget);
                rightFoot.StepTo(adjusted, 1f); // ileri
                isLeftTurn = true;
            }
        }

    }

    Vector3 GetStepPosition(Transform target)
    {
        Vector3 forward = agent.transform.forward;
        return target.position + forward;
    }
}
