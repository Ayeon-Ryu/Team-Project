using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 300f;
    public float smoothTime = 0.05f;
    public Transform playerBody;

    float pitch;
    float yaw;

    float currentPitch;
    float currentYaw;
    float pitchVelocity;
    float yawVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;

        // 오버워치식 pitch 제한
        pitch = Mathf.Clamp(pitch, -82f, 82f);

        // 부드러운 보간 → 카메라 감각을 오버워치처럼 만들기
        currentYaw = Mathf.SmoothDamp(currentYaw, yaw, ref yawVelocity, smoothTime);
        currentPitch = Mathf.SmoothDamp(currentPitch, pitch, ref pitchVelocity, smoothTime);

        playerBody.rotation = Quaternion.Euler(0f, currentYaw, 0f);
        transform.localRotation = Quaternion.Euler(currentPitch, 0f, 0f);
    }
}
