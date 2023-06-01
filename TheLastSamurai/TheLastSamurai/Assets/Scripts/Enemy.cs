using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;
    [SerializeField]
    private float invulnerabilityDuration = 0;
    [SerializeField]
    private float blinkDuration = 0.1f;
    [SerializeField] private float speed;
    [SerializeField] private Transform wallDetector;
    [SerializeField] private Transform groundAheadDetector;
    [SerializeField] private Transform groundDetector;
    [SerializeField] private float groundDetectorRadius;
    [SerializeField] private float groundDetectorExtraRadius;
    [SerializeField] private LayerMask groundMask;

    Transform target;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool onGround;
    private int health;
    private float invulnerabilityTimer = 0;
    private float blinkTimer = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        health = maxHealth;
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            target = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player object not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float speedX = Input.GetAxis("Horizontal");

        if (target)
        {
            Vector3 direction = (target.position - transform.position).normalized;

            if (Vector3.Distance(transform.position, target.position) <= 100)
            {
                // Move towards the player
                transform.Translate(direction * speed * Time.deltaTime);
            }
        }

        DetectGround();

        Vector2 currentVelocity = rb.velocity;

        if (onGround)
        {
            DetectWallAndNotGround();
        }
        else
        {
            currentVelocity.x = 0;
        }

        rb.velocity = currentVelocity;

        if (invulnerabilityTimer > 0)
        {
            invulnerabilityTimer -= Time.deltaTime;
            if (invulnerabilityTimer <= 0)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                blinkTimer -= Time.deltaTime;
                if (blinkTimer <= 0)
                {
                    spriteRenderer.enabled = !spriteRenderer.enabled;
                    blinkTimer = blinkDuration;
                }
            }
        }

        animator.SetFloat("AbsVelocityX", Mathf.Abs(currentVelocity.x));
        animator.SetFloat("VelocityY", currentVelocity.y);
        animator.SetBool("OnGround", onGround);

        if (target)
        {
            if (transform.position.x < target.position.x)
            {
                spriteRenderer.flipX = false; // Face right
            }
            else if (transform.position.x > target.position.x)
            {
                spriteRenderer.flipX = true; // Face left
            }
        }

        // ...
    }


    void DetectGround()
    {
        Collider2D collider = Physics2D.OverlapCircle(groundDetector.position, groundDetectorRadius, groundMask);
        if (collider != null)
        {
            onGround = true;
        }
        else
        {
            collider = Physics2D.OverlapCircle(groundDetector.position - Vector3.right * groundDetectorExtraRadius, groundDetectorRadius, groundMask);
            if (collider != null) onGround = true;
            else
            {
                collider = Physics2D.OverlapCircle(groundDetector.position + Vector3.right * groundDetectorExtraRadius, groundDetectorRadius, groundMask);
                if (collider != null) onGround = true;
                else onGround = false;
            }
        }
    }

    void DetectWallAndNotGround()
    {
        Collider2D collider = Physics2D.OverlapCircle(wallDetector.position, groundDetectorRadius, groundMask);
        bool wallAhead = (collider != null);

        if (wallAhead)
        {
            transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
        }
        else
        {
            if (groundAheadDetector != null)
            {
                collider = Physics2D.OverlapCircle(groundAheadDetector.position, groundDetectorRadius, groundMask);

                if (collider == null)
                {
                    transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
                }
            }
        }
    }

    public void DealDamage(int damage, GameObject damageDealer)
    {
        if (invulnerabilityTimer > 0) return;

        health = health - damage;
        if (health == 0)
        {
            Destroy(gameObject);
        }

        invulnerabilityTimer = invulnerabilityDuration;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
            if (playerRB.velocity.y < -1e-3) return;

            player.DealDamage(20, gameObject);
        }
    }


    private void OnDrawGizmos()
    {
        if (groundDetector != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(groundDetector.position, groundDetectorRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(groundDetector.position - Vector3.right * groundDetectorExtraRadius, groundDetectorRadius);
            Gizmos.DrawSphere(groundDetector.position + Vector3.right * groundDetectorExtraRadius, groundDetectorRadius);
        }

        if (wallDetector != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(wallDetector.position, groundDetectorRadius);
        }

        if (groundAheadDetector != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(groundAheadDetector.position, groundDetectorRadius);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    
}