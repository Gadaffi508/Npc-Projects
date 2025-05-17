using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.1f;

    private Vector3 originalPos;
    private float shakeTimeRemaining = 0f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        originalPos = transform.position;
    }

    private void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            transform.position = originalPos + Random.insideUnitSphere * shakeMagnitude;
            shakeTimeRemaining -= Time.deltaTime;

            if (shakeTimeRemaining <= 0f)
            {
                transform.position = originalPos;
            }
        }
    }

    public void Shake()
    {
        Debug.Log("CameraShake Triggered");
        shakeTimeRemaining = shakeDuration;
    }
}