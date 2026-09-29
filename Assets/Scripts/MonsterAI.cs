using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MonsterAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] patrolPoints;

    [Header("Settings")]
    [SerializeField] private float patrolWaitTime = 2f;
    [SerializeField] private float stopAtDistance = 0.5f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float viewAngle = 60f;
    [SerializeField] private float losePlayerTime = 3f;

    private NavMeshAgent agent;
    //private Animator animator;
    private int currentPatrolIndex;
    private bool isWaiting;
    private EnumState state = EnumState.Patrolling;

    public enum EnumState
    {
        Patrolling,
        Chasing,
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //animator = GetComponent<Animator>();
    }
    private void Start()
    {
        GoToNextPatrolPoint();
    }

    private void Update()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (state)
        {
            case EnumState.Patrolling:
                if (distanceToPlayer < detectionRange && IsPlayerInView())
                {
                    state = EnumState.Chasing;
                }
                break;
            case EnumState.Chasing:
                if (!CanSeePlayer())
                {
                    StartCoroutine(LosePlayer());
                }
                else
                {
                    agent.destination = player.position;
                }
                break;
        }
        Patrol();
       // UpdateAnimations();
    }

    private bool CanSeePlayer()
    {
        throw new NotImplementedException();
    }

    private void Patrol()
        {
        if (isWaiting) return;
        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            StartCoroutine(WaitAtPatrol());
        }
    }

    private IEnumerator WaitAtPatrol()
    {
        isWaiting = true;
        agent.isStopped = true;

        yield return new WaitForSeconds(patrolWaitTime);

        agent.isStopped = false;
        GoToNextPatrolPoint();
        isWaiting = false;
    }

    private IEnumerator LosePlayer()
    {
        yield return new WaitForSeconds(losePlayerTime);
        if (state == EnumState.Chasing)
        {
            state = EnumState.Patrolling;
            GoToNextPatrolPoint();
        }
    }

   
    private void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
            return;
        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    private void UpdateAnimations()
            {
        var isWalking = agent.velocity.magnitude > 0.1f;
       // animator.SetBool("isWalking", isWalking);
    }

    private bool IsPlayerInView()
    {
        return IsFacingPlayer() && HasClearPathToPlayer();
    }

    private bool IsFacingPlayer()
    {
        var dirToPlayer = (player.position - transform.position).normalized;
        var angle = Vector3.Angle(transform.forward, dirToPlayer);
        return angle < viewAngle / 2f;
    }

    private bool HasClearPathToPlayer()
    {
        var dirToPlayer = player.position - transform.position;
        if (Physics.Raycast(transform.position, dirToPlayer.normalized, out RaycastHit hit, dirToPlayer.magnitude))
        {
            return hit.transform == player;
        }

        return true;
    }
}   