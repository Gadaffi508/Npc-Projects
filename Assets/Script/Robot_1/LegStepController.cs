using System.Collections;
using UnityEngine;

public class LegStepController : MonoBehaviour
{
    public Robot_LegRotate leftLeg;
    public Robot_LegRotate rightLeg;
    public float stepTime = 0.5f;

    public bool isWalk = false;

    private void Start()
    {
        StartCoroutine(WalkCycle());
    }

    IEnumerator WalkCycle()
    {
        while (isWalk)
        {
            //ilk bekleme
            rightLeg.isLoopRotate = false;

            leftLeg.isLoopRotate = false;

            yield return new WaitForSeconds(stepTime);

            //Hareket
            rightLeg.isLoopRotate = true;

            leftLeg.isLoopRotate = true;

            yield return new WaitForSeconds(stepTime);
        }
    }
}
