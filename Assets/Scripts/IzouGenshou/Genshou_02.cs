using UnityEngine;

public class Genshou_02 : MonoBehaviour
{
    [SerializeField] private GameObject faceVisual;
    [SerializeField] private Collider faceCollider;
    private AudioSource bgmSource; //비지엠추가

    private bool hasLooked = false;
    private bool destroyed = false;
    private bool isActive = false;

    public bool IsActive => isActive;

    void Awake()
    {
        // 씬에서 "BGM_Player"라는 이름의 게임 오브젝트를 찾습니다.
        GameObject bgmObject = GameObject.Find("scream");

        if (bgmObject != null)
        {
            // 찾은 오브젝트에서 AudioSource 컴포넌트를 가져옵니다.
            bgmSource = bgmObject.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogError("씬에서 이름이 'BGM_Player'인 오브젝트를 찾을 수 없습니다.");
        }
    }

    void Start()
    {
        if (bgmSource != null)
        {
            bgmSource.Play();
        }
        SetSpriteVisible(false);
    }

    private void SetSpriteVisible(bool visible)
    {
        if (faceVisual == null) return;

        foreach (var sr in faceVisual.GetComponentsInChildren<SpriteRenderer>())
            sr.enabled = visible;
    }

    public void Activate()
    {
        if (this == null || gameObject == null) return;

        hasLooked = true;
        destroyed = false;
        isActive = true;
        SetSpriteVisible(true);

        if (gameObject != null)
            gameObject.SetActive(true);

        
    }

    public void Restore()
    {
        if (this == null || gameObject == null) return;

        destroyed = false;
        hasLooked = false;
        isActive = false;
        SetSpriteVisible(false);

        if (gameObject != null)
            gameObject.SetActive(false);
    }
}
