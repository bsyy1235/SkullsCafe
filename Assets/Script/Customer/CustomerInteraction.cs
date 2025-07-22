using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CustomerInteraction : MonoBehaviour
{
    public string requiredItem;
    public Animator animator;
    public CustomerController controller;

    public bool hasInteracted = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ReceiveItem(GameObject item)
    {
        if (hasInteracted) return;
        hasInteracted=true;

        string itemName = item.name.Replace("(Clone)", "").Trim();

        if (itemName == requiredItem)
        {
            Debug.Log("정답 아이템 전달");
            animator.SetTrigger("Happy");
            CoinManager.instance.AddCoin(1);
            controller.DeleteRequestUI();
        }
        else
        {
            Debug.Log("틀린 아이템 전달:"+itemName+requiredItem);
            animator.SetBool("Annoyed", true);
            controller.ShowAngryUI();
        }

        Destroy(item);
        Invoke(nameof(ResumeAfterInteraction),0.5f);
    }
    void ResumeAfterInteraction()
    {
        controller.ResumeMovement();
    }

}
