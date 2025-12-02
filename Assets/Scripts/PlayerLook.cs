using UnityEngine;
using System.Collections;
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

    IEnumerator Start()
    {
        yield return null; // 한 프레임 대기

        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (sceneName == "gameoverscene" || sceneName == "startscene" || sceneName == "clearscenereal")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;

        pitch = Mathf.Clamp(pitch, -82f, 82f);

        currentYaw = Mathf.SmoothDamp(currentYaw, yaw, ref yawVelocity, smoothTime);
        currentPitch = Mathf.SmoothDamp(currentPitch, pitch, ref pitchVelocity, smoothTime);

        playerBody.rotation = Quaternion.Euler(0f, currentYaw, 0f);
        transform.localRotation = Quaternion.Euler(currentPitch, 0f, 0f);
    }
}
