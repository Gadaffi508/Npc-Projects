using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RoborFireController : MonoBehaviour
{
    [Header("Fire")]
    public bool rightHandFire, leftHandFire, multiFire;

    public float sFireRate;

    [Header("Single Fire")]

    public Transform sFirePos;

    public GameObject singleBullet;

    [Header("Multi Fire")]

    public Transform[] mFirePos;

    public GameObject multiBullet;

    public bool fire = false;

    public TwoBoneIKConstraint constraint;

    SimpleController controller;

    float timer;

    private void Start()
    {
        controller = GetComponent<SimpleController>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (fire && timer > sFireRate)
        {
            controller.StopAgent();

            if (multiFire)
            {
                StartCoroutine(SingleFire());
                StartCoroutine(MultiFire());
            }
            else if (rightHandFire)
            {
                StartCoroutine(SingleFire());
                StartCoroutine(StopLeftHand());
            }
            else if (leftHandFire)
            {
                StartCoroutine(MultiFire());
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(StopLeftHand());
            }

                timer = 0;
        }
        else
        {
            controller.ContinueAgent();
        }
    }

    IEnumerator SingleFire()
    {
        yield return new WaitForSeconds(1f);
        GameObject simpleBullet = Instantiate(singleBullet, sFirePos.position, sFirePos.rotation);

        Vector3 direction = sFirePos.up;
        simpleBullet.GetComponent<SimpleBullet>().SetForce(direction);
    }

    IEnumerator MultiFire()
    {
        float ctimer = 0;

        while (ctimer < 1 && constraint.weight < 0.9f)
        {
            ctimer += 0.1f;
            yield return new WaitForSeconds(0.05f);
            constraint.weight = ctimer;
        }

        for (int i = 0; i < mFirePos.Length; i++)
        {
            GameObject simpleBullet = Instantiate(multiBullet, mFirePos[i].position, mFirePos[i].rotation);
            Vector3 direction = mFirePos[i].up;
            simpleBullet.GetComponent<SimpleBullet>().SetForce(direction);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator StopLeftHand()
    {
        float ctimer = 1;

        while (ctimer > 0.1f && constraint.weight > 0.2f)
        {
            ctimer -= 0.1f;
            yield return new WaitForSeconds(0.05f);
            constraint.weight = ctimer;
        }
    }
}
