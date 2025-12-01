using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DebugKey : MonoBehaviour
{
    private static DebugKey instance;

    private void Awake()
    {
        // 중복 방지
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환에도 유지
    }

    void Update()
    {
        // N 키 → 다음 스테이지
        if (Input.GetKeyDown(KeyCode.N))
        {
            StageManager.Instance?.OnTriggerHit(true);
        }

        // I 키 → 틀린 횟수 증가
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (StageManager.Instance != null)
            {
                StageManager.Instance.consecutiveWrong++;
                Debug.Log("consecutiveWrong = " + StageManager.Instance.consecutiveWrong);

                if (StageManager.Instance.consecutiveWrong >= 3)
                {
                    SceneManager.LoadScene("GameOverScene");
                }
            }
        }

       
    }

    
}
