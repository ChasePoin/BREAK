// This first example shows how to move using Input System Package (New)

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 7.5f;
    [SerializeField]
    private float jumpHeight = 1.5f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float yawSensitivity = 250.0f;
    [SerializeField]
    private float pitchSensitivity = 250.0f;
    [SerializeField]
    public Camera playerCamera;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform leftHand;
    [SerializeField]
    private Transform rightHand;
    [SerializeField]
    private HUDController hud;
    [SerializeField]
    private List<Card> cards;
    [SerializeField]
    private int maxCards = 3;
    [SerializeField]
    private List<GameObject> possibleCards;
    public SkinnedMeshRenderer playerMesh;
    private bool ballInLeftHand = false;
    private Vector2 movementInput = Vector2.zero;
    private Vector2 cameraInput = Vector2.zero;
    private float cameraPitch = 0f;
    private bool jumped = false;
    private float speedModifier = 1f;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    GameObject BallHeldByPlayer;
    public GameObject shield;
    public bool startCharge = false;
    public float chargePower = 0f;
    public float reachRadius = 2f;
    public float reachRange = 2f;
    public float maxCharge = 2f;
    public float mediumCharge = 1.2f;
    public int playerId = 0;
    public bool blocking = false;

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
        if (Physics.SphereCast(playerCamera.transform.position, reachRadius, playerCamera.transform.forward, out RaycastHit hit, reachRange))
        {
            if (BallHeldByPlayer == null) {
                GameObject ballHit = hit.transform.gameObject;
                if (ballHit.tag == "Ball")
                {
                    if (!ballHit.GetComponent<Ball>().catchable) {Debug.Log("Ball is uncatchable!"); return;}
                    BallHeldByPlayer = ballHit;
                    ballHit.transform.position = rightHand.position; //transform.position + new Vector3(.5f, 0f, 0f);
                    SphereCollider ballCollider = ballHit.GetComponent<SphereCollider>();
                    CapsuleCollider playerCollider = GetComponent<CapsuleCollider>();
                    if (ballCollider != null && playerCollider != null)
                    {
                        Physics.IgnoreCollision(ballCollider, playerCollider, true);
                    }
                    GameObject previousPlayerGO = ballHit.GetComponent<Ball>().ThrownBy;
                    if (ballCollider != null && previousPlayerGO != null && previousPlayerGO != gameObject)
                    {
                        PlayerController previousPlayerController = previousPlayerGO.GetComponent<PlayerController>();
                        previousPlayerController.hud.ball.enabled = false;
                        previousPlayerController.BallHeldByPlayer = null;
                        CapsuleCollider previousPlayerCollider = previousPlayerGO.GetComponent<CapsuleCollider>();
                        chargePower = maxCharge;
                        if (previousPlayerCollider != null) Physics.IgnoreCollision(ballCollider, previousPlayerCollider, false);
                    }
                    ballHit.layer = LayerMask.NameToLayer($"Player{playerId}");
                    hud.ball.enabled = true;
                    Debug.Log("Caught the ball.");
                }
                else
                {
                    Debug.Log("Caught someting... It wasn't a ball!");
                }
            }
            else
            {
                GameObject ballBlocked = hit.transform.gameObject;
                if (ballBlocked.tag == "Ball")
                {
                    ballBlocked.GetComponent<Rigidbody>().linearVelocity *= -1;
                    ballBlocked.GetComponent<Ball>().ThrownBy = gameObject;
                }
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
        Vector3 throwDir = playerCamera.transform.forward;
        throwDir.Normalize();

        float throwStrength = thisBall.Speed * 0.5f + chargePower;
        
        thisBall.transform.position = playerCamera.transform.position + playerCamera.transform.forward * 1.5f;

        ballRigid.constraints = RigidbodyConstraints.FreezePositionY;

        // float upward = (chargePower <= mediumCharge) ? 4f : 2f;
        ballRigid.AddForce(throwDir * throwStrength, ForceMode.Impulse);

        
        // ballRigid.AddForce(Vector3.up * upward, ForceMode.Impulse);

    
        ballRigid.useGravity = true;
        ballRigid.constraints = RigidbodyConstraints.None;
        thisBall.ThrownBy = gameObject;

        startCharge = false;
        chargePower = 0;
        BallHeldByPlayer.layer = LayerMask.NameToLayer("Balls");
        BallHeldByPlayer = null;

        // anim triggers
        if (context.control.name == "rightTrigger")
        {
            animator.SetTrigger("EndThrow");
        }
        else 
        {
            ballInLeftHand = false;
            animator.SetTrigger("EndThrowLeft");
        }
        hud.ball.enabled = false;
        }
       
    }
    public void OnCardUse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int cardNumber = context.action.GetBindingIndexForControl(context.control);
            if (hud.UseCard(cardNumber))
            {
                if (!cards[cardNumber]) return;
                switch (cards[cardNumber].Type)
                {
                    case CardTypes.Terrain:
                        cards[cardNumber].UseCard(playerToApplyTo: this);
                        break;
                    case CardTypes.Ball:
                        cards[cardNumber].UseCard(ballToApplyTo: BallHeldByPlayer.GetComponent<Ball>());
                        break;
                    case CardTypes.Player:
                        cards[cardNumber].UseCard(playerToApplyTo: this);
                        break;
                    default:
                        break;
                }
                hud.DeleteCardImage(cardNumber);
                cards[cardNumber] = null;
                AudioController.PlayClip("use-card");
            }
            else
            {
                AudioController.PlayClip("ready-card");
            }
            Debug.Log($"Use card #{cardNumber}");
        }
    }
    private void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
        GenerateRandomCards();

        int i = 0;
        foreach (Card card in cards)
        {
            hud.SetCardImage(i, card.CardSprite);
            i++;
            if (i > 2) break;
        }
        
    }
    private void Start()
    {
        hud.SetBarColors(playerId);
    }
    public void ModifySpeed(float multiplier, float seconds)
    {
        StartCoroutine(ModifySpeedIE(multiplier, seconds));
    }
    public void BlockBalls(float seconds)
    {
        StartCoroutine(BlockBallsIE(seconds));
    }
    public IEnumerator BlockBallsIE(float seconds)
    {
        blocking = true;
        GameObject shieldInstance = Instantiate(shield, transform);
        yield return new WaitForSeconds(seconds);
        Destroy(shieldInstance);
        blocking = false;
    }
    public IEnumerator ModifySpeedIE(float multiplier, float seconds)
    {
        Debug.Log($"Multiplying the speed by {multiplier} for {seconds} seconds.");
        speedModifier *= multiplier;

        yield return new WaitForSeconds(seconds);

        speedModifier /= multiplier;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hi");
        if (blocking) { blocking = false; return; }
        GameObject whatEntered = collision.gameObject;
        Ball thisBall = whatEntered.GetComponent<Ball>();
        // check if a ball entered
        if (thisBall == null) { Debug.Log("Not a Ball Entered"); return; }
        if (thisBall.ThrownBy != gameObject && thisBall.ThrownBy != null)
        {
            PlayerController otherPlayer = thisBall.ThrownBy.GetComponent<PlayerController>();
            GameManager.gm.players[otherPlayer.playerId] += 1; // they get a point!
            GameManager.gm.aliveStatus[playerId] = false;
            Debug.Log("player " + otherPlayer.playerId + " scored a point. Total points: " + GameManager.gm.players[otherPlayer.playerId]);
            Debug.Log("player " + playerId + " got tagged by a ball! Alive Status: " + GameManager.gm.aliveStatus[playerId]);
            playerCamera.enabled = false;
            thisBall.ThrownBy = null;
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (BallHeldByPlayer) 
            if (!ballInLeftHand) { BallHeldByPlayer.transform.position = rightHand.position; } else { BallHeldByPlayer.transform.position = leftHand.position; } //transform.position + new Vector3(1f, 0f, 0f);
        
        if (startCharge)
        {
            if (chargePower >= mediumCharge && chargePower <= maxCharge)
            {
                chargePower += Time.deltaTime / 4f;
            }
            else if (chargePower <= mediumCharge)
            {
                chargePower += Time.deltaTime / maxCharge;
            }
            else
            {
                chargePower = maxCharge;
            }
        }
        hud.chargeMeter.value = chargePower / maxCharge;
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
            cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);
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
        Vector3 finalMove = (move * playerSpeed * speedModifier) + (playerVelocity.y * Vector3.up);
        controller.Move(finalMove * Time.deltaTime);

        animator.SetFloat("ForwardSpeed", Vector3.Dot(finalMove, transform.forward));
        animator.SetFloat("SideSpeed", Vector3.Dot(finalMove, transform.right));
    }

    public void GenerateRandomCards()
    {
        for (int i = 0; i < maxCards; ++i)
        {
            cards.Add(possibleCards[Random.Range(0, possibleCards.Count)].GetComponent<Card>());
            // Debug.Log("choosing: " + possibleCards[Random.Range(0, possibleCards.Count)].GetComponent<Card>());
        }
    }

    public void GenerateRandomSingleCard()
    {
        Debug.Log("inside player controller");
        for(int i = 0; i < maxCards; i++)
        {
           if (cards[i]==null)
            {

                cards[i] = possibleCards[Random.Range(0, possibleCards.Count)].GetComponent<Card>();
                hud.SetCardImage(i, cards[i].CardSprite);
                Debug.Log("choosing single: " + possibleCards[Random.Range(0, possibleCards.Count)].GetComponent<Card>());
                return;
            } 
        }
        
    }
}

