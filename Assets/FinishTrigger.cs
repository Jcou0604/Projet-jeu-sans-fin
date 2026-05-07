using UnityEngine;

public class FinishTrigger : MonoBehaviour
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