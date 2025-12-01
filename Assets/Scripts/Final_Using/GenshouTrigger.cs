using UnityEngine;

public class GenshouTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    public bool isFrontTrigger = true; // true = Front, false = Back

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // StageManager 존재 여부 확인
        if (StageManager.Instance != null)
        {
            StageManager.Instance.OnTriggerHit(isFrontTrigger);
        }
    }
}
