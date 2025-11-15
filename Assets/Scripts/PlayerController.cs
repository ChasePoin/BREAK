// This first example shows how to move using Input System Package (New)

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 5.0f;
    [SerializeField]
    private float jumpHeight = 1.5f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float yawSensitivity = 250.0f;
    [SerializeField]
    private float pitchSensitivity = 250.0f;
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private Animator animator;
    private Vector2 movementInput = Vector2.zero;
    private Vector2 cameraInput = Vector2.zero;
    private float cameraPitch = 0f;
    private bool jumped = false;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        cameraInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            jumped = true;
            animator.SetTrigger("Jump");
        if (context.canceled)
            jumped = false;
    }
    public void OnCatch(InputAction.CallbackContext context)
    {
        if (context.performed)
            Debug.Log("Catch the ball. down");
    }
    public void OnThrow(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            string control = context.control.name;
            Debug.Log($"Throw Ball using {control}");
        }
        if (context.canceled)
        {
            Debug.Log("Throw ball end");
        }
    }
    public void OnCardUse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int cardNumber = context.action.GetBindingIndexForControl(context.control);
            Debug.Log($"Use card #{cardNumber}");
        }
    }
    private void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = transform.forward * movementInput.y + transform.right * movementInput.x;
        move = Vector3.ClampMagnitude(move, 1f);

        if (move != Vector3.zero)
        {
            transform.position = move;
        }

        Vector3 rotatePlayer = new Vector3(0, cameraInput.x, 0);

        if (rotatePlayer != Vector3.zero)
        {
            transform.Rotate(rotatePlayer * Time.deltaTime * yawSensitivity);
        }

        if (cameraInput != Vector2.zero)
        {
            cameraPitch -= cameraInput.y * Time.deltaTime * pitchSensitivity;
            cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);
            playerCamera.transform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
        }

        // Jump
        if (jumped && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Combine horizontal and vertical movement
        Vector3 finalMove = (move * playerSpeed) + (playerVelocity.y * Vector3.up);
        controller.Move(finalMove * Time.deltaTime);

        animator.SetFloat("ForwardSpeed", Vector3.Dot(finalMove, transform.forward));
        animator.SetFloat("SideSpeed", Vector3.Dot(finalMove, transform.forward));
    }
}
