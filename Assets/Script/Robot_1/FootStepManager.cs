using UnityEngine;

public class FootStepManager : MonoBehaviour
{
    public static FootStepManager Instance;

    public bool isLeftFootStepping = false;
    public bool isRightFootStepping = false;

    private void Awake()
    {
        Instance = this;
    }

    public bool CanStep(bool isLeft)
    {
        if (isLeft)
            return !isRightFootStepping;
        else
            return !isLeftFootStepping;
    }

    public void SetFootStepping(bool isLeft, bool stepping)
    {
        if (isLeft)
            isLeftFootStepping = stepping;
        else
            isRightFootStepping = stepping;
    }
}
