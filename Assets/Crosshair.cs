using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [Header("Crosshair")]
    public Color color = Color.white;
    public int size = 10;
    public int thickness = 2;
    public int gap = 4;

    private Texture2D dot;

    void Start()
    {
        dot = new Texture2D(1, 1);
        dot.SetPixel(0, 0, Color.white);
        dot.Apply();
    }

    void OnGUI()
    {
        float cx = Screen.width  / 2f;
        float cy = Screen.height / 2f;

        GUI.color = color;

        // Left bar
        GUI.DrawTexture(new Rect(cx - gap - size, cy - thickness / 2f, size, thickness), dot);
        // Right bar
        GUI.DrawTexture(new Rect(cx + gap,         cy - thickness / 2f, size, thickness), dot);
        // Top bar
        GUI.DrawTexture(new Rect(cx - thickness / 2f, cy - gap - size, thickness, size), dot);
        // Bottom bar
        GUI.DrawTexture(new Rect(cx - thickness / 2f, cy + gap,         thickness, size), dot);
    }
}