using UnityEngine;
using UnityEngine.UI;  // UI 관련
using System.Collections;

public class clearfadein : MonoBehaviour
{
    [Header("Fade In Settings")]
    public float fadeDuration = 2f;  // 페이드 인에 걸리는 시간
    public float delayBeforeFade = 0f; // 시작 후 대기 시간

    private Graphic uiElement; // Image, Text 등 공통 부모

    void Awake()
    {
        uiElement = GetComponent<Graphic>();
        if (uiElement == null)
        {
            Debug.LogError("이 스크립트는 Image나 Text 등 Graphic 컴포넌트가 있어야 합니다!");
        }
    }

    void Start()
    {
        if (uiElement != null)
        {
            // 페이드 인 시작 전 알파를 0으로 초기화
            Color c = uiElement.color;
            uiElement.color = new Color(c.r, c.g, c.b, 0f);

            StartCoroutine(FadeInAfterDelay());
        }
    }

    private IEnumerator FadeInAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeFade);

        Color originalColor = uiElement.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            uiElement.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        uiElement.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
    }
}
