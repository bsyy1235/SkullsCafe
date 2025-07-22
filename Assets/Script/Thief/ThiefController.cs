using UnityEngine;
using UnityEngine.AI;

public class ThiefController : MonoBehaviour
{
    public Transform[] waypoints;
    public int stopIndex = 2;
    private int current = 0;
    public GameObject coinPrefab;
    private GameObject coin = null;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isStopped = false;
    public Transform anchor;

    public Rigidbody rb;
    private Rigidbody coinRb;

    private bool hasStolenCoin = false;
    private bool isDead = false;

    AudioSource audioSource;
    public AudioClip Dieclip;

    void Start()
    {
        WarningUI.instance.OnThiefSpawn();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isStopped || agent.pathPending) return;

        if (agent.remainingDistance < 0.2f)
        {
            current++;

            if (current == stopIndex)
            {
                isStopped = true;
                agent.isStopped = true;
                StealCoin();
            }
            else if (current < waypoints.Length)
            {
                MoveToNextPoint();
            }
            else
            {
                Destroy(gameObject);
                WarningUI.instance.OnThiefDestroy();
            }
        }
    }

    void MoveToNextPoint()
    {
        animator.SetBool("isWalking", true);
        agent.SetDestination(waypoints[current].position);
    }

    void ResumeMovement()
    {
        isStopped = false;
        agent.isStopped = false;
        MoveToNextPoint();
    }

    void StealCoin()
    {
        if (CoinManager.instance.coinCount == 0)
        {
            ResumeMovement();
            return;
        }

        if (hasStolenCoin) return;

        hasStolenCoin = true;
        coin = Instantiate(coinPrefab, anchor.position, Quaternion.identity);
        coin.transform.SetParent(anchor, true);
        coinRb = coin.GetComponent<Rigidbody>();
        coinRb.useGravity = false;
        coinRb.isKinematic = true;

        CoinManager.instance.AddCoin(-1);
        ResumeMovement();
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        isStopped = true;
        agent.isStopped = true;
        animator.SetBool("Die", true);
        if (coin != null)
        {
            CoinManager.instance.AddCoin(1);
            coin.transform.SetParent(null);
            coinRb.useGravity = true;
            coinRb.isKinematic = false;
        }

        audioSource.PlayOneShot(Dieclip);
        WarningUI.instance.OnThiefDestroy();

        Destroy(gameObject, 1f);
        Destroy(coin, 1.5f);
    }

}
