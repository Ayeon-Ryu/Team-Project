using System.Collections;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [Header("Door Settings")]
    public float openAngle = 90f;
    public float openSpeed = 4f;

    [Header("Player Detection")]
    public Transform player;
    public float activateDistance = 2.5f;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    private bool isOpen = false;
    private Coroutine routine;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }

    void Update()
    {
        // player가 아직 할당되지 않았다면 씬에서 찾아보기
        if (player == null)
        {
            player = GameObject.FindWithTag("Player")?.transform;
            if (player == null) return; // 그래도 없으면 넘어감
        }

        float dist = Vector3.Distance(player.position, transform.position);

        if (!isOpen && dist <= activateDistance)
        {
            if (routine != null) StopCoroutine(routine);
            routine = StartCoroutine(OpenDoor());
        }
        else if (isOpen && dist > activateDistance)
        {
            if (routine != null) StopCoroutine(routine);
            routine = StartCoroutine(CloseDoor());
        }
    }
    
    IEnumerator OpenDoor()
    {
        isOpen = true;

        while (Quaternion.Angle(transform.rotation, openRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, openRotation, openSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = openRotation;
    }

    IEnumerator CloseDoor()
    {
        isOpen = false;

        while (Quaternion.Angle(transform.rotation, closedRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, closedRotation, openSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = closedRotation;
    }
}
