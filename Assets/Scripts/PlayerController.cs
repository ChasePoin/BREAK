// This first example shows how to move using Input System Package (New)

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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
    private Vector2 movementInput = Vector2.zero;
    private Vector2 cameraInput = Vector2.zero;
    private float cameraPitch = 0f;
    private bool jumped = false;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    GameObject? BallHeldByPlayer;

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
        if (context.canceled)
            jumped = false;
    }

    // needs did I properly catch this logic and to assign BallHeldByPlayer if successful
    public void OnCatch(InputAction.CallbackContext context)
    {

        if (Physics.SphereCast(transform.position + controller.center, transform.position.y, transform.forward, out RaycastHit hit, 4) && BallHeldByPlayer == null)
        {
            GameObject ballHit = hit.transform.gameObject;
            if (ballHit.tag == "Ball")
            {
                BallHeldByPlayer = ballHit;
                ballHit.transform.position = transform.position + new Vector3(.5f, 0f, 0f);
                Debug.Log("Caught the ball.");
            }
            else
            {
                Debug.Log("Caught someting... It wasn't a ball!");
            }
        }
        else
        {
            Debug.Log("Did not Catch the ball.");
        }
        if (context.performed)
            Debug.Log("Catch the ball. down");
    }
    // needs to take transformations and effects applied to the ball and kick it off
    public void OnThrow(InputAction.CallbackContext context)
    {
        if (BallHeldByPlayer == null) return;
        Ball thisBall = BallHeldByPlayer.GetComponent<Ball>();
        thisBall.ThrownBy = gameObject; // set ball's thrown by to this player
        Rigidbody ballRigid = BallHeldByPlayer.GetComponent<Rigidbody>();
        ApplyTransformations(ballRigid, thisBall);
        BallHeldByPlayer = null;
       
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
        if (BallHeldByPlayer) BallHeldByPlayer.transform.position = transform.position + new Vector3(1f, 0f, 0f);
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
    }

    private void ApplyTransformations(Rigidbody ballRigid, Ball thisBall)
    {
        // apply gravity effect
        ballRigid.mass *= thisBall.GravityStrength;
        // ball throw based off of speed
        Vector3 throwDir = playerCamera.transform.forward + playerCamera.transform.up * 0.2f;
        ballRigid.AddForce(throwDir.normalized * thisBall.Speed, ForceMode.Impulse);
        // direction
        Vector3 curve = new Vector3(thisBall.DirectionStrength, 0f, 0f);
        ballRigid.AddForce(curve * Time.deltaTime, ForceMode.Force);
    }
}
