using System.Collections;
using UnityEngine;

public class LegStepController : MonoBehaviour
{
    public Robot_LegRotate leftLeg;
    public Robot_LegRotate rightLeg;
    public float stepDuration = 0.5f;
    public float stepPause = 0.2f;

    private void Start()
    {
        StartCoroutine(WalkRoutine());
    }

    IEnumerator WalkRoutine()
    {
        while (true)
        {
            leftLeg.isLoopRotate = true;
            rightLeg.isLoopRotate = false;
            yield return new WaitForSeconds(stepDuration);
            leftLeg.isLoopRotate = false;

            yield return new WaitForSeconds(stepPause);

            rightLeg.isLoopRotate = true;
            leftLeg.isLoopRotate = false;
            yield return new WaitForSeconds(stepDuration);
            rightLeg.isLoopRotate = false;

            yield return new WaitForSeconds(stepPause);
        }
    }
}
