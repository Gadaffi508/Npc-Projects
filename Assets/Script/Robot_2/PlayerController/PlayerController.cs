using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour
{
    [Header("Player State")]
    public TextMeshProUGUI stateText;
    public PlayerState currentState;

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

    [Header("Climb Referances")]
    public TwoBoneIKConstraint leftIK;
    public TwoBoneIKConstraint rightIK;
    public Transform ArmTarget;
    public Transform climbCheckTransform;
    bool isClimbing;

    [Header("Climb Settings")]
    public float climbCheckDistance = 1f;
    public LayerMask climbableLayer;


    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerModelAnimator = GetComponentInChildren<Animator>();

        leftIK.weight = 0;
        rightIK.weight = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isDodging) return;

            if (CheckForClimbable(out Transform climbPoint))
            {
                StartCoroutine(Climb(climbPoint));
            }
            else
            {
                TryDodge();
            }
        }

        stateText.text = currentState.ToString();
    }


    private void FixedUpdate()
    {
        PlayerMove();
    }
    #region PlayerMove
    private void PlayerMove()
    {
        if (currentState == PlayerState.climbing) return;

        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = new Vector3(horizontalMove, 0f, verticalMove).normalized;

        if (isDodging)
        {
            currentState = PlayerState.Dodging;
            return;
        }

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

            currentState = Input.GetKey(KeyCode.LeftShift) ? PlayerState.Running : PlayerState.Walking;
        }
        else
        {
            playerRb.linearVelocity = new Vector3(0, playerRb.linearVelocity.y, 0);
            playerModelAnimator.SetFloat("speed", Mathf.Lerp(playerModelAnimator.GetFloat("speed"), 0f, Time.fixedDeltaTime * 5f));
            currentState = PlayerState.Idle;
        }
    }
    #endregion
    #region PlayerDodge
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
        currentState = PlayerState.Dodging;

        Vector3 startPosition = transform.position;
        Vector3 moveDirection = direction * moveDistance;
        float elapsed = 0f;

        playerModelAnimator.SetTrigger("roll");

        yield return new WaitForSeconds(0.05f);

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

        yield return new WaitForSeconds(.1f);

        isDodging = false;
        currentState = PlayerState.Idle;
    }

    #endregion


    public IEnumerator Climb(Transform climbSurface)
    {
        currentState = PlayerState.climbing;
        isClimbing = true;

        playerModelAnimator.SetBool("Climbing", true);

        playerRb.linearVelocity = Vector3.zero;
        playerRb.isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;

        float animationDuration = 3.5f;
        yield return new WaitForSeconds(animationDuration);

        ClimbingScript climbingScript = climbSurface.GetComponent<ClimbingScript>();

        if (climbingScript != null && climbingScript.climbTarget != null)
        {
            float moveDuration = 0.25f;
            float elapsed = 0f;
            Vector3 startPos = transform.position;
            Vector3 targetPos = climbingScript.climbTarget.position;

            while (elapsed < moveDuration)
            {
                transform.position = Vector3.Lerp(startPos, targetPos, elapsed / moveDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPos;
        }
        else
        {
            Debug.LogWarning("ClimbingScript veya climbTarget eksik!");
        }

        GetComponent<CapsuleCollider>().enabled = true;
        playerRb.isKinematic = false;

        playerModelAnimator.SetBool("Climbing", false);
        isClimbing = false;
        currentState = PlayerState.Idle;
    }


    private bool CheckForClimbable(out Transform climbTarget)
    {
        climbTarget = null;

        Vector3 origin = climbCheckTransform.position;
        Vector3 direction = climbCheckTransform.forward;

        Debug.DrawRay(origin, direction * climbCheckDistance, Color.green, 1f);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, climbCheckDistance, climbableLayer))
        {
            climbTarget = hit.transform;
            return true;
        }

        return false;
    }


}
public enum PlayerState
{
    Idle,
    Walking,
    Running,
    Dodging,
    climbing
}
