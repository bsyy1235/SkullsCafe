using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    [Header("Weapon Settings")]
    public bool isOnGround = true;

    private PlayerCombat playerCombat;
    private Rigidbody rb;
    new Collider collider;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        // ���� �ʱ� ���� (�ٴڿ� ������ ����)
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
        if (collider != null)
        {
            collider.isTrigger = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // �ٴڰ� �浹 ����
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // �ٴڿ��� ��� ��
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // ���� ������ ���ǵ��� üũ
        if (CanAttack() && other.CompareTag("Thief"))
        {
            // ThiefController�� Die �Լ� ȣ��
            ThiefController thiefController = other.GetComponent<ThiefController>();
            if (thiefController != null)
            {
                thiefController.Die();
            }
        }
    }

    private bool CanAttack()
    {
        // ���� ������ ���ǵ��� üũ
        return playerCombat != null &&
               playerCombat.weaponInHand &&
               playerCombat.isAttacking &&
               !isOnGround;
    }

    public void SetPlayerCombat(PlayerCombat combat)
    {
        playerCombat = combat;
    }

    // ���Ⱑ ���� �� ȣ��Ǵ� �Լ�
    public void OnWeaponGrabbed()
    {
        isOnGround = false;

        // Rigidbody ���� ����
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
        if (collider != null) 
        {
            collider.isTrigger = true;
        }

    }

    // ���Ⱑ ������ �� ȣ��Ǵ� �Լ�
    public void OnWeaponDropped()
    {
        isOnGround = false; // �������� ���̹Ƿ� false�� ����

        // Rigidbody ���� ����
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
        if (collider != null)
        {
            collider.isTrigger = false;
        }
    }
}