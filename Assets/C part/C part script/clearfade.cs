using UnityEngine;
using UnityEngine.UI;  // UI 관련
using System.Collections;

public class clearfade : MonoBehaviour
{
    [Header("Fade Settings")]
    public float fadeDuration = 2f;  // 페이드에 걸리는 시간
    public float delayBeforeFade = 2f; // 시작 후 대기 시간

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
            StartCoroutine(FadeAfterDelay());
    }

    private IEnumerator FadeAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeFade);

        Color originalColor = uiElement.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            uiElement.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        uiElement.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        gameObject.SetActive(false); // 페이드 끝나면 오브젝트 비활성화
    }
}
