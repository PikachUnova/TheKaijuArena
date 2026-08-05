using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class WanderingNPC : MonoBehaviour
{
    [Header("Wandering")]
    [SerializeField] private float minWaitTime = 2f;
    [SerializeField] private float maxWaitTime = 10f;

    private Vector3 homePosition;
    [SerializeField] private float minWanderDistance = 3f;
    [SerializeField] private float maxWanderDistance = 15f;


    [SerializeField] private float walkSpeed = 3.0f;

    private NavMeshAgent agent;
    private Animator animator;
    [SerializeField] private bool isStationary = false;
    private bool isWaiting;

    private void Start()
    {
        homePosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = walkSpeed;
        StartCoroutine(WanderRoutine());
    }

    private void Update()
    {
        if (agent.velocity.magnitude > 0.1f)
        {
            SetLocomotive(0.5f); // Walking
        }
        else
        {
            SetLocomotive(0f);   // Idle
        }
    }

    private IEnumerator WanderRoutine()
    {
        while (!isStationary)
        {
            if (!isWaiting)
            {
                float distance = Random.Range(minWanderDistance, maxWanderDistance);
                Vector3 randomPoint = Random.insideUnitSphere * distance;
                randomPoint += transform.position;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, distance, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }

                isWaiting = true;

                while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
                {
                    yield return null;
                }
                float wait = Random.Range(minWaitTime, maxWaitTime);
                yield return new WaitForSeconds(wait);

                isWaiting = false;
            }

            yield return null;
        }
    }

    public void SetLocomotive(float magnitude)
    {
        animator.SetFloat("MovementSpeed", magnitude, .2f, Time.deltaTime);
    }
}
