using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private GameObject boat;
    [SerializeField] private Transform axeTransform;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private Animator magicAnimator;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    private AudioSource audioSource;

    private string currentState;
    private string facingDir;

    private float originalSpeed;
    private float jumpSpeed;

    private string playerState;
    private bool isRunning;
    public bool isAttacking;

    private string currentMagic;

    private bool isRolling;
    private bool isCasting;


    private bool isGrounded;
    private int attackType;
    private float attackDelay = 1f;
    private float rollDelay = 1f;
    private float health = 1f;

    private const float KBForce = 6f;
    private float KBCounter;
    private const float KBTotalTime = 0.2f;

    private bool KBRight;

    private float deathTimer = 0.95f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = speed;
    }

    void Start()
    {
        playerState = "Idle";
        facingDir = "Right";
        jumpSpeed = speed * 2.5f;
        ChangeAnimationState("IdleAnim");
    }

    private void Update()
    {
        healthSlider.value = health;

        if (isGrounded == false && IsCurrentlyGrounded() == true)
        {
            audioSource.clip = audioClips[1];
            audioSource.Play();
            playerState = "Idle";
        }
        isGrounded = IsCurrentlyGrounded();

        if (health <= 0)
        {
            HandleDeath();
        }
        else if (m_gameManager.m_isPaused)
        {
            m_gameManager.PauseGame();
        }
        else
        {
            HandleAliveState();
        }
    }

    private void HandleDeath()
    {
        if (deathTimer <= 0)
        {
            m_gameManager.PlayerDead();
            ChangeAnimationState("Dead");
            playerState = "Dying";
        }
        else
        {
            deathTimer -= Time.deltaTime;
            ChangeAnimationState("Death");
            playerState = "Dying";
        }
    }

    private void HandleAliveState()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_gameManager.PauseGame();
        }

        if (!boat.activeInHierarchy)
        {
            HandleOnLand(horizontalInput);
        }
        else
        {
            HandleOnBoat(horizontalInput);
        }
    }

    private void HandleOnLand(float horizontalInput)
    {
        if (playerState == "Jumping" || playerState == "Movement" || playerState == "Idle")
        {
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        }

        HandleAttacks();
        HandleMagic();
        HandleMovementDirection(horizontalInput);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            speed = originalSpeed * 2;
            jumpSpeed = originalSpeed * 3.25f;
        }
        else
        {
            isRunning = false;
            speed = originalSpeed;
            jumpSpeed = originalSpeed * 2.5f;
        }

        HandleAnimations(horizontalInput);

        if (Input.GetKey(KeyCode.Space) && isGrounded && playerState != "Rolling")
        {
            Jump();
            playerState = "Jumping";
            audioSource.clip = audioClips[0];
            audioSource.Play();
        }

        HandleRolling();
    }

    private void HandleOnBoat(float horizontalInput)
    {
        isRolling = false;
        rollDelay = 1f;
        isAttacking = false;
        attackDelay = 0f;
        attackType = 0;
        speed = originalSpeed * 2;
        boxCollider2D.size = new Vector2(0.4088211f, 0.78f);
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        playerState = "Boating";

        if (horizontalInput == 0)
        {
            ChangeAnimationState("BoatIdle");
        }
        else
        {
            ChangeAnimationState("RowAnim");
        }

        HandleMovementDirection(horizontalInput);
    }

    private void HandleAttacks()
    {
        if (Input.GetMouseButtonDown(0) && isGrounded && playerState != "Attacking" && playerState.Equals("Idle"))
        {
            attackType = 2;
            isAttacking = true;
            axeTransform.eulerAngles = new Vector3(0, 0, 0);
            playerState = "Attacking";
        }
        else if (Input.GetMouseButtonDown(0) && isGrounded && playerState != "Attacking" && playerState != "Rolling")
        {
            attackType = 3;
            isAttacking = true;
            axeTransform.eulerAngles = new Vector3(0, 0, 0);
            playerState = "Attacking";
        }
        else if (Input.GetMouseButtonDown(1) && isGrounded && playerState != "Attacking" && playerState.Equals("Idle"))
        {
            attackType = 1;
            isAttacking = true;
            axeTransform.eulerAngles = new Vector3(0, 0, 0);
            playerState = "Attacking";
        }

        HandleAttackAnimations();
    }

    private void HandleMagic()
    {

    }
    private void HandleAnimations(float horizontalInput)
    {
        if(isAttacking)
        {
            ChangeAnimationState("AttackAnim" + attackType);
        }
        if (!isGrounded)
        {
            ChangeAnimationState("JumpAnim");
            animator.Play("JumpAnim");
        }
        else if (horizontalInput != 0 && isRunning && (playerState == "Movement" || playerState == "Idle") && isGrounded)
        {
            ChangeAnimationState("RunningAnim");
            playerState = "Movement";
        }
        else if (horizontalInput != 0 && (playerState == "Movement" || playerState == "Idle") && isGrounded)
        {
            ChangeAnimationState("WalkAnim");
            playerState = "Movement";
        }
        else if (horizontalInput == 0 && (playerState == "Movement" || playerState == "Idle") && isGrounded)
        {
            ChangeAnimationState("IdleAnim");
            playerState = "Idle";
        }
    }

    private void HandleMovementDirection(float horizontalInput)
    {
        if (horizontalInput > 0.001f && !isAttacking && facingDir == "Left")
        {
            transform.localScale = Vector3.one;
            facingDir = "Right";
        }
        else if (horizontalInput < -0.001f && !isAttacking && facingDir == "Right")
        {
            transform.localScale = new Vector3(-1, 1, 1);
            facingDir = "Left";
        }
    }

    private void HandleRolling()
    {
        if (!isRolling && !isAttacking && isGrounded && Input.GetKeyDown(KeyCode.LeftControl) && !boat.activeInHierarchy)
        {
            isRolling = true;
            ChangeAnimationState("RollAnim");
            playerState = "Rolling";
            rb.velocity = new Vector2(transform.localScale.x * originalSpeed * 2, rb.velocity.y);
        }
        if (isRolling && rollDelay > 0)
        {
            rollDelay -= Time.deltaTime;
        }
        else if (isRolling && rollDelay <= 0)
        {
            isRolling = false;
            rollDelay = 1f;
            playerState = "Idle";
        }
    }

    private void HandleAttackAnimations()
    {
        if (isAttacking && attackDelay > 0)
        {
            attackDelay -= Time.deltaTime;
            axeTransform.eulerAngles = new Vector3(axeTransform.eulerAngles.x, axeTransform.eulerAngles.y, axeTransform.eulerAngles.z + Time.deltaTime * -180 * transform.localScale.x);
        }
        else if (isAttacking && attackDelay <= 0)
        {
            isAttacking = false;
            attackType = 0;
            attackDelay = 1f;
            playerState = "Idle";
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            ChangeAnimationState("JumpAnim");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            boat.SetActive(false);
            playerState = "Idle";
        }
        if (collision.gameObject.layer == 4)
        {
            boat.SetActive(true);
        }
        else
        {
            boat.SetActive(false);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 4)
        {
            boat.SetActive(true);
        }
        else
        {
            boat.SetActive(false);
        }
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {

        }
        else
        {
            animator.Play(newState);

            currentState = newState;
        }
    }

    public void Knockback(bool onRight)
    {
        if (!isAttacking && !isRolling)
        {
            KBCounter = KBTotalTime;
            KBRight = onRight;
        }
    }

    public void TakeDamage(float damage)
    {
        if (!isAttacking && !isRolling)
        {
            ChangeAnimationState("HurtAnim");
            playerState = "Hurt";
            health -= damage;
            Debug.Log(health);
        }
    }

    private bool IsCurrentlyGrounded()
    {
        float raycastLength = 0.5f;

        RaycastHit2D hit = Physics2D.Raycast(boxCollider2D.bounds.center, Vector2.down, raycastLength, jumpableGround);

        return hit.collider != null;
    }

    public void ChooseMagic(string magicType)
    {
        currentMagic = magicType;  
    }
}
