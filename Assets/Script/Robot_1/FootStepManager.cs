using UnityEngine;

public class FootStepManager : MonoBehaviour
{
    public static FootStepManager Instance;

    public bool isLeftFootStepping = false;
    public bool isRightFootStepping = false;

    public enum ActiveFoot { None, Left, Right }
    public ActiveFoot currentSteppingFoot = ActiveFoot.None;

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

        // Güncel adým atan ayaðý belirle
        if (stepping)
            currentSteppingFoot = isLeft ? ActiveFoot.Left : ActiveFoot.Right;
        else if (!isLeftFootStepping && !isRightFootStepping)
            currentSteppingFoot = ActiveFoot.None;
    }

    public bool IsAnyFootStepping()
    {
        return isLeftFootStepping || isRightFootStepping;
    }

}
