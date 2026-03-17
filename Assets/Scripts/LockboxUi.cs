// LockboxUI.cs
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LockboxUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject lockboxPanel;
    public TextMeshProUGUI displayText;
    public GameObject keyObject; // The key inside the lockbox

    [Header("Settings")]
    public string correctCode = "1234";

    private string currentInput = "";
    private bool isUnlocked = false;

    void Start()
    {
        lockboxPanel.SetActive(false);
    }

    public void OpenUI()
    {
        if (!isUnlocked)
        {
            lockboxPanel.SetActive(true);
            currentInput = "";
            UpdateDisplay();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void CloseUI()
    {
        lockboxPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Called by each number button (pass "1", "2", etc.)
    public void PressNumber(string number)
    {
        if (currentInput.Length < 4)
        {
            currentInput += number;
            UpdateDisplay();

            if (currentInput.Length == 4)
                CheckCode();
        }
    }

    public void PressClear()
    {
        currentInput = "";
        UpdateDisplay();
    }

    void CheckCode()
    {
        if (currentInput == correctCode)
        {
            displayText.text = "OPEN!";
            isUnlocked = true;
            keyObject.SetActive(true); // Reveal the key
            Invoke(nameof(CloseUI), 1.5f);
        }
        else
        {
            displayText.text = "WRONG";
            Invoke(nameof(ResetAfterWrong), 1f);
        }
    }

    void ResetAfterWrong()
    {
        currentInput = "";
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        // Show asterisks for entered digits
        displayText.text = currentInput.PadRight(4, '_');
    }
}