using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 10f;

    public Transform playerModel;
    public Transform cameraTransform;

    private Rigidbody playerRb;
    private Animator playerModelAnimator;

    private float horizontalMove;
    private float verticalMove;
    private float currentSpeed; 

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerModelAnimator = GetComponentInChildren<Animator>();
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
}
