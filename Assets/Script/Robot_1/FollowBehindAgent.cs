using UnityEngine;

public class FollowBehindAgent : MonoBehaviour
{
    public Transform agentRoot;
    public float distanceBehind = 1.0f;
    public float heightOffset = 0.0f;
    public float lerpSpeed = 5f;

    void LateUpdate()
    {
        Vector3 behindPosition = agentRoot.position - agentRoot.forward * distanceBehind;
        behindPosition.y += heightOffset;

        transform.position = Vector3.Lerp(transform.position, behindPosition, Time.deltaTime * lerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, agentRoot.rotation, Time.deltaTime * lerpSpeed);
    }
}