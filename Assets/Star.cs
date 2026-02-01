using UnityEngine;
using System.Collections.Generic; 

public class Star : MonoBehaviour
{
    public bool breaksInto;
    [SerializeField] GameObject creates;
    [SerializeField] int createAmount;
    [SerializeField] float lifetime;
    [SerializeField] float speedScale;
    [SerializeField] float damping = 5;

    Vector2 initialForce;
    public Vector2 currentVelocity;

    float time = 0;

    static List<Star> currentFragments = new();
    List<Star> followedFragments = new();

    public void SetInitialForce(Vector2 force)
    {
        initialForce = force * speedScale;

        if (!breaksInto)
        {
            AddToList();
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }
    }

    Vector3 fragCenter;
    void Update()
    {
        initialForce = Vector2.Lerp(initialForce, Vector2.zero, Time.deltaTime * damping);

        if (followedFragments.Count > 0)
        {
            for (int i = 0; i <= followedFragments.Count; i++)
            {
                Star frag;
                if (i == followedFragments.Count)
                    frag = this;
                else
                {
                    frag = followedFragments[i];
                    if (Vector3.Distance(frag.transform.position, transform.position) < 0.03f)
                    {
                        followedFragments.Remove(frag);
                        Destroy(frag.gameObject);
                        i--;
                    }
                }

                // frag.currentVelocity *= Mathf.Min(Vector3.Distance(frag.transform.position, transform.position), 1);
                frag.currentVelocity *= 0.97f;
                frag.currentVelocity += (Vector2)(fragCenter - frag.transform.position).normalized * Time.deltaTime * 0.4f;
            }

            if (followedFragments.Count <= 0)
            {
                GameObject creation = Instantiate(creates, transform);
                creation.transform.SetParent(null);
                Destroy(gameObject);
            }
        }

        if (breaksInto && initialForce.magnitude <= 0.03f && time >= lifetime)
        {
            float angleOffset = Random.Range(0, 360);
            for (int i = 0; i < createAmount; i++)
            {
                float factor = i / (float)(createAmount);

                Star creation = Instantiate(creates, transform).GetComponent<Star>();
                creation.transform.SetParent(null);
                creation.SetInitialForce(((Vector2)(Quaternion.AngleAxis(factor * 360 + angleOffset, Vector3.forward) * Vector3.right) + Random.insideUnitCircle * 0.1f) * 0.02f);

                Destroy(gameObject);
            }
        }

        transform.position += (Vector3)(currentVelocity + initialForce) * Time.deltaTime * 100;
        time += Time.deltaTime;
    }

    void AddToList()
    {
        if (currentFragments.Count < createAmount - 1)
            currentFragments.Add(this);
        else
        {
            fragCenter += transform.position;
            for (int i = 0; i < createAmount - 1; i++)
            {
                fragCenter += currentFragments[0].transform.position;

                followedFragments.Add(currentFragments[0]);
                currentFragments.RemoveAt(0);
            }

            fragCenter /= createAmount;
        }
    }
}
