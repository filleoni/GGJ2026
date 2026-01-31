using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] Health health;

    Transform target;

    void Start()
    {
        target = Object.FindFirstObjectByType<LifeTree>().transform;
        if (!health)
        {
            health = GetComponent<Health>();
            if (health)
                health.SignalKnockback.AddListener((v) => { currentVelocity += (Vector3)v; });
        }
    }

    Vector3 desiredPosition;
    Vector3 desiredVelocity;
    Vector3 currentVelocity;

    void Update()
    {
        if (!target)
            return;

        desiredPosition = target.position;
        desiredVelocity = (desiredPosition - transform.position);
        if (desiredVelocity.magnitude > 1)
            desiredVelocity /= desiredVelocity.magnitude;

        currentVelocity = Vector3.Lerp(currentVelocity, desiredVelocity, Time.deltaTime * 5);
        transform.position += currentVelocity * Time.deltaTime * movementSpeed;

        if (currentVelocity.magnitude > 0.1)
        {
            transform.localScale = new Vector3(Mathf.Sign(-currentVelocity.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
