using UnityEngine;
using TMPro;

public class LockboxUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject lockboxPanel;
    public TextMeshProUGUI displayText;
    public GameObject keyObject;

    [Header("Chest")]
    public Animation chestAnimation;
    public string openAnimationName = "ChestAnim";

    [Header("Settings")]
    public string correctCode = "1234";

    private string currentInput = "";
    private bool isUnlocked = false;

    void Start()
    {
        lockboxPanel.SetActive(false);
    }

    public bool IsUIOpen()
    {
        return lockboxPanel.activeSelf;
    }

    public bool IsUnlocked()
    {
        return isUnlocked;
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

            if (chestAnimation != null)
                chestAnimation.Play(openAnimationName);

            Invoke(nameof(RevealKey), 1.2f);
            Invoke(nameof(CloseUI), 2f);
        }
        else
        {
            displayText.text = "WRONG";
            Invoke(nameof(ResetAfterWrong), 1f);
        }
    }

    void RevealKey()
    {
        if (keyObject != null)
            keyObject.SetActive(true);
    }

    void ResetAfterWrong()
    {
        currentInput = "";
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        string display = "";
        for (int i = 0; i < 4; i++)
            display += i < currentInput.Length ? currentInput[i] + " " : "_ ";
        displayText.text = display.Trim();
    }
}