using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float intensity = 0.1f;   // 흔들림 강도
    public float duration = 0.5f;    // 흔들림 지속 시간

    private Vector3 originalPos;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    public void PlayShake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;

            transform.localPosition = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
