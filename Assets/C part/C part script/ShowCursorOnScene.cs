using UnityEngine;

public class ShowCursorOnScene : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None; // 마우스 락 해제
        Cursor.visible = true;                  // 마우스 커서 표시
    }
}
