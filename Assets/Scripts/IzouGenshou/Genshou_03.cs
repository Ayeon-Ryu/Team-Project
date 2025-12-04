using UnityEngine;

public class Genshou_03 : MonoBehaviour
{
    [SerializeField] private GameObject textVisual;
    [SerializeField] private float activateAfterSeconds = 15f;
   private AudioSource bgmSource; //비지엠추가

    private float timer = 0f;
    private bool activated = false;
    private bool destroyed = false;
    private bool isActive = false;

    public bool IsActive => isActive;

    void Awake()
    {
        // 씬에서 "BGM_Player"라는 이름의 게임 오브젝트를 찾습니다.
        GameObject bgmObject = GameObject.Find("hill");

        if (bgmObject != null)
        {
            // 찾은 오브젝트에서 AudioSource 컴포넌트를 가져옵니다.
            bgmSource = bgmObject.GetComponent<AudioSource>();
            Debug.Log("효과음 03재생");
        }
        else
        {
            Debug.LogError("씬에서 이름이 'BGM_Player'인 오브젝트를 찾을 수 없습니다.");
        }
    }

    

    private void SetVisualVisible(bool visible)
    {
        if (textVisual == null) return;

        foreach (var sr in textVisual.GetComponentsInChildren<SpriteRenderer>())
            sr.enabled = visible;
    }

    public void Activate()
    {
        if (this == null || gameObject == null) return;

        activated = true;
        destroyed = false;
        isActive = true;
        SetVisualVisible(true);

        if (gameObject != null)
            gameObject.SetActive(true);
        if (bgmSource != null)
        {
            bgmSource.Play();
        }
    }

    public void Restore()
    {
        if (this == null || gameObject == null) return;

        destroyed = false;
        activated = false;
        timer = 0f;
        isActive = false;
        SetVisualVisible(false);

        if (gameObject != null)
            gameObject.SetActive(false);
    }

    void Update()
    {
        if (destroyed || activated) return;

        timer += Time.deltaTime;
        if (timer >= activateAfterSeconds)
        {
            Activate();
        }
    }
}
