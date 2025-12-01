using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Genshou_01 : MonoBehaviour
{
    [Header("References")]
    public Transform player;          // 플레이어 Transform
    public Camera playerCam;          // PlayerLook 붙은 카메라
    public Animator animator;         // 교수님 Animator
    public NavMeshAgent agent;        // NavMeshAgent

    [Header("View Settings")]
    public float viewAngle = 45f;     // 시야각
    public float viewDistance = 20f;  // 시야 거리

    private Vector3 startPosition;    // 처음 위치
    private bool isChasing = false;   // 추격 상태
    private bool isReturning = false; // 복귀 상태

    void Start()
    {
        startPosition = transform.position;
        agent.updateRotation = true; // NavMeshAgent가 자동 회전
    }

    void Update()
    {
        // 한 번이라도 바라보면 계속 추격
        if (!isChasing && !isReturning)
        {
            if (PlayerIsLookingAtMe())
                isChasing = true;
        }

        // 이동 목표 설정
        if (isChasing)
            agent.SetDestination(player.position);
        else if (isReturning)
            agent.SetDestination(startPosition);
        else
            agent.SetDestination(transform.position); // 제자리 유지

        // Animator Speed 업데이트
        if (animator != null)
        {
            float speedPercent = agent.velocity.magnitude / (agent.speed > 0 ? agent.speed : 1f);
            animator.SetFloat("Speed", speedPercent, 0.1f, Time.deltaTime);
        }
    }

    private bool PlayerIsLookingAtMe()
    {
        if (!playerCam) return false;

        Collider col = GetComponent<Collider>();
        if (!col) return false;

        Vector3 targetPoint = col.bounds.center;
        Vector3 dirToProfessor = (targetPoint - playerCam.transform.position).normalized;

        // 시야각 체크
        float angle = Vector3.Angle(playerCam.transform.forward, dirToProfessor);
        if (angle > viewAngle) return false;

        // 거리 체크
        float distance = Vector3.Distance(playerCam.transform.position, targetPoint);
        if (distance > viewDistance) return false;

        // 장애물 체크
        if (Physics.Raycast(playerCam.transform.position, dirToProfessor, out RaycastHit hit, viewDistance))
        {
            if (hit.transform != transform) return false;
        }

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ReturnZone"))
        {
            isChasing = false;
            isReturning = true;
            agent.SetDestination(startPosition);
            StartCoroutine(ReturnToStart());
        }
    }

    private IEnumerator ReturnToStart()
    {
        while (Vector3.Distance(transform.position, startPosition) > 0.1f)
        {
            // Animator Speed 계속 업데이트
            if (animator != null)
            {
                float speedPercent = agent.velocity.magnitude / (agent.speed > 0 ? agent.speed : 1f);
                animator.SetFloat("Speed", speedPercent, 0.1f, Time.deltaTime);
            }
            yield return null;
        }

        isReturning = false;
        agent.ResetPath();
    }
}
