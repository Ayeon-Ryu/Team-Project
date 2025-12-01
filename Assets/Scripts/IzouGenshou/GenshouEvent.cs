using UnityEngine;

public class GenshouEvent : MonoBehaviour
{
    [HideInInspector] public GameObject normalObject;
    [HideInInspector] public GameObject anomalyObject;
    private bool isActive = false;
    public bool IsActive => isActive;

    public void Initialize(GameObject normal, GameObject anomaly)
    {
        normalObject = normal;
        anomalyObject = anomaly;

        if (normalObject != null) normalObject.SetActive(true);
        if (anomalyObject != null) anomalyObject.SetActive(false);
    }

    public void ShowEvent()
    {
        if (normalObject != null) normalObject.SetActive(false);
        if (anomalyObject != null) anomalyObject.SetActive(true);
        isActive = true;
    }

    public void HideEvent()
    {
        if (anomalyObject != null) anomalyObject.SetActive(false);
        if (normalObject != null) normalObject.SetActive(true);
        isActive = false;
    }

    public void Restore() => HideEvent();
}
