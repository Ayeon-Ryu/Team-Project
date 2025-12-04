using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    public int score1 = 0;
    public int score2 = 0;

   
    public List<GameObject> score1Sprites; // score1 증가용
    public List<GameObject> score2Sprites; // score2 감소용
    public GameObject bbiui;

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
        if (scene.name == "gameoverscene")
        {
            Debug.Log("GameOver scene -> StageManager 제거");

            // 1) 먼저 커서를 보이게 하기
            
            if (player != null)
                Destroy(player.gameObject);

            Destroy(gameObject); // ★ StageManager 완전히 제거

            return;
        }

        // 2) Clear 씬 처리
        if (scene.name == "clearscenereal")
        {
            Debug.Log("Clear scene -> StageManager 동작 중지");
            // 1) 먼저 커서를 보이게 하기
           
            if (player != null)
                Destroy(player.gameObject);

            Destroy(gameObject); // ★ StageManager 완전히 제거

            return;
        }

        // 3) Start 씬 처리
        if (scene.name == "startscene")
        {
            Debug.Log("Start scene -> StageManager 동작 중지");
            // 1) 먼저 커서를 보이게 하기
           
            if (player != null)
                Destroy(player.gameObject);

            Destroy(gameObject); // ★ StageManager 완전히 제거

            return;
        }

        // 4) 나머지는 스테이지 씬만 처리
        if (scene.name.Contains("Stage"))
        {
            StartCoroutine(SetupStage());
        }
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
        if (isGenshou && activeGenshouIndex == 0) // 0이 "이상현상 02"라고 가정
        {
            GameObject screamObj = GameObject.Find("scream");

            if (screamObj != null)
            {
                AudioSource audio = screamObj.GetComponent<AudioSource>();
                if (audio != null)
                {
                    audio.Play();
                    Debug.Log("이상현상 02 → scream 오디오 재생됨");
                }
                else
                {
                    Debug.LogWarning("scream 오브젝트에 AudioSource가 없음!");
                }
            }
            else
            {
                Debug.LogWarning("씬에서 'scream' 오브젝트를 찾을 수 없음!");
            }
        }
        if (isGenshou && activeGenshouIndex == 1) // 0이 "이상현상 02"라고 가정
        {
            GameObject screamObj = GameObject.Find("hill");

            if (screamObj != null)
            {
                AudioSource audio = screamObj.GetComponent<AudioSource>();
                if (audio != null)
                {
                    audio.Play();
                    Debug.Log("이상현상 03 → hill 오디오 재생됨");
                }
                else
                {
                    Debug.LogWarning("scream 오브젝트에 AudioSource가 없음!");
                }
            }
            else
            {
                Debug.LogWarning("씬에서 'scream' 오브젝트를 찾을 수 없음!");
            }
        }
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

    private IEnumerator BlinkUI(GameObject bbiui, int times, float interval)
    {
        for (int i = 0; i < times; i++)
        {
            bbiui.SetActive(false);
            yield return new WaitForSeconds(interval);
            bbiui.SetActive(true);
            yield return new WaitForSeconds(interval);
        }
        bbiui.SetActive(false); // 마지막에는 UI 숨김
    }

    public void OnTriggerHit(bool isFrontTrigger)
    {
        bool correct = (isFrontTrigger && !isGenshou) || (!isFrontTrigger && isGenshou);
        string currentScene = SceneManager.GetActiveScene().name;

        Debug.Log($"OnTriggerHit called | Scene: {currentScene} | Correct: {correct} | ConsecutiveCorrect: {consecutiveCorrect} | ConsecutiveWrong: {consecutiveWrong}");

        // 앞/뒤문 성공/실패 로그
        if (isFrontTrigger && !isGenshou) Debug.Log("앞문 성공");
        else if (!isFrontTrigger && !isGenshou) Debug.Log("뒷문 실패");
        else if (!isFrontTrigger && isGenshou) Debug.Log("뒷문 성공");
        else if (isFrontTrigger && isGenshou) Debug.Log("앞문 실패");

        if (correct)
        {
            consecutiveCorrect++;
            score1 ++; // 성공 점수
            Debug.Log($"성공! 점수 +1 | 현재 점수: {score1}");
            // 오답 수는 유지
            UpdateScoreUI(); // UI 갱신
            if (currentStage >= 8)
            {
                Debug.Log("Stage cleared! Loading Clear scene.");
                SceneManager.LoadScene("clearscenereal");
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
            score2 -= 1; // 실패 점수
            
            Debug.Log($"실패! 점수 -1 | 현재 점수: {score2}");
            Debug.Log($"Wrong answer! ConsecutiveWrong: {consecutiveWrong}");
            UpdateScoreUI(); // UI 갱신
            if (consecutiveWrong >= 3)
            {
                Debug.Log("3회 연속 오답! Loading GameOver scene.");
                SceneManager.LoadScene("gameoverscene");
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
    private void UpdateScoreUI()
    {
        // score1 UI 표시
        for (int i = 0; i < score1Sprites.Count; i++)
        {
            if (score1Sprites[i] != null)
                score1Sprites[i].SetActive(i < score1);
        }

        // score2 UI 표시
        for (int i = 0; i < score2Sprites.Count; i++)
        {
            if (score2Sprites[i] != null)
                score2Sprites[i].SetActive(i < Mathf.Abs(score2)); // score2가 음수니까 절댓값 사용
        }
    }


}
