using UnityEngine;
using UnityEngine.UI;

public class PlayerGrabber : MonoBehaviour
{
    public Transform rightHandSlot;
    public float grabDistance = 10f;
    public string grabbableTag = "item";
    public string weaponTag = "Weapon";
    public string cumstomerTag = "Customer";
    public string coffeeMachineTag = "CoffeeMachine";

    private GameObject grabbedObject = null;

    private PlayerCombat playerCombat;

    public LayerMask interactableLayers;
    public LayerMask CustomerLayers;
    public GameObject interactPromptUI;
    public Text interactPromptText;

    AudioSource audioSource;
    public AudioClip grabclip;
    public AudioClip interactclip;


    void Start()
    {
        playerCombat = GetComponent<PlayerCombat>();
        interactPromptUI.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleInteractionCheck();

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryGrabObject();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropObject();
        }
        if (Input.GetMouseButton(0))
        {
            TryClickInteraction();
        }
    }

    void HandleInteractionCheck()
    {
        // 플레이어 정면 방향 기준 Ray
        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDir = Camera.main.transform.forward;

        if (Physics.Raycast(rayOrigin, rayDir, out RaycastHit hit, grabDistance, interactableLayers))
        {
            GameObject target = hit.collider.gameObject;
            // 인터랙션 가능한 태그인지 체크
            if (target.CompareTag(grabbableTag) || target.CompareTag(weaponTag) || target.CompareTag(cumstomerTag))
            {
                string targetName = target.name.Replace("(Clone)", "").Trim();
                interactPromptText.text = $"E: {targetName}";
                interactPromptText.gameObject.SetActive(true);
                return;
            }
        }

        interactPromptUI.SetActive(false);
    }

    void TryGrabObject()
    {
        // 플레이어 눈높이 기준 + 카메라 방향 기준으로 Ray 쏘기
        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDirection = Camera.main.transform.forward;


        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, grabDistance, interactableLayers))
        {
            Debug.Log("Hit: " + hit.collider.name); // 실제 충돌 확인용

            if (hit.collider.CompareTag(grabbableTag) || hit.collider.CompareTag(weaponTag))
            {
                GrabObject(hit.collider.gameObject);
            }
        }
    }

    void GrabObject(GameObject obj)
    {
        if (grabbedObject != null)
        {
            DropObject();
        }

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        Collider col = obj.GetComponent<Collider>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        if(col != null)
        {
            col.enabled = false;
        }

        obj.transform.SetParent(rightHandSlot);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        grabbedObject = obj;

        if (obj.CompareTag("Weapon") && playerCombat != null)
        {
            playerCombat.SetWeaponInHand(true);
            col.enabled = true;
            WeaponCollider wc = obj.GetComponent<WeaponCollider>();
            if (wc != null)
            {
                wc.SetPlayerCombat(playerCombat);
                wc.OnWeaponGrabbed();

            }
        }
        audioSource.PlayOneShot(grabclip);
    }

    void DropObject()
    {
        if(grabbedObject != null)
        {
            grabbedObject.transform.SetParent(null);

            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            Collider col = grabbedObject.GetComponent<Collider>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            if (col != null)
            {
                col.enabled = true;
            }

            if (grabbedObject.CompareTag("Weapon") && playerCombat != null) 
            {
                WeaponCollider wc = grabbedObject.GetComponent<WeaponCollider>();
                playerCombat.SetWeaponInHand(false);
                wc.OnWeaponDropped();
            }
                
            grabbedObject=null;
        }
    }
    void TryClickInteraction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int combinedLayers = interactableLayers | CustomerLayers;

        if (Physics.Raycast(ray, out RaycastHit hit, grabDistance, combinedLayers))
        {
            GameObject target = hit.collider.gameObject;

            // 커피머신 클릭
            if (target.CompareTag(coffeeMachineTag))
            {
                var machine = target.GetComponent<PW.BewerageMaker>();
                if (machine != null)
                {
                    machine.SendMessage("OnMouseUp"); // 커피머신의 작동 메서드 호출
                }
            }

            // 고객에게 아이템 건네주기
            else if (target.CompareTag(cumstomerTag))
            {
                TryGiveItemToCustomer();
            }
        }
    }

    void TryGiveItemToCustomer()
    {
        if (grabbedObject == null || grabbedObject.CompareTag(weaponTag)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, grabDistance, CustomerLayers))
        {
            if (hit.collider.CompareTag(cumstomerTag))
            {
                CustomerInteraction customer = hit.collider.GetComponent<CustomerInteraction>();
                if (customer != null)
                {
                    customer.ReceiveItem(grabbedObject);
                    grabbedObject = null;
                    audioSource.PlayOneShot(interactclip);
                }
            }
        }

    }
}
