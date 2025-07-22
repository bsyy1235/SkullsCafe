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

        // 무기 초기 설정 (바닥에 떨어진 상태)
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
        // 바닥과 충돌 감지
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // 바닥에서 벗어날 때
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 공격 가능한 조건들을 체크
        if (CanAttack() && other.CompareTag("Thief"))
        {
            // ThiefController의 Die 함수 호출
            ThiefController thiefController = other.GetComponent<ThiefController>();
            if (thiefController != null)
            {
                thiefController.Die();
            }
        }
    }

    private bool CanAttack()
    {
        // 공격 가능한 조건들을 체크
        return playerCombat != null &&
               playerCombat.weaponInHand &&
               playerCombat.isAttacking &&
               !isOnGround;
    }

    public void SetPlayerCombat(PlayerCombat combat)
    {
        playerCombat = combat;
    }

    // 무기가 잡힐 때 호출되는 함수
    public void OnWeaponGrabbed()
    {
        isOnGround = false;

        // Rigidbody 설정 변경
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

    // 무기가 떨어질 때 호출되는 함수
    public void OnWeaponDropped()
    {
        isOnGround = false; // 떨어지는 중이므로 false로 설정

        // Rigidbody 설정 복원
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