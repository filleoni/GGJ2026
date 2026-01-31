using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "newMask", menuName = "SO/PlayerMask", order = 0)]
public class PlayerMask : ScriptableObject
{
    public string name = "Mask";
    public Sprite image = null;

    public int damage = 5;
    public float cooldown = 1;

    public UnityEvent<PlayerAction> Process;

    float cooldownTimer = 0;

    public void ProcessLaser(PlayerAction me)
    {
        if (Input.GetButton("Fire1"))
        {
            Debug.Log("Firing my laser");

            if (cooldownTimer <= 0)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll((Vector2)me.transform.position, (Vector2)(me.Cursor.transform.position - me.transform.position));
                for (int i = 0; i < hits.Length; i++)
                {
                    Health victim = hits[i].collider.GetComponent<Health>();
                    if (victim && victim.Alignment == Health.Teams.Evil)
                    {
                        victim.TakeDamage(damage);
                        cooldownTimer = cooldown;
                    }
                }
            }
        }
        cooldownTimer = Mathf.Max(cooldownTimer - Time.deltaTime, 0);
    }

    public void ProcessGravity(PlayerAction me)
    {
        if (Input.GetButton("Fire1"))
            Debug.Log("Gravved");
    }
}
