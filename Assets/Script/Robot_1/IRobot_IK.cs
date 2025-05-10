using UnityEngine;

public abstract class IRobot_IK : MonoBehaviour
{
    [Header("Transform Reference")]
    public Transform rotateTransform;

    [Header("Value Reference")]
    public float rotationSpeed;

    public bool isLoopRotate;

    [Header("Default Value Reference")]
    public float minY = -150f;
    public float maxY = -70f;
    public float defaultY = -110.141f;

    void Start()
    {
        IK_Setup();
    }

    void Update()
    {
        Rotate_IK();
    }

    public abstract void IK_Setup();

    public abstract void Rotate_IK();
}
