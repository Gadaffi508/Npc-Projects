using UnityEngine;

public class Robot_KneeRotate : IRobot_IK
{
    private float currentX;
    private int direction = 1;

    public override void IK_Setup()
    {
        currentX = defaultY;

        if (rotateTransform != null)
        {
            Vector3 angles = rotateTransform.localEulerAngles;
            angles.x = defaultY;
            rotateTransform.localRotation = Quaternion.Euler(angles);
        }
    }

    public override void Rotate_IK()
    {
        if (rotateTransform == null) return;

        Vector3 angles = rotateTransform.localEulerAngles;

        if (isLoopRotate)
        {
            currentX += rotationSpeed * Time.deltaTime * direction;

            if (currentX >= maxY)
            {
                currentX = maxY;
                direction = -1;
            }
            else if (currentX <= minY)
            {
                currentX = minY;
                direction = 1;
            }

            angles.x = currentX;
            rotateTransform.localRotation = Quaternion.Euler(angles);
        }
        else
        {
            currentX = Mathf.MoveTowardsAngle(currentX, defaultY, rotationSpeed * Time.deltaTime);
            angles.x = currentX;
            rotateTransform.localRotation = Quaternion.Euler(angles);
        }
    }

}
