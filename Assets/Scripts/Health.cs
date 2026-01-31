using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth = 50;
    [SerializeField] GameObject bar = null;
    [SerializeField] bool cursed = false;
    Vector3 barStartSize;

    public enum Teams { Good, Evil, Cursed }
    public Teams Alignment = Teams.Evil;

    [SerializeField] List<GameObject> drops = new();

    float currentHealth = 1;
    public UnityEvent<Vector2> SignalKnockback;
    public UnityEvent<bool> SignalUncursed;

    float uncursed = 0;
    float curseTime = 0.45f;
    float curseTimer = 0;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (bar)
            barStartSize = bar.transform.localScale;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (uncursed <= 0)
        {
            SignalUncursed.Invoke(false);
            curseTimer = 0;
        }
        else
        {
            uncursed = Mathf.Max(uncursed - Time.deltaTime, 0);
            curseTimer = Mathf.Max(curseTimer - Time.deltaTime, 0);
            if (curseTimer <= 0)
            {
                TakeDamage(1.5f);
                curseTimer = curseTime;
            }
        }

        if (currentHealth != maxHealth)
            print(currentHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        if (animator)
            animator.SetTrigger("Damaged");

        if (bar)
            bar.transform.localScale = barStartSize * (currentHealth / maxHealth) * ((cursed && uncursed > 0) ? 3 : 1);
        CheckDeath();
    }

    public void TakePoison(float damage)
    {
        print("OW");

        uncursed = damage;
        SignalUncursed.Invoke(true);
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
