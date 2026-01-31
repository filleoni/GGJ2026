using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed;

    Transform target;

    void Start()
    {
        target = Object.FindFirstObjectByType<LifeTree>().transform;
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
    }
}
