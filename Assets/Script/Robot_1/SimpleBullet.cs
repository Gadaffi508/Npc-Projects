using UnityEngine;

public class SimpleBullet : MonoBehaviour
{
    public GameObject effect;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetForce(Vector3 direction)
    {
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(direction.normalized * 100f, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        CameraShake.Instance.Shake();
        Instantiate(effect,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
}
