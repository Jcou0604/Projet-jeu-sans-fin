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

    void Update()
    {
        // Si UI pas ouverte ou déjà déverrouillé → rien faire
        if (!lockboxPanel.activeSelf || isUnlocked) return;

        // Lire chiffres clavier (0 à 9)
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                AddDigit(i.ToString());
            }
        }

        // Numpad (optionnel)
        if (Input.GetKeyDown(KeyCode.Keypad0)) AddDigit("0");
        if (Input.GetKeyDown(KeyCode.Keypad1)) AddDigit("1");
        if (Input.GetKeyDown(KeyCode.Keypad2)) AddDigit("2");
        if (Input.GetKeyDown(KeyCode.Keypad3)) AddDigit("3");
        if (Input.GetKeyDown(KeyCode.Keypad4)) AddDigit("4");
        if (Input.GetKeyDown(KeyCode.Keypad5)) AddDigit("5");
        if (Input.GetKeyDown(KeyCode.Keypad6)) AddDigit("6");
        if (Input.GetKeyDown(KeyCode.Keypad7)) AddDigit("7");
        if (Input.GetKeyDown(KeyCode.Keypad8)) AddDigit("8");
        if (Input.GetKeyDown(KeyCode.Keypad9)) AddDigit("9");

        // Effacer avec Backspace
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            currentInput = "";
            UpdateDisplay();
        }

        // Valider avec Enter (optionnel)
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckCode();
        }
    }

    // 🔥 IMPORTANT pour PlayerInteraction
    public bool IsUIOpen()
    {
        return lockboxPanel.activeSelf;
    }

    public bool IsUnlocked()
    {
        return isUnlocked;
    }

    void AddDigit(string number)
    {
        if (currentInput.Length < 4)
        {
            currentInput += number;
            UpdateDisplay();

            if (currentInput.Length == 4)
                CheckCode();
        }
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