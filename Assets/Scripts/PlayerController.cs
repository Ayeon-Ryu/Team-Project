using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float runSpeed = 9f;
    [SerializeField] float jumpHeight = 1.4f;
    [SerializeField] float gravity = -20f;

    private CharacterController controller;
    private float yVelocity; // 위아래 속도 저장

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // --- 이동 입력 ---
        float xValue = Input.GetAxis("Horizontal");
        float zValue = Input.GetAxis("Vertical");

        // 달리기
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : moveSpeed;

        // 방향 벡터 만들기
        Vector3 move = transform.right * xValue + transform.forward * zValue;
        move *= currentSpeed;

        // --- 점프 ---
        if (controller.isGrounded)
        {
            if (yVelocity < 0)
                yVelocity = -2f; // 땅에 붙여주기

            if (Input.GetKeyDown(KeyCode.Space))
            {
                yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        // --- 중력 ---
        yVelocity += gravity * Time.deltaTime;

        // Y 속도 적용
        move.y = yVelocity;

        // --- 최종 이동 ---
        controller.Move(move * Time.deltaTime);
    }
}
