using UnityEngine;

public class GenshouManager : MonoBehaviour
{
    // 각 단계 완료 여부
    private bool genshou02Done = false;
    private bool genshou03Done = false;
    private bool genshou04Done = false;
    private bool genshou05Done = false;

    // 모든 단계 완료 여부
    public bool AllCompleted =>
        genshou02Done && genshou03Done && genshou04Done && genshou05Done;

    // 특정 단계까지의 완료 여부 확인
    public bool IsCompletedUntil(int stage)
    {
        if (stage >= 2 && !genshou02Done) return false;
        if (stage >= 3 && !genshou03Done) return false;
        if (stage >= 4 && !genshou04Done) return false;
        if (stage >= 5 && !genshou05Done) return false;

        return true;
    }

    // ----- 단계별 완료 함수 -----

    public void Notify02()
    {
        genshou02Done = true;
        Debug.Log("Genshou02 Done");
    }

    public void Notify03()
    {
        genshou03Done = true;
        Debug.Log("Genshou03 Done");
    }

    public void Notify04()
    {
        genshou04Done = true;
        Debug.Log("Genshou04 Done");
    }

    public void Notify05()
    {
        genshou05Done = true;
        Debug.Log("Genshou05 Done");
    }
    private void Update()
    {
        // 상태 확인용 (테스트 끝나면 삭제하면 됨)
        Debug.Log($"02:{genshou02Done}, 03:{genshou03Done}, 04:{genshou04Done}, 05:{genshou05Done}");
    }

}
