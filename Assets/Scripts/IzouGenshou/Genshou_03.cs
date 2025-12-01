using UnityEngine;

public class Genshou_03 : MonoBehaviour
{
    [SerializeField] private GameObject textVisual;
    [SerializeField] private float activateAfterSeconds = 15f;

    private float timer = 0f;
    private bool activated = false;
    private bool destroyed = false;
    private bool isActive = false;

    public bool IsActive => isActive;

    void Start()
    {
        SetVisualVisible(false);
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
