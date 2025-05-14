using UnityEngine;
using UnityEngine.AI;

public class LegAimGround : MonoBehaviour
{
    public NavMeshAgent agent;
    public LayerMask mask;

    public float offsetY = 0.1f;
    public float forwardOffset = 0.5f;
    public float backwardOffset = -0.5f;
    public float lateralOffset = 0.3f; // YEN�: Sa�a/sola kayd�rma miktar�

    public bool isRightLeg = false; // TRUE ise sa� bacak

    public Vector3 oroginPos;

    private void Update()
    {
        Vector3 moveDirection = agent.velocity.normalized;
        if (moveDirection.sqrMagnitude < 0.01f) return;

        Vector3 forward = agent.transform.forward;
        Vector3 right = agent.transform.right;

        float dot = Vector3.Dot(forward, moveDirection);
        float dynamicForwardOffset = Mathf.Lerp(backwardOffset, forwardOffset, (dot + 1f) / 2f);
        float side = isRightLeg ? 1f : -1f;
        Vector3 lateral = right * lateralOffset * side;

        Vector3 rayStart = agent.transform.position + forward * dynamicForwardOffset + lateral + Vector3.up * 1f;

        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, Mathf.Infinity, mask))
        {
            oroginPos = hit.point + Vector3.up * offsetY;
            transform.position = oroginPos;
        }
    }
}