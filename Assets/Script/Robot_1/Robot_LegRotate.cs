using UnityEngine;

public class Robot_LegRotate : IRobot_IK
{
    [Header("Leg Direction Reference")]
    public bool ýsRightOrLeftLeg = false;

    private float currentY;
    private int direction = 1;

    public override void IK_Setup()
    {
        currentY = defaultY;
        if (rotateTransform != null)
        {
            Vector3 angles = rotateTransform.localEulerAngles;
            rotateTransform.localRotation = Quaternion.Euler(angles.x, defaultY, angles.z);
        }
    }

    public override void Rotate_IK()
    {
        if (rotateTransform == null) return;

        if (isLoopRotate)
        {
            currentY += rotationSpeed * Time.deltaTime * direction;

            if (currentY >= maxY)
            {
                currentY = maxY;
                direction = -1;
            }
            else if (currentY <= minY)
            {
                currentY = minY;
                direction = 1;
            }

            Vector3 angles = rotateTransform.localEulerAngles;
            rotateTransform.localRotation = Quaternion.Euler(angles.x, currentY, angles.z);
        }
        else
        {
            currentY = Mathf.MoveTowardsAngle(currentY, defaultY, rotationSpeed * Time.deltaTime);

            Vector3 angles = rotateTransform.localEulerAngles;
            rotateTransform.localRotation = Quaternion.Euler(angles.x, currentY, angles.z);
        }
    }
}
