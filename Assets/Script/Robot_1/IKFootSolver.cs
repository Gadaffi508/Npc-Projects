using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    public Transform body;
    public float footSpacing;
    public float stepDistance;
    public float stepHeight;
    public float speed;
    public LayerMask terrainLayer;
    public bool isLeftFoot = true;

    float lerp = 1f;
    Vector3 currentPos, newPos, oldPos;

    private float baseBodyY; // Body'nin baþlangýç Y yüksekliði
    public float bodyBounceHeight = 0.1f; // Ne kadar yukarý çýkacak


    private Vector3 lastBodyPosition;
    private Vector3 velocity;

    private void Start()
    {
        currentPos = transform.position;
        oldPos = transform.position;
        newPos = transform.position;

        lastBodyPosition = body.position;

        baseBodyY = body.position.y;

    }

    private void Update()
    {
        // Yatay (X-Z düzlemi) hýz vektörünü hesapla
        velocity = (body.position - lastBodyPosition) / Time.deltaTime;
        velocity.y = 0;
        lastBodyPosition = body.position;

        transform.position = currentPos;

        Vector3 stepDirection = velocity.normalized;
        if (stepDirection == Vector3.zero)
            stepDirection = body.forward; // Hareketsizse varsayýlan yön

        Vector3 rayOrigin = body.position + (body.right * footSpacing) + stepDirection * 4;

        Ray ray = new Ray(rayOrigin, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 10, terrainLayer.value))
        {
            if (Vector3.Distance(newPos, hit.point) > stepDistance && lerp >= 1f)
            {
                if (FootStepManager.Instance.CanStep(isLeftFoot))
                {
                    lerp = 0;
                    newPos = hit.point;
                    FootStepManager.Instance.SetFootStepping(isLeftFoot, true);
                    CameraShake.Instance.Shake();
                }
            }
        }

        if (lerp < 1)
        {
            Vector3 footPos = Vector3.Lerp(oldPos, newPos, lerp);
            footPos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPos = footPos;
            lerp += Time.deltaTime * speed;

            // Sadece bir ayak (örnek: sol ayak) body bounce yapsýn
            if ((isLeftFoot && FootStepManager.Instance.currentSteppingFoot == FootStepManager.ActiveFoot.Left) ||
    (!isLeftFoot && FootStepManager.Instance.currentSteppingFoot == FootStepManager.ActiveFoot.Right))

            {
                float bounceOffset = Mathf.Sin(lerp * Mathf.PI) * bodyBounceHeight;
                Vector3 bodyPos = body.position;
                bodyPos.y = baseBodyY + bounceOffset;
                body.position = bodyPos;
            }
        }
        else
        {
            if (FootStepManager.Instance != null)
                FootStepManager.Instance.SetFootStepping(isLeftFoot, false);

            // Sadece bir ayak resetlesin (örnek: sol ayak)
            if ((isLeftFoot && FootStepManager.Instance.currentSteppingFoot == FootStepManager.ActiveFoot.Left) ||
    (!isLeftFoot && FootStepManager.Instance.currentSteppingFoot == FootStepManager.ActiveFoot.Right))
            {
                Vector3 bodyPos = body.position;
                bodyPos.y = baseBodyY;
                body.position = bodyPos;
            }


            oldPos = newPos;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(newPos, 0.2f);
    }
}
