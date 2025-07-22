using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
    public void gotoScene(string sName)
    {
        SceneManager.LoadScene(sName);
    }
}
