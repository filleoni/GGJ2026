using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 50;

    public enum Teams {Good, Evil}
    public Teams Alignment = Teams.Evil;

    int currentHealth = 1;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);

        CheckDeath();
    }

    void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
