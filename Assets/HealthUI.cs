using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] Image fill;

    void Update()
    {
        if (health)
            fill.fillAmount = health.currentHealth / health.maxHealth;
    }
}
