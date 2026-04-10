using UnityEngine;

public class KeyFloatUp : MonoBehaviour
{
    public float appearDelay = 0.5f;
    public float floatHeight = 0.5f;
    public float floatSpeed = 0.8f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isFloating = false;
    private float timer = 0f;
    private bool waiting = true;

    void OnEnable()
    {
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.up * floatHeight;
        isFloating = false;
        waiting = true;
        timer = 0f;
        transform.position = startPosition;
    }

    void Update()
    {
        if (waiting)
        {
            timer += Time.deltaTime;
            if (timer >= appearDelay)
            {
                waiting = false;
                isFloating = true;
            }
        }

        if (isFloating)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                Time.deltaTime * floatSpeed
            );

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isFloating = false;
            }
        }
    }
}