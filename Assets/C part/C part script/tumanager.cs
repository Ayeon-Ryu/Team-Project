using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;
using System.Collections.Generic;

public class tumanager: MonoBehaviour
{
    
    


   

    [Header("Player")]
    public GameObject playerPrefab;
    public Transform player;

    [Header("Prefabs")]
    public GameObject normalStagePrefab;
    public GameObject[] genshouPrefabs; // 이상현상 2~6

    [Header("Stage Info")]
    public int currentStage = 1;
    
    private GameObject currentStageObj;
    private bool isGenshou;
    private int activeGenshouIndex = -1;



    private void Awake()
    {

  
        LoadRandomStage();





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



        // Stage Prefab Instantiate
        LoadRandomStage();
        // Player 확보
        EnsurePlayer();

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
        GameObject prefabToInstantiate = (rand == -1 || genshouPrefabs.Length == 0) ? normalStagePrefab : normalStagePrefab;

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

    
}
