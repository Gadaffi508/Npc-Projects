using UnityEngine;
using UnityEngine.AI;

public class LegAimGround : MonoBehaviour
{
    GameObject raycastOrigin;

    public LayerMask mask;

    public float offsetY, offsetZ;

    public Vector3 oroginPos;

    Vector3 offset, offsetYv;

    private void Start()
    {
        raycastOrigin = transform.parent.gameObject;

        offset = new Vector3(0f, 0f, offsetZ);
        offsetYv = new Vector3(0f, offsetY, 0f);
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(raycastOrigin.transform.position, -transform.up, out hit, Mathf.Infinity, mask))
        {
            oroginPos = hit.point + offsetYv;
            transform.position = oroginPos + offset;
        }
    }
}