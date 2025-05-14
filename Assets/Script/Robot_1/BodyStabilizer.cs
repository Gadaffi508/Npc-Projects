using UnityEngine;

public class BodyStabilizer : MonoBehaviour
{
    public Transform modelRoot;
    public float lerpSpeed = 5f;

    void LateUpdate()
    {
        modelRoot.position = Vector3.Lerp(modelRoot.position, transform.position, Time.deltaTime * lerpSpeed);
    }
}