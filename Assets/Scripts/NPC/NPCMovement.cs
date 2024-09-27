using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCMovement : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent agent;
    public float turnSpeed = 10f;

    private UnityEngine.Events.UnityAction reachedTarget;


    [SerializeField] private Animator animator;

    protected virtual void Awake()
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

    private void Update()
    {
        Animate();

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

        Debug.Log("reachedTarget: " + reachedTarget.ToString());
    }


    private void Animate()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
