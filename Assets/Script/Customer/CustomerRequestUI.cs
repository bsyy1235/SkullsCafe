using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class CustomerRequestUI : MonoBehaviour
{
    public Image menuIcon;
    public GameObject anger;

    public void SetMenu(Sprite sprite)
    {
        if (menuIcon != null)
        {
            menuIcon.sprite = sprite;
        }
        if(anger != null)
        {
            anger.SetActive(false);
        }
    }
    public void showAngerIcon()
    {
        if (anger != null)
        {
            menuIcon.sprite = null;
            anger.SetActive(true);
        }
    }

}
