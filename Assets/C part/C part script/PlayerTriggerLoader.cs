using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTriggerLoader : MonoBehaviour
{
    // 인스펙터에서 Trigger 오브젝트를 직접 연결
    public GameObject triggerObject;

    private void Awake()
    {
        Debug.Log("시작됨");
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("트리거 작동됨");
        SceneManager.LoadScene("Stage1Scene");
        // 충돌한 오브젝트가 triggerObject인지 확인
        if (other.gameObject == triggerObject)
        {
            Debug.Log("충돌");
            // Stage1Scene으로 씬 전환
            SceneManager.LoadScene("Stage1Scene");
        }
    }
}
