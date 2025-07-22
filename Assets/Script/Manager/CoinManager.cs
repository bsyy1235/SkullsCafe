using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    public int coinCount = 0;
    public Text coinText;
    public GameObject AddCoinObject;
    public Text AddCoinText;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        UpdateUI();
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;

        if(coinCount < 0)
            coinCount = 0;

        AddCoinText.text = (amount>0) ? $"+{amount}" : $"{amount}";
        StartCoroutine(ShowAddCoinText());

        UpdateUI();
    }

    private IEnumerator ShowAddCoinText()
    {
        AddCoinObject.SetActive(true);
        yield return new WaitForSeconds(1);
        AddCoinObject.SetActive(false);
    }



    public void UpdateUI()
    {
        if(coinText != null)
            coinText.text = coinCount.ToString();
    }
}
