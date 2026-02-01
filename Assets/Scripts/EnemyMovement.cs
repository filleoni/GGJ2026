using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float damage;
    [SerializeField] float attackCooldown;
    [SerializeField] Health health;
    [SerializeField, Range(-1, 1)] float sideStep;
    [SerializeField, Range(-1, 1)] float sideStep2;
    [SerializeField] float stopDistance;
    [SerializeField] Sprite UncursedSprite;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] ParticleSystem cursedParticles;
    [SerializeField] Animator animator;

    Sprite startSprite;

    Transform target;
    Health targetHealth;
    float ss;
    float attackTimer;

    void Start()
    {
        if (sprite)
            startSprite = sprite.sprite;

        target = Object.FindFirstObjectByType<LifeTree>().transform;
        if (target)
            targetHealth = target.GetComponent<Health>();

        animator = GetComponent<Animator>();
        if (!health)
        {
            health = GetComponent<Health>();
            if (health)
            {
                health.SignalKnockback.AddListener((v) => { currentVelocity += (Vector3)v; });
                health.SignalUncursed.AddListener((b) => { UpdateCursed(b); });
            }

        }

        ss = Random.Range(sideStep, sideStep2) * (Random.Range(1, 100) >= 50 ? -1 : 1);
    }

    Vector3 desiredPosition;
    Vector3 desiredVelocity;
    Vector3 currentVelocity;

    float emission;
    void UpdateCursed(bool uncursed)
    {
        if (!UncursedSprite)
            return;

        if (sprite)
            sprite.sprite = uncursed ? UncursedSprite : startSprite;
        if (cursedParticles != null)
        {
            if (uncursed)
            {
                cursedParticles.enableEmission = false;
            }
            else
            {
                cursedParticles.enableEmission = true;
            }
        }
    }

    void Update()
    {
        if (!target)
            return;

        desiredPosition = target.position;
        desiredVelocity = Quaternion.AngleAxis(-90 * ss, Vector3.forward) * (desiredPosition - transform.position).normalized;
        if ((desiredPosition - transform.position).magnitude < stopDistance)
        {
            desiredVelocity = Vector2.zero;
            ProcessAttack();
        }

        currentVelocity = Vector3.Lerp(currentVelocity, desiredVelocity, Time.deltaTime * 5);
        transform.position += currentVelocity * Time.deltaTime * movementSpeed;

        if (currentVelocity.magnitude > 0.1)
        {
            transform.localScale = new Vector3(Mathf.Sign(-currentVelocity.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void ProcessAttack()
    {
        if (attackTimer <= 0)
        {
            targetHealth.TakeDamage(damage);
            attackTimer = attackCooldown;

            animator.SetTrigger("Attack");
        }
        else
            attackTimer = Mathf.Max(attackTimer - Time.deltaTime, 0);
    }
}
