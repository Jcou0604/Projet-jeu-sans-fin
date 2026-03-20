using UnityEngine;
using TMPro;
using StarterAssets; // Required for disabling player input

public class CountdownTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float totalTime = 300f; // 5 minutes

    [Header("UI References")]
    public TMP_Text timerText;
    public TMP_Text gameOverText;
    public string gameOverMessage = "Time's Up! Game Over.";

    [Header("Player Reference")]
    public GameObject playerCapsule; // Drag PlayerCapsule here

    private float timeRemaining;
    private bool isGameOver = false;

    // Reference to the First Person input controller
    private FirstPersonController fpsController;

    void Start()
    {
        timeRemaining = totalTime;

        // Hide game over text at start
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);

        // Get the FirstPersonController component from the player
        if (playerCapsule != null)
            fpsController = playerCapsule.GetComponent<FirstPersonController>();
    }

    void Update()
    {
        if (isGameOver) return;

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
        time = Mathf.Max(0, time);

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        // Display MM:SS format
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Turn timer red under 30 seconds
        if (time <= 30f)
            timerText.color = Color.red;
    }

    void TriggerGameOver()
    {
        isGameOver = true;

        // Show game over message
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = gameOverMessage;
        }

        // Hide the timer
        if (timerText != null)
            timerText.gameObject.SetActive(false);

        // Disable player movement
        if (fpsController != null)
            fpsController.enabled = false;

        // Unlock and show the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Pause the game
        Time.timeScale = 0f;

        Debug.Log("Game Over — Timer reached zero.");
    }

    public void RestartGame()
    {
        timeRemaining = totalTime;
        isGameOver = false;
        Time.timeScale = 1f;

        // Re-enable player movement
        if (fpsController != null)
            fpsController.enabled = true;

        // Lock cursor again for FPS
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);

        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
            timerText.color = Color.white; // Reset color
        }
    }
}
