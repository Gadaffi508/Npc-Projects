using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    public Transform body;
    public float footSpacing;
    public float stepDistance;
    public float stepHeight;
    public float speed;
    public LayerMask terrainLayer;
    public bool isLeftFoot = true; // Bu ayak sol mu?

    float lerp = 1f;
    Vector3 currentPos, newPos, oldPos;

    private void Start()
    {
        currentPos = transform.position;
        oldPos = transform.position;
        newPos = transform.position;
    }

    private void Update()
    {
        transform.position = currentPos;

        Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 10, terrainLayer.value))
        {
            if (Vector3.Distance(newPos, hit.point) > stepDistance && lerp >= 1f)
            {
                if (FootStepManager.Instance.CanStep(isLeftFoot))
                {
                    lerp = 0;
                    newPos = hit.point;
                    FootStepManager.Instance.SetFootStepping(isLeftFoot, true);
                }
            }
        }

        if (lerp < 1)
        {
            Vector3 footPos = Vector3.Lerp(oldPos, newPos, lerp);
            footPos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPos = footPos;
            lerp += Time.deltaTime * speed;
        }
        else
        {
            if (FootStepManager.Instance != null)
                FootStepManager.Instance.SetFootStepping(isLeftFoot, false);

            oldPos = newPos;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(newPos, 0.2f);
    }
}
