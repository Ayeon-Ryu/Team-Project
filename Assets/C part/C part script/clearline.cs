using UnityEngine;
using UnityEngine.UI;
using TMPro;   // TextMeshPro 사용
using System.Collections;
using UnityEngine.SceneManagement;

public class clearline : MonoBehaviour
{
    [Header("UI & Text Settings")]
    public GameObject uiPanel;          // 화면에 나타날 UI 패널
    public TMP_Text tmpText;            // TextMeshPro 텍스트
    public Graphic fadeTarget;

    [Header("Text Sequence Settings")]
    public string[] textSequence = { "Umm...",
"Was it a dream?",
"I see.",
"...",
"I can't believe I had such a bad dream.",
"It's bad luck.",
"But, well, it was a dream, so that's fine.",
"That's a relief." };
    private int currentIndex = 0;

    [Header("Timing Settings")]
    public float delayBeforeShow = 12f;  // 화면에 나타나기 전 대기 시간
    public float delayAfterLastLine = 2f;   // 마지막 대사 후 대기 시간
    public float fadeDuration = 3f;
    private bool isActive = false;

    void Start()
    {
        if (uiPanel != null)
            uiPanel.SetActive(false);  // 시작할 때 숨김
        if (fadeTarget != null)
        {
            Color c = fadeTarget.color;
            fadeTarget.color = new Color(c.r, c.g, c.b, 0f); // 완전히 투명
        }
            Invoke(nameof(ShowUI), delayBeforeShow);
    }

    void ShowUI()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(true);
            isActive = true;
            if (tmpText != null)
                tmpText.text = textSequence[0]; // 첫 번째 텍스트
        }
    }

    void Update()
    {
        if (isActive && Input.GetMouseButtonDown(0)) // 화면 클릭 시
        {
            AdvanceText();
        }
    }

    void AdvanceText()
    {
        currentIndex++;
        if (currentIndex >= textSequence.Length)
        {
            currentIndex = textSequence.Length - 1; // 마지막 텍스트에서 멈춤
            // 또는 반복하려면 currentIndex = 0;
            if (fadeTarget != null)
                StartCoroutine(clearfadein()); // ★ 마지막 대사 후 페이드 인 시작
            return; // 마지막 대사면 더 이상 진행하지 않음
        }

        if (tmpText != null)
            tmpText.text = textSequence[currentIndex];
    }
    private IEnumerator clearfadein()
    {
        // 페이드 인 시작 전 알파 초기화
        Color c = fadeTarget.color;
        fadeTarget.color = new Color(c.r, c.g, c.b, 0f);

        // 마지막 대사 후 대기
        yield return new WaitForSeconds(delayAfterLastLine);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            fadeTarget.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        fadeTarget.color = new Color(c.r, c.g, c.b, 1f); // 확실하게 1로 설정
                                                         // ★ 페이드 완료 후 3초 뒤 씬 전환
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("startscene");
    }
}
