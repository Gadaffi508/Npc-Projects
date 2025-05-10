using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 10f;

    [Header("References")]
    public Transform playerModel;
    public Transform cameraTransform;

    private Rigidbody playerRb;
    private Animator playerModelAnimator;

    private float horizontalMove;
    private float verticalMove;
    private float currentSpeed;

    [Header("Curve Dodge")]
    public AnimationCurve movementCurve;
    public float duration = 1f;
    public float moveDistance = 5f;

    [Header("Dodge Control")]
    public float dodgeCooldown = 0.5f;
    private bool isDodging = false;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerModelAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDodging)
        {
            TryDodge();
        }
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = new Vector3(horizontalMove, 0f, verticalMove).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.fixedDeltaTime * 5f);

            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = camForward * inputDirection.z + camRight * inputDirection.x;

            playerRb.linearVelocity = moveDirection * currentSpeed + new Vector3(0, playerRb.linearVelocity.y, 0);

            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            playerModel.rotation = Quaternion.Lerp(playerModel.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);

            float animSpeed = Input.GetKey(KeyCode.LeftShift) ? 1f : 0.5f;
            playerModelAnimator.SetFloat("speed", Mathf.Lerp(playerModelAnimator.GetFloat("speed"), animSpeed, Time.fixedDeltaTime * 5f));
        }
        else
        {
            playerRb.linearVelocity = new Vector3(0, playerRb.linearVelocity.y, 0);
            playerModelAnimator.SetFloat("speed", Mathf.Lerp(playerModelAnimator.GetFloat("speed"), 0f, Time.fixedDeltaTime * 5f));
        }
    }

    private void TryDodge()
    {
        if (isDodging) return;

        Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (inputDir.magnitude == 0f) return;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 dodgeDirection = camForward * inputDir.z + camRight * inputDir.x;
        dodgeDirection.Normalize();

        StartCoroutine(MoveWithCurve(dodgeDirection));
    }


    private IEnumerator MoveWithCurve(Vector3 direction)
    {
        isDodging = true;

        Vector3 startPosition = transform.position;
        Vector3 moveDirection = direction * moveDistance;
        float elapsed = 0f;

        playerModelAnimator.SetTrigger("roll");

        yield return new WaitForSeconds(0.2f); 

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            playerModel.rotation = toRotation;
        }

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float curveValue = movementCurve.Evaluate(t);

            transform.position = startPosition + moveDirection * curveValue;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition + moveDirection;

        yield return new WaitForSeconds(dodgeCooldown);

        isDodging = false;
    }

}
