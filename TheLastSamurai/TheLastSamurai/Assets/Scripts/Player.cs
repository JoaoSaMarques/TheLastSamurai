using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float invulnerabilityDuration = 0;
    [SerializeField]
    private float blinkDuration = 0.1f;
    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float jumpVelocity = 200.0f;
    [SerializeField]
    private float maxJumpTime = 0.1f;
    [SerializeField]
    private float jumpGravity = 1.0f;
    [SerializeField]
    private int maxJumps = 1;
    [SerializeField]
    private float coyoteTime = 0.1f;
    [SerializeField]
    private Transform groundDetector;
    [SerializeField]
    private float groundDetectorRadius = 2;
    [SerializeField]
    private float groundDetectorExtraRadius = 6.0f;
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private Collider2D groundCollider;
    [SerializeField]
    private Collider2D airCollider;
    [SerializeField]
    private int PlayerHealth = 100;
    [SerializeField]
    private int health = 100;


    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool onGround = false;
    private float lastJumpTime;
    private float initialGravity;
    private float lastGroundTime;
    private int nJumps = 0;
    private float invulnerabilityTimer = 0;
    private float blinkTimer = 0;

    private Dictionary<GameObject, float> hitTime;

    [SerializeField] private AudioSource audioSourceSteps;
    [SerializeField] private AudioSource audioSourceShot;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initialGravity = rb.gravityScale;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void Start()
    {
        animator.SetBool("Armour", GameManager.Instance.GetPlayerArmorState());
        if (animator.GetBool("Armour"))
        {
            // Apply the appropriate settings when armor is enabled
            PlayerHealth = 200;
            health = 200;
            moveSpeed = 50f;
        }
        else
        {
            PlayerHealth = 100;
            health = 100;
            moveSpeed = 100f;
        }
    }

    // Update is called once per frame
    void Update()
    {

        // Update character horizontal velocity
        Vector2 currentVelocity = rb.velocity;
        float speedX = Input.GetAxis("Horizontal");

        DetectGround();
        if (onGround)
        {
            lastGroundTime = Time.time;
        }
        if ((Time.time - lastGroundTime) <= coyoteTime)
        {
            if (currentVelocity.y <= 0)
            {
                nJumps = maxJumps;
            }
        }
        else
        {
            if (nJumps == maxJumps)
            {
                nJumps = 0;
            }
        }

        if ((Input.GetMouseButtonDown(0)))
        {
            animator.SetTrigger("Attack");
            audioSourceShot.Play();
        }

        groundCollider.enabled = onGround;
        airCollider.enabled = !onGround;

        currentVelocity.x = speedX * moveSpeed;

        if (speedX != 0 && !audioSourceSteps.isPlaying)
        {
            audioSourceSteps.Play();
        }
        else if (speedX == 0 || !onGround)
        {
            audioSourceSteps.Stop();
        }


        if ((Input.GetButtonDown("Jump")) && (nJumps > 0))
        {
            currentVelocity.y = jumpVelocity;
            lastJumpTime = Time.time;
            lastGroundTime = 0;
            rb.gravityScale = jumpGravity;
            nJumps--;
        }
        else if ((Input.GetButton("Jump")) && ((Time.time - lastJumpTime) < maxJumpTime) && (currentVelocity.y > 0))
        {
            rb.gravityScale = jumpGravity;
        }
        else
        {
            rb.gravityScale = initialGravity;
            lastJumpTime = 0;
        }

        rb.velocity = currentVelocity;

       
        animator.SetFloat("AbsVelocityX", Mathf.Abs(currentVelocity.x));
        animator.SetFloat("VelocityY", currentVelocity.y);
        animator.SetBool("OnGround", onGround);

        //if (speedX < 0) spriteRenderer.flipX = true;
        //else if (speedX > 0) spriteRenderer.flipX = false;

        //if (speedX < 0) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        //else if (speedX > 0) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        if (speedX < 0)
        {
            if (transform.right.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else if (speedX > 0)
        {
            if (transform.right.x < 0)
            {
                transform.rotation = Quaternion.identity;
            }
        }

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
        if (collider != null) onGround = true;
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

    public void SetMaxJumps(int n)
    {
        maxJumps = n;
    }

    public int GetHealth()
    {
        return health;
    }
    public int GetMaxHealth()
    {
        return PlayerHealth;
    }

    public void ToggleArmor()
    {
        bool currentArmorState = !animator.GetBool("Armour");
        GameManager.Instance.SetPlayerArmorState(currentArmorState);
        animator.SetBool("Armour", currentArmorState);

        if (currentArmorState)
        {
            PlayerHealth = 200;
            health = 200;
            moveSpeed = 50f;
        }
        else
        {
            PlayerHealth = 100;
            health = 100;
            moveSpeed = 100f;
        }
    }

    public bool IsArmor()
    {
        return animator.GetBool("Armour");
    }

    public void DealDamage(int damage, GameObject damageDealer)
    {
        if (invulnerabilityTimer > 0) return;

        if (hitTime != null)
        {
            float t;
            if (hitTime.TryGetValue(damageDealer, out t))
            {
                if ((Time.time - t) < 1.0f)
                {
                    return;
                }
            }
        }

        health = health - damage;
        if (health == 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (hitTime == null) hitTime = new Dictionary<GameObject, float>();
        hitTime[damageDealer] = Time.time;

        invulnerabilityTimer = invulnerabilityDuration;
    }

    private void OnDrawGizmos()
    {
        if (groundDetector == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(groundDetector.position, groundDetectorRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundDetector.position - Vector3.right * groundDetectorExtraRadius, groundDetectorRadius);
        Gizmos.DrawSphere(groundDetector.position + Vector3.right * groundDetectorExtraRadius, groundDetectorRadius);
    }
}
