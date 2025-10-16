using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine; 

[RequireComponent(typeof(CharacterController))]
public class PlayerControllerV2 : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraFollowTarget; 
    [SerializeField] private CinemachineCamera playerCamera; 

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float gravityValue = -9.81f;
    public float rotationSmoothTime = 0.1f;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float turnSmoothVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Ground check
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -0.5f;

        // Input
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0, input.y).normalized;

        // Move relative to camera
        if (direction.magnitude >= 0.1f)
        {

              float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                 cameraFollowTarget.eulerAngles.y;

             float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                  ref turnSmoothVelocity, rotationSmoothTime);

             transform.rotation = Quaternion.Euler(0, angle, 0);

            cameraFollowTarget.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }

        // Jump
        if (jumpAction.action.triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

        // Gravity
        velocity.y += gravityValue * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
