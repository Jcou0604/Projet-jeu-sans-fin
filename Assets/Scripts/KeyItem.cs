// KeyItem.cs
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    public string keyID = "LockboxKey"; // Use multiple keys/doors easily
    public float bobSpeed = 2f;
    public float bobHeight = 0.1f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Floating bob animation
        transform.position = startPos + Vector3.up *
            Mathf.Sin(Time.time * bobSpeed) * bobHeight;
    }

    public void Pickup()
    {
        PlayerInventory.Instance.AddKey(keyID);
        Destroy(gameObject);
    }
}