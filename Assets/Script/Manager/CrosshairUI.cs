using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
