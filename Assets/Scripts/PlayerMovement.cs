using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float playerRadius;

    Vector2 currentVelocity;
    Vector2 desiredVelocity;

    private void Update()
    {
        desiredVelocity.x = Input.GetAxisRaw("Horizontal");
        desiredVelocity.y = Input.GetAxisRaw("Vertical");

        float dot = Vector2.Dot(desiredVelocity, transform.position);
        if (dot >= 0.9f && transform.position.magnitude > playerRadius)
            desiredVelocity *= 0;

        currentVelocity = Vector2.Lerp(currentVelocity, desiredVelocity, Time.deltaTime * 4);
        if (currentVelocity.magnitude <= 0)
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, Time.deltaTime * 5f);

        transform.position += (Vector3)currentVelocity * Time.deltaTime * movementSpeed;
    }

}
