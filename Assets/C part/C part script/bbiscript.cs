using UnityEngine;
using UnityEngine.UI;

public class bbiscript : MonoBehaviour
{
    public float blinkDuration = 5f; // 점멸 지속 시간
    public float blinkSpeed = 5f;    // 점멸 속도
    private Graphic uiElement;       // Image, Text 등 Graphic 기반 UI
    private float timer;

    void Start()
    {
        uiElement = GetComponent<Graphic>();
        if (uiElement == null)
        {
            Debug.LogError("UI 오브젝트에 Graphic 컴포넌트가 필요합니다 (Image, Text 등).");
            return;
        }
        timer = 0f;
    }

    void Update()
    {
        if (timer < blinkDuration)
        {
            // 점멸 효과: alpha를 사인 함수로 변화
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
            Color c = uiElement.color;
            c.a = alpha;
            uiElement.color = c;

            timer += Time.deltaTime;
        }
        else
        {
            // 점멸 끝나면 오브젝트 비활성화
            uiElement.gameObject.SetActive(false);
        }
    }
}
