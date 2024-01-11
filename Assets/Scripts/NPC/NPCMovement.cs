using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class NPCMovement : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    public float turnSpeed = 10f;


    [SerializeField] private Animator animator;

    void Start()
    {
        if (!animator)
        {
            animator = GetComponentInChildren<Animator>();
        }
        agent = GetComponent<NavMeshAgent>();
        if (target != null) 
        {
            agent.destination = target.position;
        }
    }

    void Update()
    {
        Animates();

        if (agent.remainingDistance < 0.3f)
        {
            agent.destination = target.position;
            agent.speed = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, turnSpeed * Time.deltaTime);
        }

    }
    private void Animates()
    {
        animator.SetFloat("Speed", agent.speed);

    }
}
