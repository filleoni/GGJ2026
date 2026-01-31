using UnityEngine;
using System.Collections.Generic; 

public class Star : MonoBehaviour
{
    public bool breaksInto;
    [SerializeField] GameObject creates;
    [SerializeField] int createAmount;

    Vector2 initialForce;

    public Vector2 currentVelocity;

    static List<Star> currentFragments = new();
    List<Star> followedFragments = new();

    public void SetInitialForce(Vector2 force)
    {
        initialForce = force;

        if (!breaksInto)
            AddToList();
    }

    void Update()
    {
        initialForce = Vector2.Lerp(initialForce, Vector2.zero, Time.deltaTime * 5f);

        if (followedFragments.Count > 0)
        {
            for (int i = 0; i < followedFragments.Count; i++)
            {
                Star frag = followedFragments[i];
                if (Vector3.Distance(frag.transform.position, transform.position) < 0.03f)
                {
                    followedFragments.Remove(frag);
                    Destroy(frag.gameObject);
                    i--;
                }

                frag.currentVelocity *= Mathf.Min(Vector3.Distance(frag.transform.position, transform.position), 1);
                frag.currentVelocity += (Vector2)(transform.position - frag.transform.position).normalized * Time.deltaTime * 0.1f;
            }
            currentVelocity = Vector2.zero;

            if (followedFragments.Count <= 0)
            {
                GameObject creation = Instantiate(creates, transform);
                creation.transform.SetParent(null);
                Destroy(gameObject);
            }
        }

        if (breaksInto && initialForce.magnitude <= 0.1f)
        {
            for (int i = 0; i < createAmount; i++)
            {
                float factor = i / (float)(createAmount);

                Star creation = Instantiate(creates, transform).GetComponent<Star>();
                creation.transform.SetParent(null);
                creation.SetInitialForce((new Vector2(Mathf.Sin(Mathf.Deg2Rad * factor * 360), Mathf.Cos(Mathf.Deg2Rad * factor * 360)) + Random.insideUnitCircle * 0.5f) * 0.02f);
            }
        }

        transform.position += (Vector3)(currentVelocity + initialForce);
    }

    void AddToList()
    {
        if (currentFragments.Count < createAmount - 1)
            currentFragments.Add(this);
        else 
            for (int i = 0; i < createAmount - 1; i++)
            {
                followedFragments.Add(currentFragments[0]);
                currentFragments.RemoveAt(0);
            }
    }
}
