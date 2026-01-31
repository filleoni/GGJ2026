using UnityEngine;

public class GravityBomb : MonoBehaviour
{
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

        timer += Time.deltaTime;

        if (timer >= lifetime)
            Destroy(gameObject);
    }
}
