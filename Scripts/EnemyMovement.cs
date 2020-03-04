using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    //Privatw Variables
    private PlayerDetection playerDetection;
    private NavMeshAgent navMeshAgent;
    private Transform target;
    private Animator animator;
    public bool stopped = false;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerDetection = GetComponent<PlayerDetection>();
        playerDetection.OnDetection += PlayerDetection_OnDetection;
        animator = GetComponentInChildren<Animator>();
    }

    private void PlayerDetection_OnDetection(Transform target)
    {
        //Set this.target to target
        this.target = target;
    }

    private void Update()
    {
        //Check if target doesn't equal null
        if(target != null)
        {
            //Call the SetDestination method using the target.position value
            navMeshAgent.SetDestination(target.position);

            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && Mathf.Approximately(navMeshAgent.velocity.sqrMagnitude, 0f))
            {
                Debug.Log("Enemy at stopping place");
                stopped = true;
            }
            else
            {
                animator.SetBool("StartWalking", true);
                animator.SetBool("FinishedWalking", false);
                stopped = false;
            }
        }
    }

    public void ResetDestination()
    {
        navMeshAgent.ResetPath();
        target = null;
        Destroy(playerDetection);
    }
}
