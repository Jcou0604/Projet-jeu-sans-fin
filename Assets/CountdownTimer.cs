using UnityEngine;
using TMPro;
using StarterAssets;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CountdownTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float totalTime = 300f;

    [Header("UI References")]
    public TMP_Text timerText;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject abandonPanel;

    [Header("Player Reference")]
    public GameObject playerCapsule;

    private float timeRemaining;
    private bool isGameOver = false;

    private FirstPersonController fpsController;
    private StarterAssetsInputs starterAssetsInputs;
    private PlayerInput playerInput;

    void Start()
    {
        timeRemaining = totalTime;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (abandonPanel != null) abandonPanel.SetActive(false);

        if (playerCapsule != null)
        {
            fpsController = playerCapsule.GetComponent<FirstPersonController>();
            starterAssetsInputs = playerCapsule.GetComponent<StarterAssetsInputs>();
            playerInput = playerCapsule.GetComponent<PlayerInput>();
        }

        LockCursor();
    }

    void Update()
    {
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RestartGame();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                Abandonner();
            }
            return;
        }

        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay(timeRemaining);
        }
        else
        {
            timeRemaining = 0f;
            UpdateTimerDisplay(0f);
            TriggerGameOver();
        }
    }

    void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
        timerText.color = time <= 30f ? Color.red : Color.white;
    }

    void TriggerGameOver()
    {
        isGameOver = true;
        if (timerText != null) timerText.gameObject.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        DisablePlayer();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void TriggerWin()
    {
        isGameOver = true;
        if (timerText != null) timerText.gameObject.SetActive(false);
        if (winPanel != null) winPanel.SetActive(true);
        DisablePlayer();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Abandonner()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (abandonPanel != null) abandonPanel.SetActive(true);
    }

    void DisablePlayer()
    {
        if (fpsController != null) fpsController.enabled = false;
        if (playerInput != null) playerInput.enabled = false;
        if (starterAssetsInputs != null)
        {
            starterAssetsInputs.cursorLocked = false;
            starterAssetsInputs.cursorInputForLook = false;
            starterAssetsInputs.enabled = false;
        }
    }

    void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}