using UnityEngine;

public class Trigger : MonoBehaviour
{
    public enum TriggerType { Front, Back }
    public TriggerType triggerType;

    private GenshouRandomManager manager;

    private void Start()
    {
        manager = GenshouRandomManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (manager == null) return;

        bool hasEvent = (manager.currentEvent != null) || (manager.genshou06.IsActive);

        // 앞/뒤 트리거 판정
        if (triggerType == TriggerType.Front)
        {
            if (hasEvent)
                manager.FoundWrong();  // 이상현상 있음 → 앞 = 오답
            else
                manager.FoundCorrect(); // 이상현상 없음 → 앞 = 정답
        }
        else // Back
        {
            if (hasEvent)
                manager.FoundCorrect(); // 이상현상 있음 → 뒤 = 정답
            else
                manager.FoundWrong();   // 이상현상 없음 → 뒤 = 오답
        }

        // 06번 이벤트 종료
        manager.genshou06?.HideEvent();

        // 2~5 이벤트 종료
       // if (manager.currentEvent != null)
           // manager.currentEvent.SetActive(false);
    }
}
