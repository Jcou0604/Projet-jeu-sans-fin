using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public CountdownTimer countdownTimer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            countdownTimer.TriggerWin();
        }
    }
}