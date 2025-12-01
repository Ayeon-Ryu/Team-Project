using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [Header("Player")]
    public GameObject playerPrefab;
    public Transform player;

    [Header("Prefabs")]
    public GameObject normalStagePrefab;
    public GameObject[] genshouPrefabs; // 이상현상 2~6

    [Header("Stage Info")]
    public int currentStage = 1;
    public int consecutiveCorrect = 0;
    public int consecutiveWrong = 0;

    private GameObject currentStageObj;
    private bool isGenshou;
    private int activeGenshouIndex = -1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        EnsurePlayer();

        if (SceneManager.GetActiveScene().name.Contains("Stage"))
            StartCoroutine(SetupStage());
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.Contains("Stage")) return;
        StartCoroutine(SetupStage());
    }

    private void EnsurePlayer()
    {
        if (player != null) return;

        GameObject existingPlayer = GameObject.FindWithTag("Player");
        if (existingPlayer != null)
        {
            player = existingPlayer.transform;
            DontDestroyOnLoad(player.gameObject);
            Debug.Log("Player found in scene: " + player.name);
            return;
        }

        if (playerPrefab != null)
        {
            GameObject newPlayer = Instantiate(playerPrefab);
            player = newPlayer.transform;
            DontDestroyOnLoad(newPlayer);
            Debug.Log("Player instantiated from prefab: " + player.name);
            return;
        }

        Debug.LogError("Player prefab not assigned!");
    }

    private IEnumerator SetupStage()
    {
        yield return null; // 씬 로드 한 프레임 대기

        // Player 확보
        EnsurePlayer();

        // Stage Prefab Instantiate
        LoadRandomStage();

        // 항상 현재 Stage / 이상현상 로그
        Debug.Log($"[SetupStage] Current Stage: {currentStage} | {(isGenshou ? $"이상현상 {activeGenshouIndex + 2}" : "정상 스테이지")}");

        // Player Spawn
        MovePlayerToSpawn();

        // DoorScript 연결
        yield return AssignDoorsNextFrame();
    }

    private void LoadRandomStage()
    {
        if (currentStageObj != null)
            Destroy(currentStageObj);

        int rand = Random.Range(-1, genshouPrefabs.Length);
        GameObject prefabToInstantiate = (rand == -1 || genshouPrefabs.Length == 0) ? normalStagePrefab : genshouPrefabs[rand];

        isGenshou = (prefabToInstantiate != normalStagePrefab);
        activeGenshouIndex = isGenshou ? rand : -1;

        currentStageObj = Instantiate(prefabToInstantiate, prefabToInstantiate.transform.position, prefabToInstantiate.transform.rotation);
        currentStageObj.SetActive(true);

        Debug.Log($"[LoadRandomStage] Stage {currentStage} | {(isGenshou ? $"이상현상 {activeGenshouIndex + 2}" : "정상 스테이지")}");
    }


    private void MovePlayerToSpawn()
    {
        if (currentStageObj == null || player == null) return;

        Transform spawn = currentStageObj.transform.Find("spawn");
        if (spawn == null)
        {
            Debug.LogWarning("Spawn 위치가 없습니다! Prefab 안에 'spawn' 오브젝트를 추가하세요.");
            spawn = currentStageObj.transform;
        }

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        Vector3 centerOffset = cc != null ? new Vector3(0, -cc.center.y, 0) : Vector3.zero;
        player.SetPositionAndRotation(spawn.position + centerOffset, spawn.rotation);

        if (cc != null) cc.enabled = true;
    }

    private IEnumerator AssignDoorsNextFrame()
    {
        yield return null;

        if (currentStageObj == null || player == null) yield break;

        DoorScript[] doors = currentStageObj.GetComponentsInChildren<DoorScript>(true);
        foreach (var door in doors)
        {
            if (door != null)
                door.player = player;
        }
    }

    public void OnTriggerHit(bool isFrontTrigger)
    {
        bool correct = (isFrontTrigger && !isGenshou) || (!isFrontTrigger && isGenshou);
        string currentScene = SceneManager.GetActiveScene().name;

        Debug.Log($"OnTriggerHit called | Scene: {currentScene} | Correct: {correct} | ConsecutiveCorrect: {consecutiveCorrect} | ConsecutiveWrong: {consecutiveWrong}");

        if (correct)
        {
            consecutiveCorrect++;
            // 오답 수는 유지
            if (currentStage >= 8)
            {
                Debug.Log("Stage cleared! Loading Clear scene.");
                SceneManager.LoadScene("Clear");
                return;
            }

            currentStage++;
            Debug.Log($"Correct! Loading Stage{currentStage}Scene");
            SceneManager.LoadScene($"Stage{currentStage}Scene");
        }
        else
        {
            consecutiveWrong++;
            consecutiveCorrect = 0;

            Debug.Log($"Wrong answer! ConsecutiveWrong: {consecutiveWrong}");

            if (consecutiveWrong >= 3)
            {
                Debug.Log("3회 연속 오답! Loading GameOver scene.");
                SceneManager.LoadScene("GameOver");
                return;
            }

            // Stage1Scene-1 / Stage1Scene-2 로직
            if (currentStage == 1 || currentScene.Contains("Stage1Scene"))
            {
                if (consecutiveWrong == 1)
                {
                    currentStage = 1;
                    Debug.Log("1회 오답! Loading Stage1Scene-1");
                    SceneManager.LoadScene("Stage1Scene-1");
                }
                else if (consecutiveWrong == 2)
                {
                    currentStage = 1;
                    Debug.Log("2회 오답! Loading Stage1Scene-2");
                    SceneManager.LoadScene("Stage1Scene-2");
                }
            }
            else // Stage2 이후
            {
                if (consecutiveWrong == 1)
                {
                    currentStage = 1;
                    Debug.Log("Stage2 이후 1회 오답! Loading Stage1Scene-1");
                    SceneManager.LoadScene("Stage1Scene-1");
                }
                else if (consecutiveWrong == 2)
                {
                    currentStage = 1;
                    Debug.Log("Stage2 이후 2회 오답! Loading Stage1Scene-2");
                    SceneManager.LoadScene("Stage1Scene-2");
                }
            }
        }
    }

}
