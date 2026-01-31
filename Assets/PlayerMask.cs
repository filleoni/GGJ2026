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
    [SerializeField] GameObject asset;

    GameObject currentAsset;
    float cooldownTimer = 0;

    public void Equip(PlayerAction me)
    {
        if (asset)
            currentAsset = Instantiate(asset, me.transform);
        Process.Invoke(me);
    }

    public void Unequip(PlayerAction me)
    {
        if (currentAsset)
            Destroy(currentAsset);

        cooldownTimer = 0;
    }

    LineRenderer laserLine;
    public void ProcessLaser(PlayerAction me)
    {
        if (!laserLine) laserLine = currentAsset.GetComponentInChildren<LineRenderer>();

        if (Input.GetButton("Fire1"))
        {
            laserLine.SetPosition(0, Vector2.zero);
            laserLine.SetPosition(1, me.Cursor.transform.position - me.transform.position);

            if (cooldownTimer <= 0)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(
                    me.transform.position,
                    (me.Cursor.transform.position - me.transform.position),
                    (me.Cursor.transform.position - me.transform.position).magnitude);
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
        else
        {
            laserLine.SetPosition(0, Vector3.zero);
            laserLine.SetPosition(1, Vector3.zero);
        }
        cooldownTimer = Mathf.Max(cooldownTimer - Time.deltaTime, 0);
    }

    GameObject gravityBomb;
    public void ProcessGravity(PlayerAction me)
    {
        if (!gravityBomb)
        {
            gravityBomb = currentAsset.GetComponentInChildren<GravityBomb>().gameObject;
            if (gravityBomb)
                gravityBomb.gameObject.SetActive(false);
        }

        if (Input.GetButton("Fire1"))
        {
            if (cooldownTimer <= 0)
            {
                GravityBomb bomb = Instantiate(gravityBomb, me.transform).GetComponent<GravityBomb>();
                bomb.transform.SetParent(null);
                bomb.gameObject.SetActive(true);
                bomb.SetTargetPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

                cooldownTimer = cooldown;
            }
        }
        cooldownTimer = Mathf.Max(cooldownTimer - Time.deltaTime, 0);
    }
}
