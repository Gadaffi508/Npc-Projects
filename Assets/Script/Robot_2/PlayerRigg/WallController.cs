using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WallController : MonoBehaviour
{
    public Transform target;
    public LayerMask mask;           
    public float radius = 1f;        
    public float maxDistance = 1f;
    public float smoothSpeed = 5f;
    private float currentWeight = 0f;
    private TwoBoneIKConstraint tWbIkC;

    private void Start()
    {
        tWbIkC = GetComponent<TwoBoneIKConstraint>();
    }

    private void Update()
    {
        WallControllingHit();
    }

    private void WallControllingHit()
    {
        Collider[] hits = Physics.OverlapSphere(target.position, radius, mask);

        float targetWeight = 0f;

        if (hits.Length > 0)
        {
            float minDistance = float.MaxValue;
            foreach (Collider hit in hits)
            {
                float dist = Vector3.Distance(target.position, hit.ClosestPoint(target.position));
                if (dist < minDistance)
                    minDistance = dist;
            }

            targetWeight = 1f - Mathf.Clamp01(minDistance / maxDistance);
        }

        currentWeight = Mathf.Lerp(currentWeight, targetWeight, Time.deltaTime * smoothSpeed);
        tWbIkC.weight = currentWeight;
    }

    private void OnDrawGizmos()
    {
        if (target == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(target.position, radius);
    }
}
