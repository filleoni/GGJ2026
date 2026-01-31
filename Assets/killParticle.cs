using UnityEngine;

public class killParticle : MonoBehaviour
{
    float killTimer = 2;
    void Update()
    {
        killTimer -= Time.deltaTime;
        if (killTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
