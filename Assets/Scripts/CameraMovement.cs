using UnityEngine;
using System.Collections.Generic;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] List<Transform> target;
    [SerializeField] float followSpeed;

    Vector3 followOffset;

    public bool canMove;

    void Start()
    {
        followOffset = transform.position;
        canMove = false;
    }

    void Update()
    {
        if (canMove)
        {
            Vector3 newPos = Vector3.zero;
            int amount = 0;
            for (int i = 0; i < target.Count; i++)
            {
                newPos += target[i].position;
                amount ++;
            }
            newPos.x /= 1.5f;
            newPos.y /= 1.3f;
            newPos /= amount;
    
            transform.position = Vector3.Lerp(transform.position, newPos + followOffset, Time.deltaTime * followSpeed);
        }
    }
}
