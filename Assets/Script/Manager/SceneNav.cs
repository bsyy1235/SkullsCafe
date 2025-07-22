using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneNav : MonoBehaviour
{

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    public void ShowPanel(GameObject Panel)
    {
        Panel.SetActive(true);
    }
    public void HidePanel(GameObject Panel)
    {
        Panel.SetActive(false);
    }
}