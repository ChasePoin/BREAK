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
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform leftHand;
    [SerializeField]
    private Transform rightHand;
    private bool ballInLeftHand = false;
    private Vector2 movementInput = Vector2.zero;
    private Vector2 cameraInput = Vector2.zero;
    private float cameraPitch = 0f;
    private bool jumped = false;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    GameObject BallHeldByPlayer;
    public bool startCharge = false;
    public float chargePower = 1f;

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
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
    // needs did I properly catch this logic and to assign BallHeldByPlayer if successful
    public void OnCatch(InputAction.CallbackContext context)
    {

        if (Physics.SphereCast(transform.position + controller.center, transform.position.y, playerCamera.transform.forward, out RaycastHit hit, 4) && BallHeldByPlayer == null)
        {
            GameObject ballHit = hit.transform.gameObject;
            if (ballHit.tag == "Ball")
            {
                BallHeldByPlayer = ballHit;
                ballHit.transform.position = rightHand.position; //transform.position + new Vector3(.5f, 0f, 0f);
                SphereCollider ballCollider = ballHit.GetComponent<SphereCollider>();
                CapsuleCollider playerCollider = GetComponent<CapsuleCollider>();
                if (ballCollider != null && playerCollider != null)
                {
                    Physics.IgnoreCollision(ballCollider, playerCollider, true);
                }
                GameObject previousPlayerGO = ballHit.GetComponent<Ball>().ThrownBy;
                if (ballCollider != null && previousPlayerGO != null)
                {
                    CapsuleCollider previousPlayerCollider = previousPlayerGO.GetComponent<CapsuleCollider>();
                    if (previousPlayerCollider != null) Physics.IgnoreCollision(ballCollider, previousPlayerCollider, false);
                }
                int heldBallLayer = LayerMask.NameToLayer("HeldBalls");
                ballHit.layer = heldBallLayer;
                
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
        Rigidbody ballRigid = BallHeldByPlayer.GetComponent<Rigidbody>();
        Ball thisBall = BallHeldByPlayer.GetComponent<Ball>();
        if (context.started)
        {
            startCharge = true;
            ballRigid.mass = thisBall.GravityStrength;
            ballRigid.useGravity = false;
            if (context.control.name == "rightTrigger")
            {
                animator.SetTrigger("StartThrow");
            }
            else
            {
                ballInLeftHand = true;
                animator.SetTrigger("StartThrowLeft");
            }
        }
        if (context.canceled)
        {
            // cache values BEFORE modifying/resetting anything
            Vector3 throwDir = playerCamera.transform.forward;
            float throwStrength = thisBall.Speed * 0.5f * chargePower;

            // throw impulse
            ballRigid.AddForce(throwDir * throwStrength, ForceMode.Impulse);

            // curve impulse (side + consistent upward arc)
            Vector3 curve = new Vector3(thisBall.DirectionStrength, 6f, 0f);
            ballRigid.AddForce(curve, ForceMode.Impulse);

            // cleanup
            ballRigid.angularDamping = 3f;
            ballRigid.useGravity = true;

            startCharge = false;
            chargePower = 0;

            // set layer AFTER throw
            BallHeldByPlayer.layer = LayerMask.NameToLayer("Balls");
            BallHeldByPlayer = null;

            // anim triggers
            if (context.control.name == "rightTrigger")
                animator.SetTrigger("EndThrow");
            else {
                ballInLeftHand = false;
                animator.SetTrigger("EndThrowLeft");
            }
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
        if (BallHeldByPlayer) 
            if (!ballInLeftHand) { BallHeldByPlayer.transform.position = rightHand.position; } else { BallHeldByPlayer.transform.position = leftHand.position; } //transform.position + new Vector3(1f, 0f, 0f);
        
        if (startCharge)
        {
            if (chargePower >= 1.2f && chargePower <= 2.0f)
            {
                chargePower += Time.deltaTime / 4f;
            }
            else if (chargePower <= 1.2f)
            {
                chargePower += Time.deltaTime / 2f;
            }
            else
            {
                chargePower = 2.0f;
            }
        }
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
        } else
        {
            animator.SetTrigger("Idle");
        }

        Vector3 rotatePlayer = new Vector3(0, cameraInput.x, 0);

        if (rotatePlayer != Vector3.zero)
        {
            transform.Rotate(rotatePlayer * Time.deltaTime * yawSensitivity);
        }

        if (cameraInput != Vector2.zero)
        {
            cameraPitch -= cameraInput.y * Time.deltaTime * pitchSensitivity;
            cameraPitch = Mathf.Clamp(cameraPitch, -25f, 90f);
            playerCamera.transform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
        }

        // Jump
        if (jumped && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
            animator.SetTrigger("Jump");
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Combine horizontal and vertical movement
        Vector3 finalMove = (move * playerSpeed) + (playerVelocity.y * Vector3.up);
        controller.Move(finalMove * Time.deltaTime);

        animator.SetFloat("ForwardSpeed", Vector3.Dot(finalMove, transform.forward));
        animator.SetFloat("SideSpeed", Vector3.Dot(finalMove, transform.right));
    }

    // private void ApplyTransformations(Rigidbody ballRigid, Ball thisBall, InputAction.CallbackContext context)
    // {
    //     // // apply gravity effect
    //     // ballRigid.mass *= thisBall.GravityStrength;
    //     // // ball throw based off of speed
    //     // Vector3 throwDir = playerCamera.transform.forward + playerCamera.transform.up * 0.2f;

    //     // // on release
    //     // if (context.canceled)
    //     // {
    //     //     ballRigid.AddForce(throwDir.normalized * thisBall.Speed * chargePower, ForceMode.Impulse);
    //     //     // direction
    //     //     Vector3 curve = new Vector3(thisBall.DirectionStrength, 0f, 0f);
    //     //     ballRigid.AddForce(curve * Time.deltaTime, ForceMode.Force);
    //     //     startCharge = false;
    //     //     chargePower = 0;
    //     }
}

