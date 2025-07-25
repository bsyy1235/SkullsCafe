using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public bool weaponInHand = false;
    public bool isAttacking = false;

    public GameObject weaponCollider;

    void Update()
    {
        if (weaponInHand && Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
            //isAttacking = true;
        }

    }

    public void StartAttack()
    {
        Debug.Log("StartAttack");
        isAttacking = true;
    }

    public void SetWeaponInHand(bool state)
    {
        weaponInHand = state;
    }

    public void EndAttack()
    {
        Debug.Log("EndAttack");
        isAttacking = false;
    }
}
