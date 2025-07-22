using UnityEngine;
using UnityEngine.UI;

public class WarningUI : MonoBehaviour
{
    public static WarningUI instance;
    public GameObject warningUI;
    public Text warningText;

    private int activeThieves = 0;

    void Awake()
    {
        if(instance == null) { instance = this; }
        else
            Destroy(instance);
    }

    public void OnThiefSpawn()
    {
        activeThieves++;
        UpdateUI();
    }
    public void OnThiefDestroy()
    {
        activeThieves--;
        if (activeThieves < 0) activeThieves = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if(warningUI != null)
            warningUI.SetActive(activeThieves > 0);

        if(warningText!= null)
        {
            if (activeThieves > 0)
                warningText.text = $"Warning! : {activeThieves}명의 도둑 발생!";
            else warningText.text = "";
        }
    }

}
