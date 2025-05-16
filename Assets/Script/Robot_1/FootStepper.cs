using UnityEngine;
using UnityEngine.AI;

public class FootStepper : MonoBehaviour
{
    public float stepHeight = 0.1f;
    public float stepDuration = 0.2f;

    private Vector3 lastPosition;
    private Vector3 currentStepTarget;
    private float stepProgress = 10f;
    private bool isStepping = false;

    public bool IsStepping => isStepping;

    public float stepOffset = 0f;

    public Transform agentT;
    public Transform footManager;

    public void StepTo(Vector3 newTarget, float forwardOffset)
    {
        if (isStepping) return;

        lastPosition = transform.position;

        var agent = agentT.GetComponent<NavMeshAgent>();
        Vector3 forward = agent.transform.forward.normalized;

        Vector3 offset = forward * forwardOffset;

        currentStepTarget = newTarget + offset;
        stepProgress = 0f;
        isStepping = true;

        if (footManager.TryGetComponent<FootManager>(out FootManager manager))
        {
            if (manager.leftFoot != this)
                manager.leftFoot.stepOffset = -2f;
            if (manager.rightFoot != this)
                manager.rightFoot.stepOffset = -2f;
        }

        forwardOffset = stepOffset;
    }


    private void Update()
    {
        if (!isStepping) return;

        stepProgress += Time.deltaTime / stepDuration;

        Vector3 horizontal = Vector3.Lerp(lastPosition, currentStepTarget, stepProgress);
        float height = Mathf.Sin(stepProgress * Mathf.PI) * stepHeight;

        transform.position = horizontal + Vector3.up * height;

        if (stepProgress >= 1f)
        {
            isStepping = false;
            transform.position = currentStepTarget;
        }
    }
}