using UnityEngine;

public class Genshou_06 : MonoBehaviour
{
    private GameObject deskObject;
    private bool isActive = false;

    // 외부에서 접근 가능한 속성 추가
    public bool IsActive => isActive;

    public void Initialize(GameObject desk)
    {
        deskObject = desk;
        HideEvent();
    }

    public void ShowEvent()
    {
        if (deskObject != null)
            deskObject.SetActive(false);

        isActive = true;
    }

    public void HideEvent()
    {
        if (deskObject != null)
            deskObject.SetActive(true);

        isActive = false;
    }
}
