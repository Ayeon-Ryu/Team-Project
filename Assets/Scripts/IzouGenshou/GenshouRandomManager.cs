using UnityEngine;
using UnityEngine.SceneManagement;

public class GenshouRandomManager : MonoBehaviour
{
    public static GenshouRandomManager Instance;

    [Header("이상현상 이벤트 2~6")]
    public Genshou_02 genshou02;
    public Genshou_03 genshou03;
    public GenshouEvent genshou04;
    public GenshouEvent genshou05;
    public GenshouEvent genshou06;

    [Header("기본 오브젝트")]
    public GameObject exitObject;
    public GameObject professorObject;
    public GameObject deskObject;

    [Header("이상현상 Prefab")]
    public GameObject genshou04Prefab;
    public GameObject genshou05Prefab;
    public GameObject genshou06Prefab;

    [Header("게임 설정")]
    public int totalStages = 8;
    public int life = 3;
    public int currentStage = 1;

    public MonoBehaviour currentEvent = null;
    private bool eventActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 이벤트 오브젝트도 파괴되지 않게 처리
            if (genshou02 != null) DontDestroyOnLoad(genshou02.gameObject);
            if (genshou03 != null) DontDestroyOnLoad(genshou03.gameObject);
            if (genshou04 != null) DontDestroyOnLoad(genshou04.gameObject);
            if (genshou05 != null) DontDestroyOnLoad(genshou05.gameObject);
            if (genshou06 != null) DontDestroyOnLoad(genshou06.gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 4~6번 이벤트 Prefab Instantiate 후 Initialize
        if (genshou04Prefab != null && genshou04 != null)
            genshou04.Initialize(exitObject, Instantiate(genshou04Prefab));
        if (genshou05Prefab != null && genshou05 != null)
            genshou05.Initialize(professorObject, Instantiate(genshou05Prefab));
        if (genshou06Prefab != null && genshou06 != null)
            genshou06.Initialize(deskObject, Instantiate(genshou06Prefab));

        StartRound();
    }

    public void StartRound()
    {
        eventActive = true;

        // 모든 이벤트 초기화
        FullReset();

        // 20% 확률로 이상현상 없음
        if (Random.value < 0.2f)
        {
            currentEvent = null;
            Debug.Log($"Stage {currentStage}: 이상현상 없음");
            return;
        }

        // 이벤트 선택 (2~6)
        int eventIndex = Random.Range(2, 7);

        // 단일 이벤트만 실행
        switch (eventIndex)
        {
            case 2:
                if (genshou02 != null)
                {
                    genshou02.Activate();
                    currentEvent = genshou02;
                }
                break;
            case 3:
                if (genshou03 != null)
                {
                    genshou03.Activate();
                    currentEvent = genshou03;
                }
                break;
            case 4:
                if (genshou04 != null)
                {
                    genshou04.ShowEvent();
                    currentEvent = genshou04;
                }
                break;
            case 5:
                if (genshou05 != null)
                {
                    genshou05.ShowEvent();
                    currentEvent = genshou05;
                }
                break;
            case 6:
                if (genshou06 != null)
                {
                    genshou06.ShowEvent();
                    currentEvent = genshou06;
                }
                break;
        }

        Debug.Log($"Stage {currentStage}: 이벤트 {eventIndex} 실행");
    }

    public void FullReset()
    {
        // 모든 이벤트 비활성화
        if (genshou02 != null) genshou02.Restore();
        if (genshou03 != null) genshou03.Restore();
        if (genshou04 != null) genshou04.Restore();
        if (genshou05 != null) genshou05.Restore();
        if (genshou06 != null) genshou06.Restore();

        currentEvent = null;

        // 기본 오브젝트 복원
        if (exitObject != null) exitObject.SetActive(true);
        if (professorObject != null) professorObject.SetActive(true);
        if (deskObject != null) deskObject.SetActive(true);
    }

    public void FoundCorrect()
    {
        if (!eventActive) return;
        eventActive = false;

        Debug.Log($"Stage {currentStage}: 정답!");
        FullReset();

        currentStage++;
        if (currentStage > totalStages)
            SceneManager.LoadScene("Clear");
        else
            SceneManager.LoadScene($"Stage{currentStage}Scene");
    }

    public void FoundWrong()
    {
        if (!eventActive) return;
        eventActive = false;

        Debug.Log($"Stage {currentStage}: 오답!");
        life--;
        FullReset();

        if (life <= 0)
            SceneManager.LoadScene("GameOver");
        else
        {
            currentStage = 1;
            SceneManager.LoadScene($"Stage{currentStage}Scene");
        }
    }

    public bool HasActiveEvent()
    {
        return (currentEvent != null) &&
               ((currentEvent is Genshou_02 e2 && e2.IsActive) ||
                (currentEvent is Genshou_03 e3 && e3.IsActive) ||
                (currentEvent is GenshouEvent ge && ge.IsActive));
    }
}
