using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class Star : MonoBehaviour
{
    public bool breaksInto;
    [SerializeField] GameObject creates;
    [SerializeField] int createAmount;
    [SerializeField] float lifetime;
    [SerializeField] float speedScale;
    [SerializeField] float damping = 5;
    [SerializeField] AnimationCurve velocityOverTime;

    Vector2 initialForce;
    public Vector2 currentVelocity;

    float time = 0;

    [SerializeField] SpriteRenderer sprite;
    static List<Star> currentFragments = new();
    List<Star> followedFragments = new();

    public static UnityEvent SignalScore = new();

    private void Start()
    {
        if (!sprite)
            sprite = GetComponent<SpriteRenderer>();
    }

    public void SetInitialForce(Vector2 force)
    {
        initialForce = force * speedScale;

        if (!breaksInto)
        {
            AddToList();
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }
    }

    Vector3 fragCenter = Vector3.zero;
    void Update()
    {
        initialForce = Vector2.Lerp(initialForce, Vector2.zero, Time.deltaTime * damping);

        if (followedFragments.Count > 0)
        {
            fragCenter = Vector2.zero;
            for (int i = 0; i < followedFragments.Count; i++)
            {
                if (i == followedFragments.Count)
                    fragCenter += transform.position;
                else if (i < followedFragments.Count)
                    fragCenter += followedFragments[i].transform.position;
            }
            fragCenter /= followedFragments.Count;

            for (int i = 0; i <= followedFragments.Count - 1; i++) // Remvoe -1 to include self
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

                fragCenter = transform.position;
                // frag.currentVelocity *= 0.96f;
                frag.transform.position = Vector2.MoveTowards(frag.transform.position, fragCenter, Time.deltaTime * 5f);
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

        if (!creates)
        {
            float vel = velocityOverTime.Evaluate(time);
            currentVelocity.y = vel;
            transform.up = Vector2.up;
            transform.localScale = new Vector2(1 / ((vel * 15) + 0.6f), 20 * vel);
            if (time < 0.07f)
                transform.localScale = new Vector2(3, 0.2f);
            if (!sprite.isVisible)
            {
                SignalScore?.Invoke();
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
