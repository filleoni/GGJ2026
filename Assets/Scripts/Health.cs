using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Sonity;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth = 50;
    [SerializeField] GameObject bar = null;
    public bool cursed = false;
    Vector3 barStartSize;

    [SerializeField] private SoundEvent deathSound;
    
    [SerializeField] private SoundEvent enemysound;

    public enum Teams { Good, Evil, Cursed }
    public Teams Alignment = Teams.Evil;

    [SerializeField] List<GameObject> drops = new();

    float currentHealth = 1;
    public UnityEvent<Vector2> SignalKnockback;
    public UnityEvent<bool> SignalUncursed;

    public float uncursed = 0;
    float curseTime = 0.45f;
    float curseTimer = 0;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (bar)
            barStartSize = bar.transform.localScale;
        currentHealth = maxHealth;
        
        Debug.Log("started sound from enemies");
        
        enemysound.Play(transform);
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
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage * ((cursed && uncursed > 0) ? 3 : 1), 0);
        if (animator)
            animator.SetTrigger("Damaged");

        if (bar)
            bar.transform.localScale = barStartSize * (currentHealth / maxHealth);
        CheckDeath();
    }

    public void TakePoison(float damage)
    {
        uncursed = damage;
        SignalUncursed.Invoke(true);
    }

    public void TakePercentualDamage(float percent)
    {
        print(percent + ", " + maxHealth * percent + ", " + currentHealth);
        TakeDamage(maxHealth * percent / 3);
    }

    public void TakeKnockback(Vector2 knockback)
    {
        SignalKnockback.Invoke(knockback);
    }

    void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            
            float angleOffset = Random.Range(0, 360);
            for (int i = 0; i < drops.Count; i++)
            {
                float factor = i / (float)(drops.Count);

                GameObject d = Instantiate(drops[i], transform);
                d.transform.SetParent(null);
                Star star = d.GetComponent<Star>();
                if (star)
                {
                    star.SetInitialForce(((Vector2)(Quaternion.AngleAxis(factor * 360 + angleOffset, Vector3.forward) * Vector3.right) + Random.insideUnitCircle * 0.1f) * 0.02f);
                }
            }
            enemysound.Stop(transform);

            if (Alignment != Teams.Good)
                Destroy(gameObject);
        }
    }
}
