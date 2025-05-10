using UnityEngine;

public class Robot_LegRotate : IRobot_IK
{
    private float currentY;
    private int direction = 1;

    public override void IK_Setup()
    {
        rotateTransform.rotation = Quaternion.Euler(0f, defaultY, 0f);
        currentY = defaultY;
    }

    public override void Rotate_IK()
    {
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

            rotateTransform.rotation = Quaternion.Euler(0f, currentY, 0f);
        }
        else
        {
            currentY = Mathf.MoveTowardsAngle(currentY, defaultY, rotationSpeed * Time.deltaTime);
            rotateTransform.rotation = Quaternion.Euler(0f, currentY, 0f);
        }
    }
}
