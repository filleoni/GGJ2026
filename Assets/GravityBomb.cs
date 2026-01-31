using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravityBomb : MonoBehaviour
{
    [SerializeField] float effectRange;
    [SerializeField, Range(0, 1)] float percentualDamage;

    [SerializeField] float lifetime = 1;
    [SerializeField] AnimationCurve jumpCurve;
    [SerializeField] float jumpHeight;

    Vector3 startPos;
    Vector3 targetPos;
    float timer = 0;

    public void SetTargetPosition(Vector2 pos)
    {
        startPos = transform.position;
        targetPos = pos;
    }

    void Update()
    {
        float factor = timer / lifetime;
        transform.position = Vector3.Lerp(startPos, targetPos, factor) + Vector3.up * jumpCurve.Evaluate(factor) * jumpHeight;

        timer += Time.deltaTime; // / (Vector3.Distance(startPos, targetPos) * 0.3f);

        if (timer >= lifetime)
            StartCoroutine(Explode());
    }

    float explosionTime = 1;
    float explosionTimer = 0;
    List<Health> victims = new();
    IEnumerator Explode()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            transform.position,
            effectRange,
            Vector3.forward);
        for (int i = 0; i < hits.Length; i++)
        {
            Health victim = hits[i].collider.GetComponent<Health>();
            if (victim && victim.Alignment == Health.Teams.Evil)
            {
                victim.TakeKnockback((transform.position - victim.transform.position).normalized * 1);
                victim.TakePercentualDamage(percentualDamage);
            }
        }

        while (explosionTimer < explosionTime)
        {
            explosionTimer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
        yield return null;
    }
}
