using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float openAngle = 90f;
    public float openSpeed = 2f;

    private bool isUnlocked = false;
    private bool isOpen = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        closedRot = transform.rotation;
        openRot = Quaternion.Euler(
            transform.eulerAngles + Vector3.up * openAngle);
    }

    void Update()
    {
        if (isOpen)
            transform.rotation = Quaternion.Lerp(
                transform.rotation, openRot, Time.deltaTime * openSpeed);
    }

    public void UnlockDoor()
    {
        isUnlocked = true;
    }

    public void TryOpen()
    {
        if (isUnlocked)
            isOpen = true;
        else
            Debug.Log("The door is locked!");
    }
}