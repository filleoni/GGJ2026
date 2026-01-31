using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth = 50;

    public enum Teams { Good, Evil }
    public Teams Alignment = Teams.Evil;

    [SerializeField] List<GameObject> drops = new();

    float currentHealth = 1;
    public UnityEvent<Vector2> SignalKnockback;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        if (animator)
            animator.SetTrigger("Damaged");

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
            for (int i = 0; i < drops.Count; i++)
            {
                float factor = i / (float)(drops.Count);

                GameObject d = Instantiate(drops[i], transform);
                d.transform.SetParent(null);
                Star star = d.GetComponent<Star>();
                if (star)
                {
                    star.SetInitialForce((new Vector2(Mathf.Sin(Mathf.Deg2Rad * factor * 360), Mathf.Cos(Mathf.Deg2Rad * factor * 360)) + Random.insideUnitCircle * 0.5f) * 0.02f);
                }
            }
            Destroy(gameObject);
        }
    }
}
