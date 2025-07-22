using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CustomerController : MonoBehaviour
{
    public Transform[] waypoints;
    public int stopIndex = 4;
    private int current = 0;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isStopped = false;

    [Header("UI")]
    public GameObject requestUIPrefab;
    public Transform uiAnchor;
    private GameObject uiInstance;

    [HideInInspector] public string requiredItem;
    [HideInInspector] public Sprite[] itemIcons;

    private CustomerInteraction interaction;

    void Awake()
    {
        interaction = GetComponent<CustomerInteraction>();
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        //ShowRequestUI();
    }
    
    void Update()
    {
        if (isStopped || agent.pathPending) return;

        if (agent.remainingDistance < 0.2f)
        {
            current++;

            if (current == stopIndex && !interaction.hasInteracted)
            {
                ShowRequestUI();
                isStopped = true;
                agent.isStopped = true;
                animator.SetBool("isWalking", false);
            }
            else if (current < waypoints.Length)
            {
                MoveToNextPoint();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        if (uiInstance != null && Camera.main != null)
        {
            // 카메라를 바라보는 Billboard 방식
            Vector3 dir = Camera.main.transform.position - uiInstance.transform.position;
            dir.y = 0; // Y축 회전만 적용
            if (dir.sqrMagnitude > 0.001f)
            {
                uiInstance.transform.rotation = Quaternion.LookRotation(-dir);
            }
        }
    }

    void MoveToNextPoint()
    {
        animator.SetBool("isWalking", true);
        agent.SetDestination(waypoints[current].position);
    }

    public void ResumeMovement()
    {
        isStopped = false;
        agent.isStopped = false;
        MoveToNextPoint();
    }

    void ShowRequestUI()
    {
        if (requestUIPrefab == null || uiAnchor == null || itemIcons.Length == 0) return;

        uiInstance = Instantiate(requestUIPrefab, uiAnchor.position, Quaternion.identity);
        uiInstance.transform.SetParent(uiAnchor, true);

        // 월드 캔버스 회전 및 크기 설정
        uiInstance.transform.localRotation = Quaternion.LookRotation(uiInstance.transform.position - Camera.main.transform.position);
        uiInstance.transform.localScale = Vector3.one * 0.01f; // 필요 시 조절

        Sprite icon = GetItemIcon(requiredItem);

        CustomerRequestUI uiScript = uiInstance.GetComponent<CustomerRequestUI>();
        if (uiScript != null)
        {
            uiScript.SetMenu(icon);
        }
    }

    public void ShowAngryUI()
    {
        if (uiInstance == null) return;

        CustomerRequestUI uiScript = uiInstance.GetComponent<CustomerRequestUI>();
        if (uiScript != null)
        {
            uiScript.showAngerIcon();
            StartCoroutine(HideAngryUIAfterDelay(1f));
        }
    }

    public void DeleteRequestUI()
    {
        Destroy(uiInstance);
    }
    
    Sprite GetItemIcon(string itemName)
    {
        switch (itemName)
        {
            case "Coffee": return itemIcons[0];
            case "OrangeJuice": return itemIcons[1];
            case "Donut": return itemIcons[2];
            case "Cake": return itemIcons[3];
            default: return null;
        }
    }
    IEnumerator HideAngryUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DeleteRequestUI();
    }
}
