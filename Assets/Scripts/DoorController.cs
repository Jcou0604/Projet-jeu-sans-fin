// DoorController.cs
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public string requiredKeyID = "LockboxKey";
    public float openAngle = 90f;
    public float openSpeed = 2f;

    private bool isOpen = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        closedRot = transform.rotation;
        openRot = Quaternion.Euler(transform.eulerAngles + Vector3.up * openAngle);
    }

    void Update()
    {
        if (isOpen)
            transform.rotation = Quaternion.Lerp(
                transform.rotation, openRot, Time.deltaTime * openSpeed);
    }

    public void TryOpen()
    {
        if (PlayerInventory.Instance.HasKey(requiredKeyID))
        {
            isOpen = true;
            Debug.Log("Door opened!");
        }
        else
        {
            Debug.Log("You need a key!");
        }
    }
}