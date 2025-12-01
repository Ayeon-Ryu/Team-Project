using UnityEngine;

public class ComputerController : MonoBehaviour
{
    public GameObject computer;

    void Start()
    {
        computer.SetActive(true);

        var manager = GenshouRandomManager.Instance;
        if (manager != null && manager.genshou06 != null)
        {
            // 06번 이벤트 활성 시 컴퓨터 끄기
            if (manager.genshou06.IsActive)
            {
                computer.SetActive(false);
            }
        }
    }

    // 트리거에서 호출 시 컴퓨터 복구
    public void RestoreComputer()
    {
        if (!computer.activeSelf)
        {
            computer.SetActive(true);
        }
    }
}
