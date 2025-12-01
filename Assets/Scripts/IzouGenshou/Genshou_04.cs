using UnityEngine;

public class Genshou_04 : MonoBehaviour
{
    [HideInInspector] public GameObject targetObject; // Manager에서 연결
    private bool isActive = false;

    public bool IsActive => isActive;

    void Start()
    {
        if (targetObject != null)
            targetObject.SetActive(false);
    }

    public void ActivateGlitch()
    {
        if (targetObject != null)
            targetObject.SetActive(true);

        isActive = true;
    }

    public void RestoreNormal()
    {
        if (targetObject != null)
            targetObject.SetActive(false);

        isActive = false;
    }
}
