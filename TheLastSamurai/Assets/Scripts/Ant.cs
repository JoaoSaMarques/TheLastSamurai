using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 1;
    [SerializeField]
    private float invulnerabilityDuration = 2;
    [SerializeField]
    private float blinkDuration = 0.1f;
    [SerializeField] private float speed;
    [SerializeField] private Transform wallDetector;
    [SerializeField] private Transform groundAheadDetector;
    [SerializeField] private Transform groundDetector;
    [SerializeField] private float groundDetectorRadius;
    [SerializeField] private float groundDetectorExtraRadius;
    [SerializeField] private LayerMask groundMask;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool onGround;
    private int health;
    private float invulnerabilityTimer = 0;
    private float blinkTimer = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        DetectGround();

        Vector2 currentVelocity = rb.velocity;

        if (onGround)
        {
            DetectWallAndNotGround();
            currentVelocity.x = speed * transform.right.x;
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

            player.DealDamage(1, gameObject);
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
}
