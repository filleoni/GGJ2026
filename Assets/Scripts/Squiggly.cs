using UnityEngine;
using System.Collections.Generic;

public class Squiggly : MonoBehaviour
{
    [SerializeField] LineRenderer line;
    [SerializeField] float followRadius;

    Transform target;

    Vector3 followPosition;

    private void Start()
    {
        target = transform.parent;
        transform.SetParent(null);
        transform.position = Vector3.zero;

        line.positionCount = 10;
    }

    private void Update()
    {
        followPosition = (target.position.normalized * Mathf.Min((transform.position - target.position).magnitude, followRadius));

        for (int i = 0; i < line.positionCount; i++)
        {
            float factor = i / ((float)line.positionCount - 1);

            line.SetPosition(i, Vector2.Lerp(followPosition, target.position, factor)); // + Vector2.down * Mathf.Sin(factor * Mathf.Rad2Deg)); 
        }
    }
}
