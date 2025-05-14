using UnityEngine;

public class FootStepper : MonoBehaviour
{
    public float stepHeight = 0.1f;
    public float stepDuration = 0.2f;

    private Vector3 lastPosition;
    private Vector3 currentStepTarget;
    private float stepProgress = 10f;
    private bool isStepping = false;

    public bool IsStepping => isStepping;

    public void StepTo(Vector3 newTarget)
    {
        if (isStepping) return;

        lastPosition = transform.position;
        currentStepTarget = newTarget;
        stepProgress = 0f;
        isStepping = true;
    }

    private void Update()
    {
        if (!isStepping) return;

        stepProgress += Time.deltaTime / stepDuration;

        Vector3 horizontal = Vector3.Lerp(lastPosition, currentStepTarget, stepProgress * 5);
        float height = Mathf.Sin(stepProgress * Mathf.PI) * stepHeight;

        transform.position = horizontal + Vector3.up * height;

        if (stepProgress >= 1f)
        {
            isStepping = false;
            transform.position = currentStepTarget;
        }
    }
}