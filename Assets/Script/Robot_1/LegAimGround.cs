using UnityEngine;

public class LegAimGround : MonoBehaviour
{
    GameObject raycastOrigin;

    public LayerMask mask;

    private void Start()
    {
        raycastOrigin = transform.parent.gameObject;
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(raycastOrigin.transform.position, -transform.up,out hit,Mathf.Infinity, mask))
        {
            transform.position = hit.point;
        }
    }
}
