using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth = 50;

    public enum Teams { Good, Evil }
    public Teams Alignment = Teams.Evil;

    float currentHealth = 1;
    public UnityEvent<Vector2> SignalKnockback;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);

        CheckDeath();
    }

    public void TakePercentualDamage(float percent)
    {
        print(percent + ", " + maxHealth * percent + ", " + currentHealth);
        TakeDamage(maxHealth * percent);
    }

    public void TakeKnockback(Vector2 knockback)
    {
        SignalKnockback.Invoke(knockback);
    }

    void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
