using UnityEngine;
using UnityEngine.SceneManagement;

public class gamestartscript : MonoBehaviour
{
    // Inspector에서 다음 씬의 이름을 지정합니다. (예: "GameScene", "Level1")
    public string targetSceneName = "SampleScene";

    // 버튼 역할을 할 오브젝트에 부여할 태그 이름
    private const string ButtonTag = "gamestartbutton";

    void Update()
    {
        // 1. 마우스 왼쪽 버튼을 눌렀을 때만 실행
        if (Input.GetMouseButtonDown(0))
        {
            HandleRaycastClick();
        }
    }

    void HandleRaycastClick()
    {
        // 현재 마우스 위치에서 카메라를 통해 광선(Ray) 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit; // 충돌 정보를 저장할 변수

        // 2. 광선 발사 및 충돌 검사
        // 광선이 씬의 어떤 콜라이더와 충돌했다면 'hit'에 정보가 저장되고 true 반환
        if (Physics.Raycast(ray, out hit))
        {
            // 3. 충돌한 오브젝트가 버튼인지 확인
            // 충돌한 오브젝트의 태그가 미리 정의된 'ButtonTag'와 일치하는지 확인
            if (hit.transform.CompareTag(ButtonTag))
            {
                Debug.Log($"버튼 클릭 감지! 오브젝트 이름: {hit.transform.name}");

                // 4. 버튼이 맞다면 씬 전환 함수 호출
                LoadNextScene();
            }
            else
            {
                // 버튼이 아닌 다른 오브젝트에 닿았을 때 (선택 사항)
                Debug.Log($"버튼이 아닌 다른 오브젝트({hit.transform.name}) 클릭.");
            }
        }
    }

    void LoadNextScene()
    {
        // 지정된 씬 이름으로 씬을 로드합니다.
        SceneManager.LoadScene(targetSceneName);
    }
}
