using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent agent;
    public float turnSpeed = 10f;

    private UnityEngine.Events.UnityAction reachedTarget;


    [SerializeField] private Animator animator;

    void Start()
    {
        if (!animator)
        {
            animator = GetComponentInChildren<Animator>();
        }
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetTarget(Transform _target, UnityEngine.Events.UnityAction _reachedTarget = null)
    {
        target = _target;
        agent.destination = _target.position;

        reachedTarget = null;
        reachedTarget = _reachedTarget;
    }

    void Update()
    {
        Animates();

        if (target == null)
        {
            return;
        }

        if (agent.remainingDistance < 0.3f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, turnSpeed * Time.deltaTime);

            if (reachedTarget != null)
            {
                reachedTarget.Invoke();
                reachedTarget = null;
            }
        }

        Debug.DrawLine(transform.position, target.position, Color.red);
    }


    private void Animates()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
