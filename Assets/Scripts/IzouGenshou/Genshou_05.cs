using UnityEngine;

public class Genshou_05 : MonoBehaviour
{
    [HideInInspector] public GameObject targetObject; // Manager에서 연결
    private bool isActive = false;

    public bool IsActive => isActive;

    void Start()
    {
        if (targetObject != null)
            targetObject.SetActive(false);
    }

    // 5번 이벤트 활성화
    public void ActivateGlitch()
    {
        RestoreNormal();          // 기존 상태 초기화
        isActive = true;          // 활성화 상태
        if (targetObject != null)
            targetObject.SetActive(true); // 실제 오브젝트 활성화
    }

    // 이벤트 초기화 / 비활성화
    public void RestoreNormal()
    {
        if (targetObject != null)
            targetObject.SetActive(false);

        isActive = false;
    }
}
