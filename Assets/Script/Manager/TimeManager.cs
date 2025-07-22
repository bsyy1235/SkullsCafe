using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public Image timer;
    public Text timerText;
    public float totalTime = 180f;
    public int coin = 30;

    public GameObject pauseMenuUI;
    public static bool isPaused = false;

    public GameObject howToPlayPanel;
    public GameObject checkPanel;
    private bool isRed = false;

    private RectTransform timerTransform;
    private bool isPulsing = false;

    AudioSource audioSource;
    public AudioClip audioClip;

    public string ClearScene = "ClearScreen";
    public string GameOverScene = "GameOver";

    void Start()
    {
        if (timer != null)
            timerTransform = timer.GetComponent<RectTransform>();
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }

        if (!isPaused && totalTime > 0)
        {
            totalTime -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(totalTime / 60);
            int seconds = Mathf.FloorToInt(totalTime % 60);

            timerText.text = string.Format("{0}:{1:D2}", minutes, seconds);

            if (totalTime <= 6f && !isPulsing)
            {
                isPulsing = true;
                StartCoroutine(PulseTimer());
                StartCoroutine(PlayAudioLoop());

            }

            if (totalTime <= 10f && !isRed)
            {
                SetColor();
            }
        }
        else if (totalTime <= 0)
        {
            timerText.text = "0:00";
            if (CoinManager.instance.coinCount >= coin)
                SceneManager.LoadScene(ClearScene);
            else
                SceneManager.LoadScene(GameOverScene);
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        howToPlayPanel.SetActive(false);
        checkPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SetColor()
    {
        isRed = true;
        timerText.color = Color.red;
        if (timer != null)
            timer.color = Color.red;
    }

    private IEnumerator PulseTimer()
    {
        Vector3 originalScale = timerTransform.localScale;
        Vector3 targetScale = originalScale * 1.5f;

        while (totalTime > 0f)
        {
            // 커지기
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * 2f;
                timerTransform.localScale = Vector3.Lerp(originalScale, targetScale, t);
                yield return null;

            }

            // 줄어들기
            t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * 2f;
                timerTransform.localScale = Vector3.Lerp(targetScale, originalScale, t);
                yield return null;
            }

            yield return null;
        }
    }

    private IEnumerator PlayAudioLoop()
    {
        while (totalTime > 0f)
        {
            audioSource.PlayOneShot(audioClip);
            yield return new WaitForSeconds(audioClip.length); // 클립 길이만큼 대기
        }
    }

    public void ShowPlayPanel()
    {
        howToPlayPanel.SetActive(true);
    }
    public void HidePlayPanel()
    {
        howToPlayPanel.SetActive(false);
    }
    public void ShowCheckPanel()
    {
        checkPanel.SetActive(true);
    }
    public void HideCheckPanel()
    {
        checkPanel.SetActive(false);
    }
}