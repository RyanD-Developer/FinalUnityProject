using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform[] patrolPoints;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage = 0.05f;
    [SerializeField] private int patrolDestination;
    [SerializeField] private float health = 1f;

    public Transform playerTransform;
    [SerializeField] bool isChasing;
    [SerializeField] private float chaseDistance;
    [SerializeField] PlayerController playerController;
    [SerializeField] private GameManager m_gameManager;

    private float KBForce = 6f;
    private float KBCounter;
    private float KBTotalTime = 0.2f;

    private bool KBRight;

    [SerializeField] private float attackDelay;
    [SerializeField] private float deathTimer;
    private float currentDelay;
    private bool isAttacking;

    private string currentState;
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (m_gameManager.m_isDead)
        {
            ChangeAnimationState("Idle");
        }
        else
        {
            if (KBCounter <= 0)
            {
                if (health <= 0)
                {
                    ChangeAnimationState("Death");
                    if (deathTimer <= 0)
                    {
                        Object.Destroy(this.gameObject);
                    }
                    else
                    {
                        deathTimer -= Time.deltaTime;
                    }
                }
                else
                {
                    if (!isAttacking)
                    {
                        if (isChasing)
                        {
                            if (transform.position.x > playerTransform.position.x)
                            {
                                ChangeAnimationState("Walk");
                                transform.localScale = new Vector3(1, 1, 1);
                                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                            }
                            if (transform.position.x < playerTransform.position.x)
                            {
                                ChangeAnimationState("Walk");
                                transform.localScale = new Vector3(-1, 1, 1);
                                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                            }
                        }
                        else
                        {
                            if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
                            {
                                isChasing = true;
                            }
                            if (patrolDestination == 0)
                            {
                                ChangeAnimationState("Walk");
                                transform.position = Vector2.MoveTowards(transform.position, new Vector2(patrolPoints[0].position.x, transform.position.y), moveSpeed * Time.deltaTime);
                                if (Vector2.Distance(transform.position, patrolPoints[0].position) < .2f)
                                {
                                    transform.localScale = new Vector3(1, 1, 1);
                                    patrolDestination = 1;
                                }
                            }
                            else if (patrolDestination == 1)
                            {
                                ChangeAnimationState("Walk");
                                transform.position = Vector2.MoveTowards(transform.position, new Vector2(patrolPoints[1].position.x, transform.position.y), moveSpeed * Time.deltaTime);
                                if (Vector2.Distance(transform.position, patrolPoints[1].position) < .2f)
                                {
                                    transform.localScale = new Vector3(-1, 1, 1);
                                    patrolDestination = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        ChangeAnimationState("Attack");
                        currentDelay -= Time.deltaTime;
                        if (currentDelay <= 0)
                        {
                            currentDelay = attackDelay;
                            ChangeAnimationState("Walk");
                            isAttacking = false;
                        }
                    }
                }
            }
            else
            {
                ChangeAnimationState("Hurt");
                if (KBRight == true)
                {
                    rb.velocity = new Vector2(-KBForce, KBForce);
                }
                else if (!KBRight)
                {
                    rb.velocity = new Vector2(KBForce, KBForce);
                }
                KBCounter -= Time.deltaTime;
            }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_gameManager.m_isDead)
        {

        }
        else
        {
            if (collision.gameObject.tag == "Player" || collision.gameObject.layer == 6)
            {
                if (!playerController.isAttacking && !isAttacking)
                {
                    ChangeAnimationState("Attack");
                    if (collision.transform.position.x > transform.position.x)
                    {
                        playerController.Knockback(false);
                    }
                    else
                    {
                        playerController.Knockback(true);
                    }
                    playerController.TakeDamage(damage);
                    isAttacking = true;
                }
                else
                {
                    ChangeAnimationState("Hurt");
                    health -= 0.5f;
                    if (collision.transform.position.x > transform.position.x)
                    {
                        Knockback(true);
                    }
                    else
                    {
                        Knockback(false);
                    }
                    Debug.Log(health);
                }
            }
        }
    }

    public void Knockback(bool onRight)
    {
        KBCounter = KBTotalTime;
        KBRight = onRight;
    }
}
