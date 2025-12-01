using UnityEngine;

public class Genshou_02 : MonoBehaviour
{
    [SerializeField] private GameObject faceVisual;
    [SerializeField] private Collider faceCollider;

    private bool hasLooked = false;
    private bool destroyed = false;
    private bool isActive = false;

    public bool IsActive => isActive;

    void Start()
    {
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
